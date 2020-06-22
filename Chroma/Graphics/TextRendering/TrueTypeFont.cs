using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.FreeType;
using Chroma.Natives.FreeType.Native;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.TextRendering
{
    public unsafe class TrueTypeFont : DisposableResource
    {
        private Log Log => LogManager.GetForCurrentAssembly();

        private int _size;
        private bool _hintingEnabled;
        private bool _forceAutoHinting;
        private HintingMode _hintingMode;

        
        internal static FreeTypeLibrary Library { get; }
        internal IntPtr Face { get; }
        internal FT_FaceRec FaceRec { get; private set; }
        internal byte[] FaceData { get; private set; }

        public string Alphabet { get; }

        public Dictionary<char, TrueTypeGlyph> RenderInfo { get; private set; }
        public Texture Atlas { get; private set; }

        public string FileName { get; }

        public int Size
        {
            get => _size;
            set
            {
                _size = value;

                ResizeFont();
                RebuildAtlas();
            }
        }

        public int ScaledLineSpacing { get; private set; }
        public int LineSpacing { get; private set; }

        public int Ascender { get; private set; }
        public int Descender { get; private set; }
        public int MaxBearing { get; private set; }

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

        public TrueTypeFont(string fileName, int size, string alphabet = null)
        {
            FileName = fileName;
            Alphabet = alphabet;
            _size = size; // do not use property here

            if (!File.Exists(fileName))
                throw new FileNotFoundException("Couldn't find the font at the provided path.", fileName);
            
            FT.FT_New_Face(Library.Native, fileName, 0, out var facePtr);
            Face = facePtr;

            InitializeFontData();
        }

        public TrueTypeFont(Stream stream, int size, string alphabet = null)
        {
            Alphabet = alphabet;
            _size = size; // do not use property here to avoid premature atlas building

            if (stream == null)
                throw new ArgumentNullException(nameof(stream), "Stream cannot be null.");
            
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            
            // needs to be class-scope property or field
            // because it gets rekt by GC when ran without
            // debugger
            FaceData = ms.ToArray();
            fixed (byte* fontPtr = &FaceData[0])
            {
                FT.FT_New_Memory_Face(
                    Library.Native,
                    new IntPtr(fontPtr),
                    FaceData.Length,
                    0,
                    out var facePtr
                );

                Face = facePtr;
            }
            
            InitializeFontData();
        }

        public bool CanRenderGlyph(char c)
            => RenderInfo.ContainsKey(c);

        public bool HasGlyph(char c)
        {
            try
            {
                return FT.FT_Get_Char_Index(Face, c) != 0;
            }
            catch (AccessViolationException ave)
            {
                Console.WriteLine(ave);
            }

            return false;
        }

        public Vector2 Measure(string text)
        {
            var width = 0f;

            var maxWidth = width;
            var maxHeight = (text.Count(c => c == '\n') + 1) * ScaledLineSpacing;

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

                var info = RenderInfo[c];
                width += info.Advance.X;
            }

            if (maxWidth < width)
                maxWidth = width;

            return new Vector2(maxWidth, maxHeight);
        }

        private void InitializeFontData()
        {
            ResizeFont();

            RenderInfo = new Dictionary<char, TrueTypeGlyph>();

            _hintingEnabled = true;
            _forceAutoHinting = true;
            _hintingMode = HintingMode.Normal;

            RebuildAtlas();
        }

        private void ResizeFont()
        {
            FT.FT_Set_Pixel_Sizes(Face, 0, (uint)Size);

            FaceRec = Marshal.PtrToStructure<FT_FaceRec>(Face);
            ScaledLineSpacing = FaceRec.size->metrics.height.ToInt32() >> 6;
            LineSpacing = FaceRec.height >> 6;

            Ascender = FaceRec.size->metrics.ascender.ToInt32() >> 6;
            Descender = (FaceRec.descender >> 6);
        }

        private void RebuildAtlas()
        {
            if (RenderInfo.Count > 0 && Atlas != null)
                InvalidateFont();

            if (string.IsNullOrEmpty(Alphabet))
                Atlas = GenerateTextureAtlas(1..512);
            else
                Atlas = GenerateTextureAtlas(Alphabet);
        }

        private void InvalidateFont()
        {
            RenderInfo.Clear();

            Atlas.Dispose();
            Atlas = null;
        }

        private Texture GenerateTextureAtlas(Range glyphRange)
        {
            var glyphs = new List<char>();

            for (var c = (char)glyphRange.Start.Value; c < (char)glyphRange.End.Value; c++)
                glyphs.Add(c);

            return GenerateTextureAtlas(glyphs);
        }

        private Texture GenerateTextureAtlas(IEnumerable<char> glyphs)
        {
            var enumerable = glyphs as char[] ?? glyphs.ToArray();

            var maxDim = (1 + FaceRec.size->metrics.height.ToInt32() >> 6) *
                         MathF.Ceiling(MathF.Sqrt(enumerable.Length));
            var texWidth = 1;

            while (texWidth < maxDim)
                texWidth <<= 1;

            var texHeight = texWidth;

            var texSize = texWidth * texHeight;
            var managedPixels = Marshal.AllocHGlobal(texWidth * texHeight);
            var pixels = (byte*)managedPixels.ToPointer();

            for (var i = 0; i < texSize; i++)
                pixels[i] = 0;

            var penX = 0;
            var penY = 0;

            foreach (var c in enumerable)
            {
                if (!HasGlyph(c))
                {
                    continue;
                }

                if (CanRenderGlyph(c))
                {
                    Log.Warning($"The font {FileName} has already generated the glyph for \\u{(int)c:X4}");
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

                FT.FT_Load_Char(Face, c, glyphFlags);
                var bmp = FaceRec.glyph->bitmap;

                if (penX + bmp.width >= texWidth)
                {
                    penX = 0;
                    penY += ((FaceRec.size->metrics.height.ToInt32() >> 6) + 1);
                }

                RenderGlyphToBitmap(bmp, penX, penY, texWidth, pixels);
                var glyph = BuildGlyphInfo(bmp, penX, penY);

                RenderInfo.Add(c, glyph);

                if (glyph.Bearing.Y > MaxBearing)
                    MaxBearing = (int)glyph.Bearing.Y;

                penX += (int)bmp.width + 1;
            }

            var tex = CreateTextureFromFTBitmap(pixels, texWidth, texHeight);

            Marshal.FreeHGlobal(managedPixels);
            return tex;
        }

        public Vector2 GetKerning(char prev, char current)
        {
            FT.FT_Get_Kerning(Face, prev, current, 0, out var kerning);
            return new Vector2(kerning.x.ToInt32() >> 6, kerning.y.ToInt32() >> 6);
        }

        private TrueTypeGlyph BuildGlyphInfo(FT_Bitmap bmp, int penX, int penY)
        {
            return new TrueTypeGlyph
            {
                Position = new Vector2(penX, penY),
                Size = new Vector2(
                    (FaceRec.glyph->metrics.width.ToInt32() >> 6),
                    FaceRec.glyph->metrics.height.ToInt32() >> 6
                ),
                BitmapSize = new Vector2(
                    (int)bmp.width,
                    (int)bmp.rows
                ),
                BitmapCoordinates = new Vector2(
                    FaceRec.glyph->bitmap_left,
                    FaceRec.glyph->bitmap_top
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

        private void RenderGlyphToBitmap(FT_Bitmap bmp, int penX, int penY, int texWidth, byte* pixels)
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

        private Texture CreateTextureFromFTBitmap(byte* pixels, int texWidth, int texHeight)
        {
            var surfaceSize = texWidth * texHeight * 4;

            var managedSurfaceData = Marshal.AllocHGlobal(surfaceSize);
            var surfaceData = (byte*)managedSurfaceData.ToPointer();

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
            Marshal.FreeHGlobal(managedSurfaceData);

            return new Texture(gpuImage);
        }

        private bool IsMonochromeBitSet(FT_GlyphSlotRec* glyph, int x, int y)
        {
            var pitch = glyph->bitmap.pitch;
            var buf = (byte*)glyph->bitmap.buffer.ToPointer();

            var row = &buf[pitch * y];
            var value = row[x >> 3];

            return (value & (0x80 >> (x & 7))) != 0;
        }

        protected override void FreeManagedResources()
        {
            if (FaceData != null && FaceData.Length > 0)
                FaceData = null;
        }

        protected override void FreeNativeResources()
        {
            FT.FT_Done_Face(Face);
        }
    }
}