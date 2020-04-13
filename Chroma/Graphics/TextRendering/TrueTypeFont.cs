using System;
using System.Collections.Generic;
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

        internal static FreeTypeLibrary Library { get; }
        internal IntPtr Face { get; }
        internal FT_FaceRec FaceRec { get; }

        public Dictionary<char, TrueTypeGlyph> RenderInfo { get; }
        public Texture Atlas { get; private set; }

        public string FileName { get; }
        public int Size { get; }

        public int ScaledLineSpacing { get; }
        public int LineSpacing { get; }

        public int Ascender { get; }
        public int Descender { get; }
        public int MaxBearing { get; private set; }

        static TrueTypeFont()
        {
            Library = new FreeTypeLibrary();
        }

        public TrueTypeFont(string fileName, int size, string alphabet = null)
        {
            FileName = fileName;
            Size = size;

            FT.FT_New_Face(Library.Native, fileName, 0, out IntPtr facePtr);
            Face = facePtr;

            FT.FT_Set_Pixel_Sizes(Face, 0, (uint)Size);
            FaceRec = Marshal.PtrToStructure<FT_FaceRec>(Face);

            ScaledLineSpacing = FaceRec.size->metrics.height.ToInt32() >> 6;
            LineSpacing = FaceRec.height >> 6;

            Ascender = FaceRec.size->metrics.ascender.ToInt32() >> 6;

            Descender = (FaceRec.descender >> 6);

            RenderInfo = new Dictionary<char, TrueTypeGlyph>();

            if (string.IsNullOrEmpty(alphabet))
                Atlas = GenerateTextureAtlas(1..512);
            else
                Atlas = GenerateTextureAtlas(alphabet);
        }

        public bool CanRenderGlyph(char c)
            => RenderInfo.ContainsKey(c);

        public bool HasGlyph(char c)
            => FT.FT_Get_Char_Index(Face, c) != 0;

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

        private Texture GenerateTextureAtlas(Range glyphRange)
        {
            var glyphs = new List<char>();

            for (char c = (char)glyphRange.Start.Value; c < (char)glyphRange.End.Value; c++)
            {
                glyphs.Add(c);
            }

            return GenerateTextureAtlas(glyphs);
        }

        private Texture GenerateTextureAtlas(IEnumerable<char> glyphs)
        {
            var maxDim = (1 + FaceRec.size->metrics.height.ToInt32() >> 6) * MathF.Ceiling(MathF.Sqrt(glyphs.Count()));
            var texWidth = 1;

            while (texWidth < maxDim)
                texWidth <<= 1;

            var texHeight = texWidth;

            var texSize = texWidth * texHeight;
            IntPtr managedPixels = Marshal.AllocHGlobal(texWidth * texHeight);
            byte* pixels = (byte*)managedPixels.ToPointer();

            for (var i = 0; i < texSize; i++)
                pixels[i] = 0;

            int penX = 0;
            int penY = 0;

            foreach (var c in glyphs)
            {
                if (!HasGlyph(c))
                {
                    Log.Warning($"The font {FileName} doesn't support the requested glyph \\u{(int) c:X4}");
                    continue;
                }

                if (CanRenderGlyph(c))
                {
                    Log.Warning($"The font {FileName} has already generated the glyph for \\u{(int)c:X4}");
                    continue;
                }

                FT.FT_Load_Char(Face, c, FT.FT_LOAD_RENDER | FT.FT_LOAD_FORCE_AUTOHINT | FT.FT_LOAD_TARGET_LIGHT);
                var bmp = FaceRec.glyph->bitmap;
                var buffer = (byte*)FaceRec.glyph->bitmap.buffer.ToPointer();

                if (penX + bmp.width >= texWidth)
                {
                    penX = 0;
                    penY += ((FaceRec.size->metrics.height.ToInt32() >> 6) + 1);
                }

                for (var row = 0; row < bmp.rows; ++row)
                {
                    for (var col = 0; col < bmp.width; ++col)
                    {
                        var x = penX + col;
                        var y = penY + row;

                        pixels[y * texWidth + x] = buffer[row * bmp.pitch + col];
                    }
                }

                var glyph = new TrueTypeGlyph
                {
                    Position = new Vector2(penX, penY),
                    Size = new Vector2(
                        FaceRec.glyph->metrics.width.ToInt32() >> 6,
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
            FT.FT_Get_Kerning(Face, prev, current, 0, out FT_Vector kerning);
            return new Vector2(kerning.x.ToInt32() >> 6, kerning.y.ToInt32() >> 6);
        }

        private Texture CreateTextureFromFTBitmap(byte* pixels, int texWidth, int texHeight)
        {
            var surfaceSize = texWidth * texHeight * 4;

            IntPtr managedSurfaceData = Marshal.AllocHGlobal(surfaceSize);
            byte* surfaceData = (byte*)managedSurfaceData.ToPointer();

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

        protected override void FreeNativeResources()
        {
            FT.FT_Done_Face(Face);
        }
    }
}
