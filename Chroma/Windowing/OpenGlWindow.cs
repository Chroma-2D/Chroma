using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.SDL2;
using Chroma.Windowing.EventArgs;
using Chroma.Windowing.Events;
using Chroma.Windowing.Events.SpecializedHandlers;
using System;

namespace Chroma.Windowing
{
    public sealed class OpenGlWindow : IDisposable
    {
        private ulong _nowFrameTime = SDL.SDL_GetPerformanceCounter();
        private ulong _lastFrameTime = 0;

        private float Delta { get; set; }
        private FpsCounter FpsCounter { get; }
        private Game Game { get; }
        private RenderContext RenderContext { get; }

        internal delegate void StateUpdateDelegate(float delta);
        internal delegate void DrawDelegate(RenderContext context);

        internal StateUpdateDelegate Update;
        internal DrawDelegate Draw;

        internal EventDispatcher EventDispatcher { get; }
        internal SDL_gpu.GPU_Target_PTR RenderTargetPointer { get; }

        public event EventHandler Closed;
        public event EventHandler Exposed;
        public event EventHandler MouseEntered;
        public event EventHandler MouseLeft;
        public event EventHandler Focused;
        public event EventHandler Unfocused;
        public event EventHandler<CancelEventArgs> QuitRequested;
        public event EventHandler<WindowResizeEventArgs> SizeChanged;
        public event EventHandler<WindowResizeEventArgs> Resized;
        public event EventHandler<WindowMoveEventArgs> Moved;

        public bool Disposed { get; private set; }
        public IntPtr Handle { get; }
        public bool Running { get; set; }

        public WindowProperties Properties { get; }

        public float FPS => FpsCounter.FPS;

        internal OpenGlWindow(Game game)
        {
            Game = game;
            Properties = new WindowProperties(this);

            FpsCounter = new FpsCounter();

            Handle = SDL.SDL_CreateWindow(
                Properties.Title,
                (int)Properties.Position.X,
                (int)Properties.Position.Y,
                (int)Properties.Size.Width,
                (int)Properties.Size.Height,
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL
            );

            SDL_gpu.GPU_SetInitWindow(SDL.SDL_GetWindowID(Handle));

            var bestRenderer = GraphicsManager.Instance.GetBestRenderer();
            Console.WriteLine($"    Selecting best renderer: {bestRenderer.name}");
            RenderTargetPointer = SDL_gpu.GPU_InitRenderer(
                bestRenderer.renderer,
                (ushort)Properties.Size.Width,
                (ushort)Properties.Size.Height,
                0
            );

            GraphicsManager.Instance.VSyncEnabled = true;

            EventDispatcher = new EventDispatcher(this);
            new WindowEventHandlers(EventDispatcher);
            new FrameworkEventHandlers(EventDispatcher);
            new InputEventHandlers(EventDispatcher);

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
                    EventDispatcher.Dispatch(ev);

                Update?.Invoke(Delta);
                Draw?.Invoke(RenderContext);

                SDL_gpu.GPU_Flip(RenderTargetPointer);
                FpsCounter.Update();
            }
        }

        public void GoFullscreen(bool exclusive = false, bool autoRes = true)
        {
            var flag = (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;

            if (exclusive)
                flag = (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;

            SDL.SDL_SetWindowFullscreen(Handle, flag);

            if (autoRes)
                DetermineNativeResolution();
        }

        public void GoWindowed(ushort width, ushort height)
        {
            SDL.SDL_SetWindowFullscreen(Handle, 0);
            SDL_gpu.GPU_SetWindowResolution(width, height);

            Properties.Size = new Size(width, height);
            Properties.Position = new Vector2(SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED);
        }

        internal void OnQuitRequested(CancelEventArgs e)
        {
            QuitRequested?.Invoke(this, e);

            if (!e.Cancel)
                Running = false;
        }

        internal void OnExposed()
        {
            SDL_gpu.GPU_Flip(RenderTargetPointer);
            Exposed?.Invoke(this, System.EventArgs.Empty);
        }

        internal void OnMouseEntered()
            => MouseEntered?.Invoke(this, System.EventArgs.Empty);

        internal void OnMouseLeft()
            => MouseLeft?.Invoke(this, System.EventArgs.Empty);

        internal void OnFocused()
            => Focused?.Invoke(this, System.EventArgs.Empty);

        internal void OnUnfocused()
            => Unfocused?.Invoke(this, System.EventArgs.Empty);

        internal void OnClosed()
            => Closed?.Invoke(this, System.EventArgs.Empty);

        internal void OnSizeChanged(WindowResizeEventArgs e)
            => SizeChanged?.Invoke(this, e);

        internal void OnResized(WindowResizeEventArgs e)
            => Resized?.Invoke(this, e);

        internal void OnMoved(WindowMoveEventArgs e)
            => Moved?.Invoke(this, e);

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
            Properties.Size = new Size(mode.w, mode.h);
        }

        #region IDisposable
        private void Dispose(bool disposing)
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

        ~OpenGlWindow()
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
