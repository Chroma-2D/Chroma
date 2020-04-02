using System;
using System.Runtime.InteropServices;
using Chroma.Natives.SDL;
using FreeTypeSharp;
using FreeTypeSharp.Native;

namespace Chroma.Graphics.TextRendering
{
    public unsafe class TrueTypeFont : IDisposable
    {
        internal static FreeTypeLibrary Library { get; }
        internal IntPtr Face { get; }
        internal FT_FaceRec FaceRec { get; }

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
        }

        public Texture RenderGlyph(char c)
        {
            var index = FT.FT_Get_Char_Index(Face, c);
            FT.FT_Load_Glyph(Face, index, FT.FT_LOAD_RENDER);

            var bitmap = FaceRec.glyph->bitmap;
            byte* bitmapBuffer = (byte*)bitmap.buffer.ToPointer();

            var pixPtr = Marshal.AllocHGlobal((int)bitmap.width * (int)bitmap.rows * 4);
            byte* pixPtrPtr = (byte*)pixPtr.ToPointer();

            for (var y = 0; y < bitmap.rows; y++)
            {
                for (var x = 0; x < bitmap.width; x++)
                {
                    byte value = bitmapBuffer[(bitmap.pitch * y) + x];
                    var origin = ((bitmap.width * y) + x) * 4;
                    pixPtrPtr[origin] = 0xFF;
                    pixPtrPtr[origin + 1] = 0xFF;
                    pixPtrPtr[origin + 2] = 0xFF;
                    pixPtrPtr[origin + 3] = value;
                }
            }

            var surface = SDL2.SDL_CreateRGBSurfaceFrom(
                pixPtr,
                (int)bitmap.width,
                (int)bitmap.rows,
                32,
                (int)bitmap.width * 4,
                0x000000FF,
                0x0000FF00,
                0x00FF0000,
                0xFF000000
            );

            SDL2.SDL_SetSurfaceBlendMode(surface, SDL2.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            var imageptr = SDL_gpu.GPU_CopyImageFromSurface(surface);
            SDL2.SDL_FreeSurface(surface);
            Marshal.FreeHGlobal(pixPtr);
            return new Texture(imageptr);
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
