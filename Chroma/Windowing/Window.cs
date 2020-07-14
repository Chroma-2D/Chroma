using System;
using System.Drawing;
using System.Numerics;
using System.Threading;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using Chroma.Windowing.EventArgs;
using Chroma.Windowing.EventHandling;
using Chroma.Windowing.EventHandling.Specialized;

namespace Chroma.Windowing
{
    public sealed class Window : DisposableResource
    {
        private ulong _nowFrameTime = SDL2.SDL_GetPerformanceCounter();
        private ulong _lastFrameTime;

        private string _title = "Chroma Framework";
        private Size _size = new Size(800, 600);
        private Vector2 _position = new Vector2(SDL2.SDL_WINDOWPOS_CENTERED, SDL2.SDL_WINDOWPOS_CENTERED);
        private WindowState _state = WindowState.Normal;

        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        private float Delta { get; set; }
        private FpsCounter FpsCounter { get; }
        private RenderContext RenderContext { get; }

        internal delegate void StateUpdateDelegate(float delta);

        internal delegate void DrawDelegate(RenderContext context);

        internal StateUpdateDelegate Update;
        internal DrawDelegate Draw;

        internal Game Game { get; }
        internal EventDispatcher EventDispatcher { get; }

        internal IntPtr RenderTargetHandle { get; }

        public float FPS => FpsCounter.FPS;
        public bool Exists { get; private set; }

        public IntPtr Handle { get; }

        public Size Size
        {
            get
            {
                if (Handle != IntPtr.Zero)
                {
                    SDL2.SDL_GetWindowSize(Handle, out var w, out var h);
                    return new Size(w, h);
                }
                else
                {
                    return _size;
                }
            }

            set
            {
                _size = value;

                if (Handle != IntPtr.Zero)
                {
                    if (GraphicsManager.ViewportAutoResize)
                    {
                        SDL_gpu.GPU_SetWindowResolution((ushort)_size.Width, (ushort)_size.Height);
                    }
                    else
                    {
                        SDL2.SDL_SetWindowSize(Handle, (ushort)_size.Width, (ushort)_size.Height);
                    }
                }
            }
        }

        public Vector2 Position
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return _position;

                SDL2.SDL_GetWindowPosition(Handle, out var x, out var y);
                return new Vector2(x, y);
            }

