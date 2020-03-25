using Chroma.SDL2;
using System;

namespace Chroma.Graphics
{
    public class Texture : IDisposable
    {
        internal SDL_gpu.GPU_Image_PTR ImageHandle { get; set; }

        public bool Disposed { get; private set; }

        public Size Size
        {
            get => new Size(
                ImageHandle.Value.w,
                ImageHandle.Value.h
            );
        }

        public Vector2 Anchor
        {
            get => new Vector2(
                ImageHandle.Value.anchor_x,
                ImageHandle.Value.anchor_y
            );
        }

        public int BytesPerPixel => ImageHandle.Value.bytes_per_pixel;

        public Texture(string fileName)
        {
            ImageHandle = SDL_gpu.GPU_LoadImage(fileName);
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    // No unmanaged resources to free.
                }

                SDL_gpu.GPU_FreeImage(ImageHandle);
                Disposed = true;
            }
        }

        ~Texture() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
