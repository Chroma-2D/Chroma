using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;

namespace Chroma.Graphics.TextRendering.Bitmap
{
    public class BitmapFont : DisposableResource, IFontProvider
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        private readonly Func<string, Texture> _pageTextureLookup;

        private List<string> _lines;
        private Dictionary<string, Action> _parsers;

        private BitmapFontLexer _lexer;

        private BitmapFontInfo Info { get; set; }
        private BitmapFontCommon Common { get; set; }

        private List<BitmapFontPage> Pages { get; } = new();
        private List<BitmapFontKerningPair> Kernings { get; } = new();
        private Dictionary<char, BitmapGlyph> Glyphs { get; } = new();

        public string FamilyName => Info.FaceName;
        public string FileName { get; }

        public int DeclaredCharCount { get; private set; }
        public int DeclaredKerningCount { get; private set; }

        public bool IsKerningEnabled { get; set; }

        public int Height => Info.Size;
        public int LineSpacing => (int)(Common.LineHeight + Info.Spacing.Y);

        public BitmapFont(string fileName, Stream dataStream, Func<string, Texture> pageTextureLookup = null)
        {
            FileName = fileName;
            _pageTextureLookup = pageTextureLookup;

            Initialize(dataStream);
        }

        public BitmapFont(string fileName, Func<string, Texture> pageTextureLookup = null)
        {
            FileName = fileName;
            _pageTextureLookup = pageTextureLookup;

            using (var fs = new FileStream(FileName, FileMode.Open))
            {
                Initialize(fs);
            }
        }

        private void Initialize(Stream dataStream)
        {
            using (var sr = new StreamReader(dataStream))
                _lines = sr.ReadToEnd().Split('\n').ToList();

            _parsers = new Dictionary<string, Action>
            {
                { "info", ParseFontInformation },
                { "common", ParseFontCommons },
                { "page", ParsePage },
                { "chars", ParseCharCount },
                { "char", ParseChar },
                { "kernings", ParseKerningCount },
                { "kerning", ParseKerningInfo }
            };

            ParseFontDefinition();
        }

        public bool HasGlyph(char c)
            => Glyphs.ContainsKey(c);

        public int GetKerning(char first, char second)
            => Kernings.FirstOrDefault(x => x.First == first && x.Second == second).Amount;

        public Size Measure(string text)
        {
            var maxWidth = 0;
            var width = 0;
            var height = LineSpacing;

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];

                if (c == '\n')
                {
                    height += LineSpacing;

                    if (width > maxWidth)
                        maxWidth = width;

                    width = 0;
                    continue;
                }

                if (!HasGlyph(c))
                    continue;

