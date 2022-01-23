using System;
using System.Drawing;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderTarget : Texture
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        
        internal IntPtr TargetHandle { get; }

        public Camera CurrentCamera { get; private set; }
        public Rectangle? CurrentViewport { get; private set; }

        public RenderTarget(int width, int height)
            : base(width, height)
        {
            if (ImageHandle == IntPtr.Zero)
            {
                var msg = $"Failed to create texture handle: {SDL2.SDL_GetError()}";
                _log.Error(msg);
                
                throw new GraphicsException(msg);
            }
            
            TargetHandle = SDL_gpu.GPU_LoadTarget(ImageHandle);

            if (TargetHandle == IntPtr.Zero)
            {
                var msg = $"Failed to create render target handle: {SDL2.SDL_GetError()}";
                
                _log.Error(msg);
                throw new GraphicsException(msg);
            }
        }

        public RenderTarget(Size size)
            : this(size.Width, size.Height) { }

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
            EnsureOnMainThread();
            
            SDL_gpu.GPU_FreeTarget(TargetHandle);
            base.FreeNativeResources();
        }
    }
}