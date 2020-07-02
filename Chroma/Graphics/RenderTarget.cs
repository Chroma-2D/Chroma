using System;
using System.Drawing;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderTarget : Texture
    {
        internal IntPtr TargetHandle { get; }

        public Camera CurrentCamera { get; private set; }
        public Rectangle? CurrentViewport { get; private set; }

        public RenderTarget(int width, int height)
            : base(width, height)
        {
            TargetHandle = SDL_gpu.GPU_LoadTarget(ImageHandle);
        }

        public void SetViewport(Rectangle viewportRectangle)
        {
            EnsureNotDisposed();

            CurrentViewport = viewportRectangle;
            SDL_gpu.GPU_SetViewport(TargetHandle, new SDL_gpu.GPU_Rect
            {
                x = viewportRectangle.X,
                y = viewportRectangle.Y,
                w = viewportRectangle.Width,
                h = viewportRectangle.Height
            });
        }

        public void ResetViewport()
        {
            EnsureNotDisposed();

            SDL_gpu.GPU_UnsetViewport(TargetHandle);
            CurrentViewport = null;
        }

        public void SetCamera(Camera camera)
        {
            EnsureNotDisposed();

            SDL_gpu.GPU_SetCamera(TargetHandle, ref camera.GpuCamera);
            CurrentCamera = camera;
        }

        public void ResetCamera()
        {
            EnsureNotDisposed();

            SDL_gpu.GPU_SetCamera(TargetHandle, IntPtr.Zero);
            CurrentCamera = null;
        }

        protected override void FreeNativeResources()
        {
            SDL_gpu.GPU_FreeTarget(TargetHandle);
            base.FreeNativeResources();
        }
    }
}