using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.FreeType;
using Chroma.Natives.FreeType.Native;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.TextRendering.TrueType
{
    public class TrueTypeFont : DisposableResource, IFontProvider
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        private static readonly FreeTypeLibrary _library;

        private unsafe FT_FaceRec* _face;

        private int _height;
        private bool _hintingEnabled;
        private bool _preferAutoHinter;
        private HintingMode _hintingMode;
        private KerningMode _kerningMode;

        private int _maxBearing;

        private readonly Dictionary<char, TrueTypeGlyph> _glyphs = new();
        private readonly Dictionary<int, int> _kernings = new();
        
        private byte[] _ttfData;
        private Texture _atlas;

        public static TrueTypeFont Default => EmbeddedAssets.DefaultFont;

        public string FamilyName
        {
            get
            {
                unsafe
                {
                    return Marshal.PtrToStringAnsi(_face->family_name);
                }
            }
        }

        public IReadOnlyCollection<char> Alphabet { get; private set; }

        public bool IsKerningEnabled { get; set; } = true;
        public int LineSpacing { get; private set; }

        public int MaximumBearing => _maxBearing;

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                InitializeFontData();
            }
        }

        public bool PreferAutoHinter
        {
            get => _preferAutoHinter;
            set
            {
                _preferAutoHinter = value;
                InitializeFontData();
            }
        }

        public bool HintingEnabled
        {
            get => _hintingEnabled;
            set
            {
                _hintingEnabled = value;
                InitializeFontData();
            }
        }

        public HintingMode HintingMode
        {
            get => _hintingMode;
            set
            {
                _hintingMode = value;
                InitializeFontData();
            }
        }

        public KerningMode KerningMode
        {
            get => _kerningMode;
            set
            {
                _kerningMode = value;
                InitializeFontData();
            }
        }

        public static string FreeTypeVersion
        {
            get
            {
                FT.FT_Library_Version(
                    _library.Native,
                    out var major,
                    out var minor,
                    out var patch
                );

                return $"{major}.{minor}.{patch}";
            }
        }

        static TrueTypeFont()
        {
            _library = new FreeTypeLibrary();
        }

        public TrueTypeFont(string fileName, int height, string alphabet = null)
            : this(File.OpenRead(fileName), height, alphabet)
        {
        }

        public TrueTypeFont(Stream stream, int height, string alphabet = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "TTF stream cannot be null.");

            Alphabet = alphabet?.ToCharArray();
            _height = height; // do not use property here to avoid premature atlas building

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                _ttfData = ms.ToArray();
            }

            _hintingEnabled = true;
            _preferAutoHinter = true;
            _hintingMode = HintingMode.Normal;

            InitializeFontData();
        }

        public void GetGlyphControlBox(char c, out int xMin, out int xMax, out int yMin, out int yMax)
        {
            xMin = 0;
            xMax = 0;
            
            yMin = 0;
            yMax = 0;
            
            if (!HasGlyph(c))
                return;

            xMin = _glyphs[c].MinimumX;
            xMax = _glyphs[c].MaximumX;
            
            yMin = _glyphs[c].MinimumY;
            yMax = _glyphs[c].MaximumY;
        }

        public bool HasGlyph(char c)
            => _glyphs.ContainsKey(c);

        public Size Measure(string text)
        {
            var width = 0;

            var maxWidth = width;
            var maxHeight = (text.Count(c => c == '\n') + 1) * LineSpacing;

            foreach (var c in text)
            {
                if (c == '\n')
                {
                    if (maxWidth < width)
                        maxWidth = width;

                    width = 0;
                    continue;
                }

                if (!HasGlyph(c))
                    continue;

                var info = _glyphs[c];
                width += (int)info.Advance.X;
            }

            if (maxWidth < width)
                maxWidth = width;

            return new Size(maxWidth, maxHeight);
        }

        public Texture GetTexture(char c = (char)0)
            => _atlas;

        public int GetHorizontalAdvance(char c)
            => (int)_glyphs[c].Advance.X;

        public Rectangle GetGlyphBounds(char c)
        {
            if (!HasGlyph(c))
                return Rectangle.Empty;

            return new(
                (int)_glyphs[c].Position.X,
                (int)_glyphs[c].Position.Y,
                (int)_glyphs[c].Size.X,
                (int)_glyphs[c].Size.Y
            );
        }

        public Vector2 GetRenderOffsets(char c)
        {
            return new(
                _glyphs[c].Bearing.X,
                -_glyphs[c].Bearing.Y + _maxBearing
            );
        }

        public int GetKerning(char first, char second)
        {
            var key = (first << 16) | second;
            
            _kernings.TryGetValue(key, out var kerning);
            return kerning;
        }

        private void InitializeFontData()
        {
            InvalidateFont();

            LoadTtf();
            CreateAlphabetIfNeeded();
            ResizeFont();
            RetrieveKerningData();
            RebuildAtlas();
            UnloadTtf();
        }

        private void InvalidateFont()
        {
            _atlas?.Dispose();
            _atlas = null;
            
            _glyphs.Clear();
            _kernings.Clear();
        }

        private unsafe void LoadTtf()
        {
            fixed (byte* fontPtr = &_ttfData[0])
            {
                FT.FT_New_Memory_Face(
                    _library.Native,
                    new IntPtr(fontPtr),
                    _ttfData.Length,
                    0,
                    out _face
                );
            }
        }

        private void CreateAlphabetIfNeeded()
        {
            Alphabet ??= GenerateAlphabet(1..512);
        }

        private unsafe void UnloadTtf()
        {
            if (_face != null)
            {
                FT.FT_Done_Face(_face);
                _face = null;
            }
        }

        private unsafe void ResizeFont()
        {
            FT.FT_Set_Pixel_Sizes(_face, 0, (uint)Height);
            LineSpacing = _face->size->metrics.height.ToInt32() >> 6;
        }

        private void RebuildAtlas()
        {
            _atlas = GenerateTextureAtlas(Alphabet);
        }

        private void RetrieveKerningData()
        {
            for (var i = 0; i < Alphabet.Count; i++)
            {
                var left = Alphabet.ElementAt(i);
                var right = Alphabet.ElementAt(Alphabet.Count - i - 1);

                var key = (left << 16) | right;
                
                var amount = GetTtfKerning(left, right);
                _kernings.Add(key, amount);
            }
        }

        private List<char> GenerateAlphabet(params Range[] ranges)
        {
            var glyphs = new List<char>();

            foreach (var range in ranges)
            {
                for (var c = (char)range.Start.Value; c < (char)range.End.Value; c++)
                    glyphs.Add(c);
            }

            return glyphs;
        }

        private bool TtfContainsGlyph(char c)
        {
            unsafe
            {
                return FT.FT_Get_Char_Index(_face, c) != 0;
            }
        }

        private unsafe int GetTtfKerning(char left, char right)
        {
            var leftIndex = FT.FT_Get_Char_Index(_face, left);
            var rightIndex = FT.FT_Get_Char_Index(_face, right);

            if (leftIndex == 0 || rightIndex == 0)
                return 0;

            FT.FT_Get_Kerning(_face, leftIndex, rightIndex, (uint)_kerningMode, out var kerning);
            return kerning.x.ToInt32() >> 6;
        }

        private unsafe Texture GenerateTextureAtlas(IEnumerable<char> glyphs)
        {
            var array = glyphs as char[] ?? glyphs.ToArray();
            var maxDim = (1 + LineSpacing) * MathF.Ceiling(MathF.Sqrt(array.Length));

            var texWidth = 1;
            while (texWidth < maxDim)
                texWidth <<= 1;

            var texHeight = texWidth;

            var texSize = texWidth * texHeight * 4;
            var pixelData = new byte[texSize];

            fixed (byte* pixels = &pixelData[0])
            {
                var penX = 0;
                var penY = 0;

                for (var i = 0; i < array.Length; i++)
                {
                    var character = array[i];

                    if (!TtfContainsGlyph(character))
                        continue;

                    if (HasGlyph(character))
                    {
                        _log.Warning(
                            $"The font {FamilyName} has already generated the glyph for \\u{character:X4}");
                        continue;
                    }

                    var glyphFlags = FT.FT_LOAD_RENDER | FT.FT_LOAD_PEDANTIC;
                    var renderMode = FT_Render_Mode.FT_RENDER_MODE_NORMAL;

                    if (HintingEnabled)
                    {
                        if (PreferAutoHinter)
                        {
                            glyphFlags |= FT.FT_LOAD_FORCE_AUTOHINT;
                        }
                        
                        switch (HintingMode)
                        {
                            case HintingMode.Normal:
                                glyphFlags |= FT.FT_LOAD_TARGET_NORMAL;
                                break;

                            case HintingMode.Light:
                                glyphFlags |= FT.FT_LOAD_TARGET_LIGHT;
                                renderMode = FT_Render_Mode.FT_RENDER_MODE_LIGHT;
                                break;

                            case HintingMode.Monochrome:
                                glyphFlags |= FT.FT_LOAD_TARGET_MONO;
                                glyphFlags |= FT.FT_LOAD_MONOCHROME;

                                renderMode = FT_Render_Mode.FT_RENDER_MODE_MONO;
                                break;

                            default: 
                                throw new InvalidOperationException("Unsupported hinting mode.");
                        }
                    }
                    else
                    {
                        glyphFlags |= FT.FT_LOAD_NO_HINTING;
                    }

                    var index = FT.FT_Get_Char_Index(_face, character);
                    FT.FT_Load_Glyph(_face, index, glyphFlags);
                    FT.FT_Render_Glyph(_face->glyph, renderMode);

                    var bmp = _face->glyph->bitmap;
                    if (penX + bmp.width >= texWidth)
                    {
                        penX = 0;
                        penY += ((_face->size->metrics.height.ToInt32() >> 6) + 1);
                    }

                    RenderGlyphToBitmap(bmp, penX, penY, texWidth, pixels);
                    var glyph = BuildGlyphInfo(bmp, penX, penY);

                    _glyphs.Add(character, glyph);

                    if (glyph.Bearing.Y > _maxBearing)
                        _maxBearing = (int)glyph.Bearing.Y;

                    penX += (int)bmp.width + 1;
                }

                var tex = CreateTextureFromFTBitmap(pixels, texWidth, texHeight);
                return tex;
            }
        }

        private unsafe TrueTypeGlyph BuildGlyphInfo(FT_Bitmap bmp, int penX, int penY)
        {
            FT.FT_Get_Glyph((IntPtr)_face->glyph, out var glyph);
            FT.FT_Glyph_Get_CBox(
                glyph,
                FT_Glyph_BBox_Mode.FT_GLYPH_BBOX_PIXELS,
                out var cbox
            );
            
            return new()
            {
                Position = new Vector2(penX, penY),
                Size = new Vector2(
                    (int)bmp.width,
                    (int)bmp.rows
                ),
                Bearing = new Vector2(
                    _face->glyph->metrics.horiBearingX.ToInt32() >> 6,
                    _face->glyph->metrics.horiBearingY.ToInt32() >> 6
                ),
                Advance = new Vector2(
                    _face->glyph->advance.x.ToInt32() >> 6,
                    _face->glyph->advance.y.ToInt32() >> 6
                ),
                MaximumX = cbox.xMax.ToInt32(),
                MaximumY = cbox.yMax.ToInt32(),
                MinimumX = cbox.xMin.ToInt32(),
                MinimumY = cbox.yMin.ToInt32()
            };
        }

        private unsafe void RenderGlyphToBitmap(FT_Bitmap bmp, int penX, int penY, int texWidth, byte* pixels)
        {
            var buffer = (byte*)_face->glyph->bitmap.buffer.ToPointer();

            for (var row = 0; row < bmp.rows; ++row)
            {
                for (var col = 0; col < bmp.width; ++col)
                {
                    var x = penX + col;
                    var y = penY + row;

                    if (HintingEnabled && HintingMode == HintingMode.Monochrome)
                    {
                        pixels[y * texWidth + x] =
                            IsMonochromeBitSet(_face->glyph, col, row) ? (byte)0xFF : (byte)0x00;
                    }
                    else
                    {
                        pixels[y * texWidth + x] = buffer[row * bmp.pitch + col];
                    }
                }
            }
        }

        private unsafe Texture CreateTextureFromFTBitmap(byte* pixels, int texWidth, int texHeight)
        {
            var surfaceSize = texWidth * texHeight * 4;

            var managedSurfaceData = new byte[surfaceSize];
            fixed (byte* surfaceData = &managedSurfaceData[0])
            {
                for (var i = 0; i < texWidth * texHeight; ++i)
                {
                    surfaceData[i * 4 + 0] = 0xFF;
                    surfaceData[i * 4 + 1] = 0xFF;
                    surfaceData[i * 4 + 2] = 0xFF;
                    surfaceData[i * 4 + 3] |= pixels[i];
                }

                var surface = SDL2.SDL_CreateRGBSurfaceFrom(
                    new IntPtr(surfaceData),
                    texWidth,
                    texHeight,
                    32,
                    texWidth * 4,
                    0x000000FF,
                    0x0000FF00,
                    0x00FF0000,
                    0xFF000000
                );

                var gpuImage = SDL_gpu.GPU_CopyImageFromSurface(surface);

                SDL2.SDL_FreeSurface(surface);
                return new Texture(gpuImage);
            }
        }

        private unsafe bool IsMonochromeBitSet(FT_GlyphSlotRec* glyph, int x, int y)
        {
            var pitch = glyph->bitmap.pitch;
            var buf = (byte*)glyph->bitmap.buffer.ToPointer();

            var row = &buf[pitch * y];
            var value = row[x >> 3];

            return (value & (0x80 >> (x & 7))) != 0;
        }

        protected override void FreeManagedResources()
        {
            InvalidateFont();
            
            if (_ttfData is { Length: > 0 })
                _ttfData = null;
        }
    }
}