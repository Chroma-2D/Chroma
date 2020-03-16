using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.SDL2;
using System;

namespace Chroma.Windowing
{
    public abstract class WindowBase : IDisposable
    {
        private Size _size;
        private Vector2 _position;
        private string _title;
        private bool _vSyncEnabled;

        private ulong _nowFrameTime = SDL.SDL_GetPerformanceCounter();
        private ulong _lastFrameTime = 0;

        private FpsCounter FpsCounter { get; }

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

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                SDL.SDL_SetWindowPosition(Handle, (int)_position.X, (int)_position.Y);
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

        public float FPS => FpsCounter.FPS;

        public bool VSyncEnabled
        {
            get => _vSyncEnabled;
            set
            {
                _vSyncEnabled = value;

                if (!value)
                {
                    SDL.SDL_GL_SetSwapInterval(0);
                }
                else
                {
                    SDL.SDL_GL_SetSwapInterval(1);
                }
            }
        }

        public bool IsFullScreen { get; private set; }
        public bool IsBorderless { get; private set; }

        internal WindowBase(Game game, Vector2 position, ushort width, ushort height, bool vSyncEnabled, SDL.SDL_WindowFlags windowFlags)
        {
            Game = game;

            _size = new Size(width, height);
            _position = position;
            _title = "Chroma Engine";

            FpsCounter = new FpsCounter();

            Handle = SDL.SDL_CreateWindow(_title, (int)_position.X, (int)_position.Y, width, height, windowFlags);
            SDL_gpu.GPU_SetInitWindow(SDL.SDL_GetWindowID(Handle));

            var bestRenderer = GraphicsManager.Instance.GetBestRenderer();
            Console.WriteLine($"    Selecting best renderer: {bestRenderer.name}");
            RenderTargetPointer = SDL_gpu.GPU_InitRenderer(bestRenderer.renderer, width, height, 0);

            // must be set AFTER creating context ^
            VSyncEnabled = vSyncEnabled;

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
                FpsCounter.Update();
            }
        }

        public void SwitchToExclusiveFullscreen(bool autoRes = true)
        {
            SDL.SDL_SetWindowFullscreen(Handle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);

            if (autoRes)
                DetermineNativeResolution();

            IsFullScreen = true;
            IsBorderless = false;
        }

        public void SwitchToBorderlessFullscreen(bool autoRes = true)
        {
            SDL.SDL_SetWindowFullscreen(Handle, (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);

            if (autoRes)
                DetermineNativeResolution();

            IsFullScreen = true;
            IsBorderless = true;
        }

        public void SwitchToWindowed(ushort width, ushort height)
        {
            SDL.SDL_SetWindowFullscreen(Handle, 0);
            SDL_gpu.GPU_SetWindowResolution(width, height);

            Size = new Size(width, height);
            Position = new Vector2(SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED);

            IsFullScreen = false;
            IsBorderless = false;
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

        private void DetermineNativeResolution()
        {
            var thisDisplayIndex = SDL.SDL_GetWindowDisplayIndex(Handle);
            var display = GraphicsManager.Instance.FetchDesktopDisplayInfo(thisDisplayIndex);

            var mode = new SDL.SDL_DisplayMode
            {
                driverdata = display.UnderlyingDisplayMode.driverdata,
                format = display.UnderlyingDisplayMode.format,
                w = (int)display.Dimensions.Width,
                h = (int)display.Dimensions.Height,
                refresh_rate = display.RefreshRate
            };

            SDL_gpu.GPU_SetWindowResolution((ushort)mode.w, (ushort)mode.h);
            Size = new Size(mode.w, mode.h);
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
