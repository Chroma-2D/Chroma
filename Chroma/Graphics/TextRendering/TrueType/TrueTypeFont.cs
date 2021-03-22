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
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        private int _height;
        private bool _hintingEnabled;
        private bool _forceAutoHinting;
        private HintingMode _hintingMode;
        private IntPtr _facePtr;
        private int _maxBearing;

        internal static FreeTypeLibrary Library { get; }

        internal FT_FaceRec FaceRec { get; private set; }
        internal byte[] TtfData { get; private set; }
        internal Dictionary<char, TrueTypeGlyph> Glyphs { get; } = new();

        public static TrueTypeFont Default => EmbeddedAssets.DefaultFont;

        public string FamilyName => Marshal.PtrToStringAnsi(FaceRec.family_name);
        public IReadOnlyCollection<char> Alphabet { get; private set; }
        public Texture Atlas { get; private set; }

        public bool IsKerningEnabled { get; set; } = true;
        public int LineSpacing { get; private set; }

        public int Height
        {
            get => _height;
            set
            {
                _height = value;

                ResizeFont();
                RebuildAtlas();
            }
        }

        public bool ForceAutoHinting
        {
            get => _forceAutoHinting;
            set
            {
                _forceAutoHinting = value;
                RebuildAtlas();
            }
        }

        public bool HintingEnabled
        {
            get => _hintingEnabled;
            set
            {
                _hintingEnabled = value;
                RebuildAtlas();
            }
        }

        public HintingMode HintingMode
        {
            get => _hintingMode;
            set
            {
                _hintingMode = value;
                RebuildAtlas();
            }
        }

        static TrueTypeFont()
        {
            Library = new FreeTypeLibrary();
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
                TtfData = ms.ToArray();

                unsafe
                {
                    fixed (byte* fontPtr = &TtfData[0])
                    {
                        FT.FT_New_Memory_Face(
                            Library.Native,
                            new IntPtr(fontPtr),
                            TtfData.Length,
                            0,
                            out _facePtr
                        );
                    }
                }
            }

            InitializeFontData();
        }

        public bool HasGlyph(char c)
            => Glyphs.ContainsKey(c);

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

                var info = Glyphs[c];
                width += (int)info.Advance.X;
            }

            if (maxWidth < width)
                maxWidth = width;

            return new Size(maxWidth, maxHeight);
        }

        public Texture GetTexture(char c = (char)0)
            => Atlas;

        public int GetHorizontalAdvance(char c)
            => (int)Glyphs[c].Advance.X;

        public Rectangle GetGlyphBounds(char c)
        {
            if (!HasGlyph(c))
                return Rectangle.Empty;

            return new(
                (int)Glyphs[c].Position.X,
                (int)Glyphs[c].Position.Y,
                (int)Glyphs[c].Size.X,
                (int)Glyphs[c].Size.Y
            );
        }

        public Vector2 GetRenderOffsets(char c)
        {
            return new(
                Glyphs[c].Bearing.X,
                -Glyphs[c].Bearing.Y + _maxBearing
            );
        }

        public int GetKerning(char left, char right)
        {
            FT.FT_Get_Kerning(_facePtr, left, right, 0, out var kerning);
            return kerning.x.ToInt32() >> 6;
        }

        private void InitializeFontData()
        {
            ResizeFont();

            _hintingEnabled = true;
            _forceAutoHinting = true;
            _hintingMode = HintingMode.Normal;

            RebuildAtlas();
        }

        private void ResizeFont()
        {
            FT.FT_Set_Pixel_Sizes(_facePtr, 0, (uint)Height);

            FaceRec = Marshal.PtrToStructure<FT_FaceRec>(_facePtr);

            unsafe
            {
                LineSpacing = FaceRec.size->metrics.height.ToInt32() >> 6;
            }
        }

        private void RebuildAtlas()
        {
            if (Glyphs.Count > 0 && Atlas != null)
                InvalidateFont();

            Atlas = (Alphabet == null)
                ? GenerateTextureAtlas(1..512)
                : GenerateTextureAtlas(Alphabet);
        }

        private void InvalidateFont()
        {
            Glyphs.Clear();

            Atlas.Dispose();
            Atlas = null;
        }

        private Texture GenerateTextureAtlas(params Range[] ranges)
        {
            var glyphs = new List<char>();

            foreach (var range in ranges)
            {
                for (var c = (char)range.Start.Value; c < (char)range.End.Value; c++)
                    glyphs.Add(c);
            }

            Alphabet = glyphs;
            return GenerateTextureAtlas(glyphs);
        }

        private bool TtfContainsGlyph(char c)
            => FT.FT_Get_Char_Index(_facePtr, c) != 0;

        private unsafe Texture GenerateTextureAtlas(IEnumerable<char> glyphs)
        {
            var array = glyphs as char[] ?? glyphs.ToArray();

            var maxDim = (1 + FaceRec.size->metrics.height.ToInt32() >> 6) *
                         MathF.Ceiling(MathF.Sqrt(array.Length));

            var texWidth = 1;

            while (texWidth < maxDim)
                texWidth <<= 1;

            var texHeight = texWidth;

            var texSize = texWidth * texHeight * 4;

            var pixelData = new byte[texSize];

            fixed (byte* pixels = &pixelData[0])
            {
                for (var i = 0; i < texSize; i++)
                    pixels[i] = 0;

                var penX = 0;
                var penY = 0;

                for (var i = 0; i < array.Length; i++)
                {
                    var character = array[i];

                    if (!TtfContainsGlyph(character))
                        continue;

                    if (HasGlyph(character))
                    {
                        _log.Warning($"The font {FamilyName} has already generated the glyph for \\u{(int)character:X4}");
                        continue;
                    }

                    var glyphFlags = FT.FT_LOAD_RENDER;

                    if (ForceAutoHinting)
                    {
                        glyphFlags |= FT.FT_LOAD_FORCE_AUTOHINT;
                    }

                    if (HintingEnabled)
                    {
                        glyphFlags |= HintingMode switch
                        {
                            HintingMode.Normal => FT.FT_LOAD_TARGET_NORMAL,
                            HintingMode.Light => FT.FT_LOAD_TARGET_LIGHT,
                            HintingMode.Monochrome => FT.FT_LOAD_TARGET_MONO,
                            _ => throw new InvalidOperationException("Unsupported hinting mode.")
                        };
                    }
                    else
                    {
                        glyphFlags |= FT.FT_LOAD_MONOCHROME;
                    }

                    FT.FT_Load_Char(_facePtr, character, glyphFlags);
                    var bmp = FaceRec.glyph->bitmap;

                    if (penX + bmp.width >= texWidth)
                    {
                        penX = 0;
                        penY += ((FaceRec.size->metrics.height.ToInt32() >> 6) + 1);
                    }

                    RenderGlyphToBitmap(bmp, penX, penY, texWidth, pixels);
                    var glyph = BuildGlyphInfo(bmp, penX, penY);

                    Glyphs.Add(character, glyph);

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
            return new()
            {
                Position = new Vector2(penX, penY),
                Size = new Vector2(
                    (int)bmp.width,
                    (int)bmp.rows
                ),
                Bearing = new Vector2(
                    FaceRec.glyph->metrics.horiBearingX.ToInt32() >> 6,
                    FaceRec.glyph->metrics.horiBearingY.ToInt32() >> 6
                ),
                Advance = new Vector2(
                    FaceRec.glyph->advance.x.ToInt32() >> 6,
                    FaceRec.glyph->advance.y.ToInt32() >> 6
                )
            };
        }

        private unsafe void RenderGlyphToBitmap(FT_Bitmap bmp, int penX, int penY, int texWidth, byte* pixels)
        {
            var buffer = (byte*)FaceRec.glyph->bitmap.buffer.ToPointer();

            for (var row = 0; row < bmp.rows; ++row)
            {
                for (var col = 0; col < bmp.width; ++col)
                {
                    var x = penX + col;
                    var y = penY + row;

                    if (HintingEnabled && HintingMode != HintingMode.Monochrome)
                    {
                        pixels[y * texWidth + x] = buffer[row * bmp.pitch + col];
                    }
                    else
                    {
                        pixels[y * texWidth + x] =
                            IsMonochromeBitSet(FaceRec.glyph, col, row) ? (byte)0xFF : (byte)0x00;
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
                for (var i = 0; i < surfaceSize; i++)
                    surfaceData[i] = 0;

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
                SDL2.SDL_SetSurfaceBlendMode(surface, SDL2.SDL_BlendMode.SDL_BLENDMODE_BLEND);
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
            if (TtfData != null && TtfData.Length > 0)
                TtfData = null;
        }

        protected override void FreeNativeResources()
        {
            FT.FT_Done_Face(_facePtr);
        }
    }
}