                width += Glyphs[c].HorizontalAdvance;
            }

            if (width > maxWidth)
                maxWidth = width;

            return new Size(maxWidth, height);
        }

        public Texture GetTexture(char c)
        {
            if (Pages.Count == 0)
                return null;

            if (!HasGlyph(c))
                return null;

            var glyph = Glyphs[c];

            return Pages[glyph.Page].Texture;
        }

        public Rectangle GetGlyphBounds(char c)
        {
            if (!HasGlyph(c))
                return Rectangle.Empty;

            return new(
                Glyphs[c].BitmapX,
                Glyphs[c].BitmapY,
                Glyphs[c].Width,
                Glyphs[c].Height
            );
        }

        public Vector2 GetRenderOffsets(char c)
        {
            if (!HasGlyph(c))
                return Vector2.Zero;

            return new(Glyphs[c].OffsetX, Glyphs[c].OffsetY);
        }

        public int GetHorizontalAdvance(char c)
        {
            if (!HasGlyph(c))
                return 0;

            return Glyphs[c].HorizontalAdvance;
        }

        public Texture GetTexture(int page)
        {
            if (!Pages.Any())
                return null;

            if (page < 0 || page >= Pages.Count)
                throw new ArgumentOutOfRangeException(nameof(page), "Page number invalid.");

            return Pages[page].Texture;
        }

        protected override void FreeManagedResources()
        {
            _log.Debug($"Disposing {Info.FaceName}.");

            foreach (var page in Pages)
                page.Texture.Dispose();
        }

        private void ParseFontDefinition()
        {
            foreach (var line in _lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var words = line.Trim().Split(' ').ToList();
                words.RemoveAll(string.IsNullOrWhiteSpace);

                var verb = words[0];
                var arguments = string.Join(' ', words.Skip(1));

                if (_parsers.ContainsKey(verb))
                {
                    _lexer = new BitmapFontLexer(arguments);
                    _parsers[verb]();
                }
                else
                {
                    _log.Warning($"Unexpected BMFont block '{verb}'");
                }
            }

            _log.Debug($"Expected {DeclaredCharCount}, parsed: {Glyphs.Count}.");
        }

        private void ParseFontInformation()
        {
            Info = new BitmapFontInfo();

            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "face":
                        Info.FaceName = _lexer.CurrentValue;
                        break;

                    case "size":
                        Info.Size = GetInteger(_lexer.CurrentValue);
                        break;

                    case "bold":
                        Info.IsBold = GetBoolean(_lexer.CurrentValue);
                        break;

                    case "italic":
                        Info.IsItalic = GetBoolean(_lexer.CurrentValue);
                        break;

                    case "charset":
                        Info.CharSet = _lexer.CurrentValue;
                        break;

                    case "unicode":
                        Info.IsUnicode = GetBoolean(_lexer.CurrentValue);
                        break;

                    case "stretchH":
                        Info.ScalePercent = GetInteger(_lexer.CurrentValue);
                        break;

                    case "smooth":
                        Info.IsSmooth = GetBoolean(_lexer.CurrentValue);
                        break;

                    case "aa":
                        Info.IsSuperSampled = GetBoolean(_lexer.CurrentValue);
                        break;

                    case "padding":
                        Info.Padding = GetPadding(_lexer.CurrentValue);
                        break;

                    case "spacing":
                        Info.Spacing = GetSpacing(_lexer.CurrentValue);
                        break;

                    case "outline":
                        Info.OutlineThickness = GetInteger(_lexer.CurrentValue);
                        break;

                    default:
                        _log.Warning($"Unexpected info parameter '{_lexer.CurrentKey}'");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }
        }

        private void ParseFontCommons()
        {
            Common = new BitmapFontCommon();

            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "lineHeight":
                        Common.LineHeight = GetInteger(_lexer.CurrentValue);
                        break;

                    case "base":
                        Common.BaseLine = GetInteger(_lexer.CurrentValue);
                        break;

                    case "scaleW":
                        Common.ScaleW = GetInteger(_lexer.CurrentValue);
                        break;

                    case "scaleH":
                        Common.ScaleH = GetInteger(_lexer.CurrentValue);
                        break;

                    case "pages":
                        Common.PageCount = GetInteger(_lexer.CurrentValue);
                        break;

                    case "packed":
                        Common.IsPacked = GetBoolean(_lexer.CurrentValue);
                        break;

                    case "alphaChnl":
                        Common.AlphaMode = (BitmapFontChannelMode)GetInteger(_lexer.CurrentValue);
                        break;

                    case "redChnl":
                        Common.RedMode = (BitmapFontChannelMode)GetInteger(_lexer.CurrentValue);
                        break;

                    case "greenChnl":
                        Common.GreenMode = (BitmapFontChannelMode)GetInteger(_lexer.CurrentValue);
                        break;

                    case "blueChnl":
                        Common.BlueMode = (BitmapFontChannelMode)GetInteger(_lexer.CurrentValue);
                        break;

                    default:
                        _log.Warning($"Unexpected common parameter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }
        }

        private void ParsePage()
        {
            var id = -1;
            var fileName = string.Empty;

            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "id":
                        id = GetInteger(_lexer.CurrentValue);
                        break;

                    case "file":
                        fileName = _lexer.CurrentValue;
                        break;

                    default:
                        _log.Warning($"Unexpected page definition parameter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }

            if (id >= 0)
            {
                Pages.Add(new BitmapFontPage(id, Path.GetDirectoryName(FileName), fileName, _pageTextureLookup));
            }
            else
            {
                _log.Warning("Failed to parse page definition. Invalid page definition line?");
            }
        }

        private void ParseCharCount()
        {
            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "count":
                        DeclaredCharCount = GetInteger(_lexer.CurrentValue);
                        break;

                    default:
                        _log.Warning($"Unexpected char count declaration paramter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }
        }

        private void ParseChar()
        {
            var glyph = new BitmapGlyph();

            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "id":
                        glyph.CodePoint = (char)GetInteger(_lexer.CurrentValue);
                        break;

                    case "x":
                        glyph.BitmapX = GetInteger(_lexer.CurrentValue);
                        break;

                    case "y":
                        glyph.BitmapY = GetInteger(_lexer.CurrentValue);
                        break;

                    case "width":
                        glyph.Width = GetInteger(_lexer.CurrentValue);
                        break;

                    case "height":
                        glyph.Height = GetInteger(_lexer.CurrentValue);
                        break;

                    case "xoffset":
                        glyph.OffsetX = GetInteger(_lexer.CurrentValue);
                        break;

                    case "yoffset":
                        glyph.OffsetY = GetInteger(_lexer.CurrentValue);
                        break;

                    case "page":
                        glyph.Page = GetInteger(_lexer.CurrentValue);
                        break;

                    case "chnl":
                        glyph.Channel = GetInteger(_lexer.CurrentValue);
                        break;

                    case "xadvance":
                        glyph.HorizontalAdvance = GetInteger(_lexer.CurrentValue);
                        break;

                    default:
                        _log.Warning($"Unexpected glyph definition parameter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }

            Glyphs.Add(glyph.CodePoint, glyph);
        }

        private void ParseKerningCount()
        {
            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "count":
                        DeclaredKerningCount = GetInteger(_lexer.CurrentValue);
                        break;

                    default:
                        _log.Warning($"Unexpected kerning count parameter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }
        }

        private void ParseKerningInfo()
        {
            var kerning = new BitmapFontKerningPair();

            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "first":
                        kerning.First = (char)GetInteger(_lexer.CurrentValue);
                        break;

                    case "second":
                        kerning.Second = (char)GetInteger(_lexer.CurrentValue);
                        break;

                    case "amount":
                        kerning.Amount = GetInteger(_lexer.CurrentValue);
                        break;

                    default:
                        _log.Warning($"Unexpected kerning info parameter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }

            Kernings.Add(kerning);
        }

        private bool GetBoolean(string value)
            => value != "0";

        private int GetInteger(string value)
            => int.Parse(value, CultureInfo.InvariantCulture);

        private Vector4 GetPadding(string value)
        {
            var values = value.Split(',').Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            return new Vector4(values[0], values[1], values[2], values[3]);
        }

        private Vector2 GetSpacing(string value)
        {
            var values = value.Split(',').Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            return new Vector2(values[0], values[1]);
        }
    }
}