            set
            {
                _position = value;

                if (Handle != IntPtr.Zero)
                    SDL2.SDL_SetWindowPosition(Handle, (int)_position.X, (int)_position.Y);
            }
        }

        public Vector2 Center => new Vector2(Size.Width, Size.Height) / 2;

        public string Title
        {
            get
            {
                if (Handle == IntPtr.Zero)
                    return _title;

                return SDL2.SDL_GetWindowTitle(Handle);
            }
            set
            {
                _title = value;

                if (Handle != IntPtr.Zero)
                    SDL2.SDL_SetWindowTitle(Handle, _title);
            }
        }

        public WindowState State
        {
            get => _state;

            set
            {
                _state = value;

                if (Handle != IntPtr.Zero)
                {
                    switch (value)
                    {
                        case WindowState.Maximized:
                            var flags = (SDL2.SDL_WindowFlags)SDL2.SDL_GetWindowFlags(Handle);

                            if (!flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_RESIZABLE))
                            {
                                Log.Warning("Refusing to maximize a non-resizable window.");
                                return;
                            }

                            SDL2.SDL_MaximizeWindow(Handle);
                            break;

                        case WindowState.Minimized:
                            SDL2.SDL_MinimizeWindow(Handle);
                            break;

                        case WindowState.Normal:
                            SDL2.SDL_RestoreWindow(Handle);
                            break;
                    }
                }
            }
        }

        public bool CanResize
        {
            get
            {
                var flags = (SDL2.SDL_WindowFlags)SDL2.SDL_GetWindowFlags(Handle);
                return flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
            }

            set => SDL2.SDL_SetWindowResizable(
                Handle,
                value
                    ? SDL2.SDL_bool.SDL_TRUE
                    : SDL2.SDL_bool.SDL_FALSE
            );
        }

        public Size MaximumSize
        {
            get
            {
                SDL2.SDL_GetWindowMaximumSize(Handle, out var w, out var h);
                return new Size(w, h);
            }

            set
            {
                if (value == Size.Empty)
                {
                    SDL2.SDL_SetWindowMaximumSize(Handle, 16384, 16384);
                }
                else
                {
                    SDL2.SDL_SetWindowMaximumSize(Handle, value.Width, value.Height);
                }
            }
        }

        public Size MinimumSize
        {
            get
            {
                SDL2.SDL_GetWindowMinimumSize(Handle, out var w, out var h);
                return new Size(w, h);
            }

            set
            {
                if (value == Size.Empty)
                {
                    SDL2.SDL_SetWindowMinimumSize(Handle, 1, 1);
                }
                else
                {
                    SDL2.SDL_SetWindowMinimumSize(Handle, value.Width, value.Height);
                }
            }
        }

        public bool EnableBorder
        {
            get
            {
                var flags = (SDL2.SDL_WindowFlags)SDL2.SDL_GetWindowFlags(Handle);
                return !flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
            }
            set => SDL2.SDL_SetWindowBordered(Handle, value ? SDL2.SDL_bool.SDL_TRUE : SDL2.SDL_bool.SDL_FALSE);
        }

        public bool IsFullScreen
        {
            get
            {
                var flags = (SDL2.SDL_WindowFlags)SDL2.SDL_GetWindowFlags(Handle);

                return flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN)
                       || flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
            }
        }

        public bool IsCursorGrabbed
        {
            get => SDL2.SDL_GetWindowGrab(Handle) == SDL2.SDL_bool.SDL_TRUE;
            set => SDL2.SDL_SetWindowGrab(Handle, value ? SDL2.SDL_bool.SDL_TRUE : SDL2.SDL_bool.SDL_FALSE);
        }

        public event EventHandler Closed;
        public event EventHandler Hidden;
        public event EventHandler Shown;
        public event EventHandler Invalidated;
        public event EventHandler<WindowStateEventArgs> StateChanged;
        public event EventHandler MouseEntered;
        public event EventHandler MouseLeft;
        public event EventHandler Focused;
        public event EventHandler Unfocused;
        public event EventHandler<WindowMoveEventArgs> Moved;
        public event EventHandler<WindowSizeEventArgs> SizeChanged;
        public event EventHandler<WindowSizeEventArgs> Resized;
        public event EventHandler<CancelEventArgs> QuitRequested;

        internal Window(Game game)
        {
            Game = game;

            Handle = SDL2.SDL_CreateWindow(
                Title,
                (int)Position.X,
                (int)Position.Y,
                Size.Width,
                Size.Height,
                SDL2.SDL_WindowFlags.SDL_WINDOW_OPENGL | 
                SDL2.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI
            );

            if (Handle == IntPtr.Zero)
                throw new FrameworkException("Failed to initialize the window.", true);

            SDL_gpu.GPU_SetInitWindow(SDL2.SDL_GetWindowID(Handle));

            var bestRenderer = GraphicsManager.GetBestRenderer();

            RenderTargetHandle = SDL_gpu.GPU_InitRenderer(
                bestRenderer.renderer,
                (ushort)Size.Width,
                (ushort)Size.Height,
                0
            );

            if (RenderTargetHandle == IntPtr.Zero)
                throw new FrameworkException("Failed to initialize the renderer.", true);

            MaximumSize = Size.Empty;
            MinimumSize = Size.Empty;

            FpsCounter = new FpsCounter();
            RenderContext = new RenderContext(this);

            EventDispatcher = new EventDispatcher(this);
            _ = new WindowEventHandlers(EventDispatcher);
            _ = new FrameworkEventHandlers(EventDispatcher);
            _ = new InputEventHandlers(EventDispatcher);
        }

        public void Show()
            => SDL2.SDL_ShowWindow(Handle);

        public void Hide()
            => SDL2.SDL_HideWindow(Handle);

        public void CenterScreen()
            => Position = new Vector2(SDL2.SDL_WINDOWPOS_CENTERED, SDL2.SDL_WINDOWPOS_CENTERED);

        public void GoFullscreen(bool exclusive = false)
        {
            var flag = (uint)SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;

            if (exclusive)
                flag = (uint)SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN;

            SDL2.SDL_SetWindowFullscreen(Handle, flag);

            var thisDisplayIndex = SDL2.SDL_GetWindowDisplayIndex(Handle);
            Size = Game.Graphics.GetNativeResolution(thisDisplayIndex);
        }

        public void GoWindowed(Size size, bool centerOnScreen = false)
        {
            SDL2.SDL_SetWindowFullscreen(Handle, 0);
            SDL_gpu.GPU_SetWindowResolution((ushort)size.Width, (ushort)size.Height);

            Size = size;

            if (centerOnScreen)
                CenterScreen();
        }

        public void SetIcon(Texture texture)
        {
            if (texture.Disposed)
                throw new InvalidOperationException("The texture provided was already disposed.");

            unsafe
            {
                SDL2.SDL_SetWindowIcon(Handle, new IntPtr(texture.Surface));
            }
        }

        internal void Run(Action postStatusSetAction = null)
        {
            Exists = true;
            postStatusSetAction?.Invoke();

            while (Exists)
            {
                _lastFrameTime = _nowFrameTime;
                _nowFrameTime = SDL2.SDL_GetPerformanceCounter();
                Delta = (_nowFrameTime - _lastFrameTime) / (float)SDL2.SDL_GetPerformanceFrequency();

                while (SDL2.SDL_PollEvent(out var ev) != 0)
                    EventDispatcher.Dispatch(ev);

                Update?.Invoke(Delta);

                if (GraphicsManager.AutoClear)
                    RenderContext.Clear(GraphicsManager.AutoClearColor);

                Draw?.Invoke(RenderContext);

                SDL_gpu.GPU_Flip(RenderTargetHandle);
                FpsCounter.Update();

                if (GraphicsManager.LimitFramerate)
                    Thread.Sleep(1);
            }
        }

        internal void OnClosed()
            => Closed?.Invoke(this, System.EventArgs.Empty);

        internal void OnHidden()
            => Hidden?.Invoke(this, System.EventArgs.Empty);

        internal void OnShown()
            => Shown?.Invoke(this, System.EventArgs.Empty);

        internal void OnInvalidated()
        {
            SDL_gpu.GPU_Flip(RenderTargetHandle);
            Invalidated?.Invoke(this, System.EventArgs.Empty);
        }

        internal void OnStateChanged(WindowStateEventArgs e)
        {
            _state = e.State;
            StateChanged?.Invoke(this, e);
        }

        internal void OnMouseEntered()
            => MouseEntered?.Invoke(this, System.EventArgs.Empty);

        internal void OnMouseLeft()
            => MouseLeft?.Invoke(this, System.EventArgs.Empty);

        internal void OnFocusOffered()
        {
            SDL2.SDL_SetWindowInputFocus(Handle);
        }

        internal void OnFocused()
            => Focused?.Invoke(this, System.EventArgs.Empty);

        internal void OnUnfocused()
            => Unfocused?.Invoke(this, System.EventArgs.Empty);

        internal void OnMoved(WindowMoveEventArgs e)
        {
            _position = e.Position;

            Moved?.Invoke(this, e);
        }

        internal void OnSizeChanged(WindowSizeEventArgs e)
        {
            _size = e.Size;

            SizeChanged?.Invoke(this, e);
        }

        internal void OnResized(WindowSizeEventArgs e)
        {
            if (GraphicsManager.ViewportAutoResize)
            {
                SDL_gpu.GPU_SetWindowResolution(
                    (ushort)e.Size.Width,
                    (ushort)e.Size.Height
                );
            }

            Resized?.Invoke(this, e);
        }

        internal void OnQuitRequested(CancelEventArgs e)
        {
            QuitRequested?.Invoke(this, e);

            if (!e.Cancel)
                Exists = false;
        }

        protected override void FreeNativeResources()
        {
            SDL_gpu.GPU_FreeTarget(RenderTargetHandle);
            SDL_gpu.GPU_Quit();
            SDL2.SDL_Quit();
        }
    }
}