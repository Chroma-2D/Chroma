using System;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderTarget : Texture
    {
        internal IntPtr Handle { get; }

        public RenderTarget(ushort width, ushort height)
            : base(width, height)
        {
            Handle = SDL_gpu.GPU_LoadTarget(ImageHandle);
        }

        protected override void FreeNativeResources()
        {
            SDL_gpu.GPU_FreeTarget(Handle);
            base.FreeNativeResources();
        }
    }
}
