using Chroma.Graphics;
using Chroma.SDL2;
using System;

namespace Chroma.Windowing
{
    public abstract class WindowBase : IDisposable
    {
        private Size _size;
        private string _title;

        private ulong _nowFrameTime = SDL.SDL_GetPerformanceCounter();
        private ulong _lastFrameTime = 0;

        internal SDL_gpu.GPU_Target_PTR RenderTargetPointer { get; }

        protected float Delta { get; private set; }
        protected Game Game { get; }
        protected RenderContext RenderContext { get; }

        public IntPtr Handle { get; }
        public bool Disposed { get; private set; }
        public bool Running { get; protected set; }

        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                SDL.SDL_SetWindowSize(Handle, (ushort)_size.Width, (ushort)_size.Height);
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                SDL.SDL_SetWindowTitle(Handle, _title);
            }
        }

        internal WindowBase(Game game, ushort width, ushort height, SDL.SDL_WindowFlags windowFlags)
        {
            Game = game;

            _size = new Size(width, height);
            _title = "Chroma Engine";

            Handle = SDL.SDL_CreateWindow(_title, 100, 100, width, height, windowFlags);
            SDL_gpu.GPU_SetInitWindow(SDL.SDL_GetWindowID(Handle));

            // TODO: decouple from ogl
            RenderTargetPointer = SDL_gpu.GPU_InitRenderer(SDL_gpu.GPU_RendererEnum.GPU_RENDERER_OPENGL_3, width, height, 0);
            RenderContext = new RenderContext(this);
        }

        public void Run()
        {
            Running = true;

            while (Running)
            {
                _lastFrameTime = _nowFrameTime;
                _nowFrameTime = SDL.SDL_GetPerformanceCounter();
                Delta = (_nowFrameTime - _lastFrameTime) / (float)SDL.SDL_GetPerformanceFrequency();

                while (SDL.SDL_PollEvent(out SDL.SDL_Event ev) != 0)
                    OnSdlEvent(ev);

                OnUpdate(Delta);
                OnDraw();

                SDL_gpu.GPU_Flip(RenderTargetPointer);
            }
        }

        internal virtual void OnSdlEvent(SDL.SDL_Event ev)
        {
            if (ev.type == SDL.SDL_EventType.SDL_QUIT)
                Running = false;
        }

        protected virtual void OnUpdate(float delta)
        {
        }

        protected virtual void OnDraw()
        {
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Running = false;

                if (disposing)
                {
                    // No managed resources to free.
                }

                SDL_gpu.GPU_FreeTarget(RenderTargetPointer);
                SDL_gpu.GPU_Quit();
                SDL.SDL_Quit();

                Disposed = true;
            }
        }

        ~WindowBase()
        {
           Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
