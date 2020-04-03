using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Chroma.Natives.FreeType;
using Chroma.Natives.FreeType.Native;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.TextRendering
{
    public unsafe class TrueTypeFont : IDisposable
    {
        internal static FreeTypeLibrary Library { get; }
        internal IntPtr Face { get; }
        internal FT_FaceRec FaceRec { get; }

        public Dictionary<char, Glyph> RenderInfo { get; }
        public Texture Atlas { get; private set; }

        public bool Disposed { get; private set; }

        public string FileName { get; }
        public int Size { get; }

        static TrueTypeFont()
        {
            Library = new FreeTypeLibrary();
        }

        public TrueTypeFont(string fileName, int size)
        {
            FileName = fileName;
            Size = size;

            FT.FT_New_Face(Library.Native, fileName, 0, out IntPtr facePtr);
            Face = facePtr;

            FT.FT_Set_Pixel_Sizes(Face, 0, (uint)Size);
            FaceRec = Marshal.PtrToStructure<FT_FaceRec>(Face);

            RenderInfo = new Dictionary<char, Glyph>();
            Atlas = GenerateTextureAtlas();
        }

        public bool HasGlyph(char c)
            => FT.FT_Get_Char_Index(Face, c) != 0;

        private Texture GenerateTextureAtlas(int maxGlyphs = 512)
        {
            var maxDim = (1 + FaceRec.size->metrics.height.ToInt32() >> 6) * MathF.Ceiling(MathF.Sqrt(maxGlyphs));
            var texWidth = 1;

            while (texWidth < maxDim) texWidth <<= 1;
            var texHeight = texWidth;

            byte* pixels = stackalloc byte[texWidth * texHeight];
            int penX = 0;
            int penY = 0;

            var glyphsGenerated = 0;
            for (char c = (char)0; c < char.MaxValue; ++c)
            {
                if (!HasGlyph(c))
                    continue;

                if (glyphsGenerated >= maxGlyphs)
                    break;

                FT.FT_Load_Char(Face, c, FT.FT_LOAD_RENDER);
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

                var glyph = new Glyph
                {
                    Position = new Vector2(penX, penY),
                    Dimensions = new Vector2(bmp.width, bmp.rows),
                    Offset = new Vector2(FaceRec.glyph->bitmap_left, FaceRec.glyph->bitmap_top),
                    Advance = FaceRec.glyph->advance.x.ToInt32() >> 6
                };

                penX += (int)bmp.width + 1;
                glyphsGenerated++;
            }

            return CreateTextureFromFTBitmap(pixels, texWidth, texHeight);
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

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    // No managed resources to free.
                }

                FT.FT_Done_Face(Face);
                Disposed = true;
            }
        }

        ~TrueTypeFont()
            => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
