using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;

namespace Chroma.Graphics.TextRendering.Bitmap
{
    public class BitmapFont : DisposableResource
    {
        private List<string> _lines;
        private Dictionary<string, Action> _parsers;
        private BitmapFontLexer _lexer;

        private Log Log => LogManager.GetForCurrentAssembly();

        public string FileName { get; }

        public BitmapFontInfo Info { get; private set; }
        public BitmapFontCommon Common { get; private set; }
        public int DeclaredCharCount { get; private set; }

        public List<BitmapFontPage> Pages { get; }
        public Dictionary<char, BitmapGlyph> Glyphs { get; }

        public BitmapFont(string fileName)
        {
            FileName = fileName;

            Pages = new List<BitmapFontPage>();
            Glyphs = new Dictionary<char, BitmapGlyph>();

            using (var sr = new StreamReader(FileName))
                _lines = sr.ReadToEnd().Split('\n').ToList();

            _parsers = new Dictionary<string, Action>
            {
                {"info", ParseFontInformation},
                {"common", ParseFontCommons},
                {"page", ParsePage},
                {"chars", ParseCharCount},
                {"char", ParseChar}
            };

            ParseFontDefinition();
        }

        public bool HasGlyph(char c)
            => Glyphs.ContainsKey(c);

        public Size Measure(string s)
        {
            var maxW = 0;
            var w = 0;
            var h = Common.LineHeight;

            foreach (var c in s)
            {
                if (c == '\n')
                {
                    h += Common.LineHeight;
                    maxW = w;
                    w = 0;
                    continue;
                }

                if (!HasGlyph(c))
                    continue;

                w += Glyphs[c].Width + Glyphs[c].HorizontalAdvance + Glyphs[c].OffsetX;
            }

            if (w > maxW)
                maxW = w;
            
            return new Size(maxW, h);
        }

        protected override void FreeManagedResources()
        {
            Log.Debug($"Disposing {Info.FaceName}.");

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
                words.RemoveAll(x => string.IsNullOrWhiteSpace(x));

                var verb = words[0];
                var arguments = string.Join(' ', words.Skip(1));

                if (_parsers.ContainsKey(verb))
                {
                    _lexer = new BitmapFontLexer(arguments);
                    _parsers[verb]();
                }
                else
                {
                    Log.Warning($"Unexpected BMFont block '{verb}'");
                }
            }

            Log.Debug($"Expected {DeclaredCharCount}, parsed: {Glyphs.Count}.");
        }

        private void ParseFontInformation()
        {
            Info = new BitmapFontInfo();
            Log.Debug("Parsing font information block.");

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
                        Log.Warning($"Unexpected info parameter '{_lexer.CurrentKey}'");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }
        }

        private void ParseFontCommons()
        {
            Common = new BitmapFontCommon();
            Log.Debug("Parsing font commons block.");

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
                        Log.Warning($"Unexpected common parameter '{_lexer.CurrentKey}'.");
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
            Log.Debug("Parsing font page definition.");

            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "id":
                        id = GetInteger(_lexer.CurrentValue);
                        break;

                    case "file":
                        fileName = Path.Combine(
                            Path.GetDirectoryName(FileName),
                            _lexer.CurrentValue
                        );
                        break;

                    default:
                        Log.Warning($"Unexpected page definition parameter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }

            if (id >= 0)
            {
                Pages.Add(new BitmapFontPage(id, fileName));
            }
            else
            {
                Log.Warning("Failed to parse page definition. Invalid page definition line?");
            }
        }

        private void ParseCharCount()
        {
            Log.Debug("Parsing char count declaration.");

            while (true)
            {
                switch (_lexer.CurrentKey)
                {
                    case "count":
                        DeclaredCharCount = GetInteger(_lexer.CurrentValue);
                        break;

                    default:
                        Log.Warning($"Unexpected char count declaration paramter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }
        }

        private void ParseChar()
        {
            Log.Debug("Parsing glyph definition.");

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
                        Log.Warning($"Unexpected glyph definition parameter '{_lexer.CurrentKey}'.");
                        break;
                }

                if (_lexer.IsEOL) break;
                else _lexer.Next();
            }

            Glyphs.Add(glyph.CodePoint, glyph);
        }

        private bool GetBoolean(string value)
            => value != "0";

        private int GetInteger(string value)
            => int.Parse(value);

        private Vector4 GetPadding(string value)
        {
            var values = value.Split(',').Select(x => int.Parse(x)).ToArray();
            return new Vector4(values[0], values[1], values[2], values[3]);
        }

        private Vector2 GetSpacing(string value)
        {
            var values = value.Split(',').Select(x => int.Parse(x)).ToArray();
            return new Vector2(values[0], values[1]);
        }
    }
}