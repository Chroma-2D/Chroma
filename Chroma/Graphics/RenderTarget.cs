using System;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderTarget : Texture
    {
        internal IntPtr TargetHandle { get; }

        public RenderTarget(ushort width, ushort height)
            : base(width, height)
        {
            TargetHandle = SDL_gpu.GPU_LoadTarget(ImageHandle);
        }

        public void SetCamera(Camera camera)
        {
            EnsureNotDisposed();
            SDL_gpu.GPU_SetCamera(TargetHandle, ref camera.GpuCamera);
        }

        public void ResetCamera()
            => SDL_gpu.GPU_SetCamera(TargetHandle, IntPtr.Zero);

        protected override void FreeNativeResources()
        {
            SDL_gpu.GPU_FreeTarget(TargetHandle);
            base.FreeNativeResources();
        }
    }
}
