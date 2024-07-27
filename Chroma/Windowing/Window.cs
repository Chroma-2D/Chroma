namespace Chroma.Windowing;

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.MemoryManagement;
using Chroma.Natives.Bindings.SDL;
using Chroma.Threading;
using Chroma.Windowing.DragDrop;
using Chroma.Windowing.EventHandling;
using Chroma.Windowing.EventHandling.Specialized;

public sealed class Window : DisposableResource
{
    private static readonly Log _log = LogManager.GetForCurrentAssembly();

    // Prevents delegate garbage collection.
    private readonly SDL2.SDL_HitTest _internalHitTestCallback;

    private readonly PerformanceCounter _performanceCounter;
    private readonly RenderContext _renderContext;

    private string _title = "Chroma Framework";
    private Size _size = new(800, 600);
    private Vector2 _position = new(SDL2.SDL_WINDOWPOS_CENTERED);
    private WindowState _state = WindowState.Normal;
    private bool _enableDragDrop;
    private WindowHitTestDelegate? _hitTestDelegate;

    private IntPtr _windowHandle;
    private IntPtr _currentIconPtr;
    private PlatformWindowInformation _windowInfo;
    private bool _exists;

    internal delegate void StateUpdateDelegate(float delta);

    internal delegate void DrawDelegate(RenderContext context);

    internal StateUpdateDelegate? FixedUpdate;
    internal StateUpdateDelegate? Update;
    internal DrawDelegate? Draw;

    internal Game Game { get; }
    internal EventDispatcher? EventDispatcher { get; private set; }
    internal DragDropManager DragDropManager { get; }

    internal IntPtr Handle => _windowHandle;
    internal IntPtr RenderTargetHandle { get; private set; }

    internal static Window Instance { get; private set; } = null!;

    public delegate WindowHitTestResult WindowHitTestDelegate(Window window, Vector2 position);

    public IntPtr SystemWindowHandle => _windowInfo.SystemWindowHandle;

    public bool Exists => _exists;

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
                if (Game.Graphics.ViewportAutoResize)
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

    public int Width => Size.Width;
    public int Height => Size.Height;

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

    public bool TopMost
    {
        get
        {
            if (Handle == IntPtr.Zero)
                return false;

            return SDL2.SDL_GetWindowFlags(Handle)
                .HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_ALWAYS_ON_TOP);
        }

        set
        {
            if (Handle == IntPtr.Zero)
                return;

            SDL2.SDL_SetWindowAlwaysOnTop(Handle, value);
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
                        var flags = SDL2.SDL_GetWindowFlags(Handle);

                        if (!flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_RESIZABLE))
                        {
                            _log.Warning("Refusing to maximize a non-resizable window.");
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
        get => SDL2.SDL_GetWindowFlags(Handle)
            .HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

        set => SDL2.SDL_SetWindowResizable(Handle, value);
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

    public int MaximumWidth => MaximumSize.Width;
    public int MaximumHeight => MaximumSize.Height;

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

    public int MinimumWidth => MinimumSize.Width;
    public int MinimumHeight => MinimumSize.Height;

    public bool IsFullScreen
        => SDL2.SDL_GetWindowFlags(Handle)
            .HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN);

    public bool IsBorderlessFullScreen
        => SDL2.SDL_GetWindowFlags(Handle)
            .HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);

    public Display CurrentDisplay
    {
        get
        {
            var index = SDL2.SDL_GetWindowDisplayIndex(Handle);

            if (index < 0)
            {
                _log.Error($"Failed to retrieve window index: {SDL2.SDL_GetError()}");
                return Display.Invalid;
            }

            var display = Game.Graphics.GetDisplayList().ElementAtOrDefault(index);

            if (display == null)
            {
                _log.Error($"Failed to retrieve the display at index {index}. This should never happen.");
                return Display.Invalid;
            }

            return display;
        }
    }

    public bool HasKeyboardFocus
    {
        get
        {
            if (Handle == IntPtr.Zero)
                return false;

            return SDL2.SDL_GetWindowFlags(Handle).HasFlag(
                SDL2.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS
            );
        }
    }

    public bool IsMouseOver
    {
        get
        {
            if (Handle == IntPtr.Zero)
                return false;

            return SDL2.SDL_GetWindowFlags(Handle).HasFlag(
                SDL2.SDL_WindowFlags.SDL_WINDOW_MOUSE_FOCUS
            );
        }
    }

    public bool EnableBorder
    {
        get
        {
            var flags = SDL2.SDL_GetWindowFlags(Handle);
            return !flags.HasFlag(SDL2.SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
        }
        set => SDL2.SDL_SetWindowBordered(Handle, value);
    }

    public bool EnableDragDrop
    {
        get => _enableDragDrop;
        set
        {
            _enableDragDrop = value;
            var state = _enableDragDrop ? SDL2.SDL_ENABLE : SDL2.SDL_DISABLE;

            SDL2.SDL_EventState(SDL2.SDL_EventType.SDL_DROPFILE, state);
            SDL2.SDL_EventState(SDL2.SDL_EventType.SDL_DROPTEXT, state);
            SDL2.SDL_EventState(SDL2.SDL_EventType.SDL_DROPBEGIN, state);
            SDL2.SDL_EventState(SDL2.SDL_EventType.SDL_DROPCOMPLETE, state);
        }
    }

    public WindowHitTestDelegate? HitTest
    {
        get => _hitTestDelegate;
        set
        {
            if (Handle == IntPtr.Zero)
                return;

            _hitTestDelegate = value;

            if (SDL2.SDL_SetWindowHitTest(
                    Handle,
                    IsHitTestEnabled
                        ? _internalHitTestCallback
                        : null,
                    IntPtr.Zero
                ) < 0)
            {
                _log.Error($"Failed to set the window hit testing delegate: {SDL2.SDL_GetError()}");
            }
        }
    }

    public bool IsHitTestEnabled => _hitTestDelegate != null;

    public WindowMode Mode { get; }

    public event EventHandler? Shown;
    public event EventHandler? Hidden;
    public event EventHandler? Invalidated;
    public event EventHandler<WindowMoveEventArgs>? Moved;
    public event EventHandler<WindowSizeEventArgs>? Resized;
    public event EventHandler<WindowSizeEventArgs>? SizeChanged;
    public event EventHandler<WindowStateEventArgs>? StateChanged;
    public event EventHandler? MouseEntered;
    public event EventHandler? MouseLeft;
    public event EventHandler? Focused;
    public event EventHandler? Unfocused;
    public event EventHandler? Closed;
    public event EventHandler<DisplayChangedEventArgs>? DisplayChanged;
    public event EventHandler<CancelEventArgs>? QuitRequested;

    public event EventHandler<FileDragDropEventArgs>? FilesDropped;
    public event EventHandler<TextDragDropEventArgs>? TextDropped;

    internal Window(Game game)
    {
        Game = game;

        do
        {
            if (!Game.Graphics.QueryOpenGl())
                continue;

            RenderTargetHandle = Game.Graphics.InitializeRenderer(this, out _windowHandle);
        } while (RenderTargetHandle == IntPtr.Zero && Game.Graphics.AnyRenderersAvailable);

        if (RenderTargetHandle == IntPtr.Zero)
        {
            throw new FrameworkException(
                "Every available OpenGL renderer has failed to initialize." +
                "Enabling SDL_gpu debugging in boot.json might shed some more light on the problem."
            );
        }

        if (Handle == IntPtr.Zero)
            throw new FrameworkException($"Failed to initialize SDL window: {SDL2.SDL_GetError()}.");

        _windowInfo = new PlatformWindowInformation(Handle);
            
        Title = _title;
        Position = _position;

        MaximumSize = Size.Empty;
        MinimumSize = Size.Empty;

        DragDropManager = new DragDropManager(this);
        EnableDragDrop = true;

        Mode = new WindowMode(this);
        _internalHitTestCallback = HitTestCallback;
        _performanceCounter = new PerformanceCounter();
        _renderContext = new RenderContext(this);

        Instance = this;
    }

    public void Show()
        => SDL2.SDL_ShowWindow(Handle);

    public void Hide()
        => SDL2.SDL_HideWindow(Handle);

    public void Flash(WindowFlash flash)
    {
        if (SDL2.SDL_FlashWindow(Handle, (SDL2.SDL_FlashOperation)flash) < 0)
        {
            _log.Error($"Failed to flash the window using {flash}: {SDL2.SDL_GetError()}");
        }
    }

    public void StopFlashing()
    {
        if (SDL2.SDL_FlashWindow(Handle, SDL2.SDL_FlashOperation.SDL_FLASH_CANCEL) < 0)
        {
            _log.Error($"Failed to stop flashing the window: {SDL2.SDL_GetError()}");
        }
    }

    public void CenterOnScreen()
    {
        var bounds = CurrentDisplay.Bounds;

        var targetX = bounds.Width / 2 - _size.Width / 2;
        var targetY = bounds.Height / 2 - _size.Height / 2;

        Position = new Vector2(bounds.X + targetX, bounds.Y + targetY);
    }

    public void SetIcon(Texture texture)
    {
        if (texture.Disposed)
            throw new ArgumentException("The texture provided was already disposed.");

        if (_currentIconPtr != IntPtr.Zero)
            SDL2.SDL_FreeSurface(_currentIconPtr);

        _currentIconPtr = texture.AsSdlSurface();

        SDL2.SDL_SetWindowIcon(
            Handle,
            _currentIconPtr
        );
    }

    public void SaveScreenshot(Stream outputStream, PixelFormat format = PixelFormat.RGB)
    {
        EnsureNotDisposed();

        var rwOpsIo = new SdlRwOps(outputStream, true);

        var created = false;
        var locked = false;

        var surface = SDL_gpu.GPU_CopySurfaceFromTarget(RenderTargetHandle);

        if (surface == IntPtr.Zero)
        {
            _log.Error($"Failed to copy window render target to SDL surface: {SDL2.SDL_GetError()}");
            goto __exit;
        }
        created = true;

        // original is freed by ConvertSurfaceToFormat
        surface = PixelFormatConverter.ConvertSurfaceToFormat(surface, PixelFormat.RGB);

        if (surface == IntPtr.Zero)
            goto __exit;

        if (SDL2.SDL_LockSurface(surface) < 0)
        {
            _log.Error($"Failed to lock the created SDL surface: {SDL2.SDL_GetError()}");
            goto __exit;
        }
        locked = true;

        if (SDL2.SDL_SaveBMP_RW(surface, rwOpsIo.RwOpsHandle, true) < 0)
        {
            _log.Error($"Failed to save the BMP screenshot to stream: {SDL2.SDL_GetError()}");
        }

        __exit:
        if (locked)
            SDL2.SDL_UnlockSurface(surface);

        if (created)
            SDL2.SDL_FreeSurface(surface);

        rwOpsIo.Dispose();
    }

    public void SaveScreenshot(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            SaveScreenshot(stream);
        }
    }

    internal void InitializeEventDispatcher()
    {
        if (EventDispatcher != null)
        {
            _log.Warning("Tried to initialize event dispatching twice.");
            return;
        }

        EventDispatcher = new EventDispatcher(this);
        _ = new WindowEventHandlers(EventDispatcher);
        _ = new FrameworkEventHandlers(EventDispatcher);
        _ = new InputEventHandlers(EventDispatcher);
        _ = new AudioEventHandlers(EventDispatcher);
    }

    internal void Run()
    {
        _exists = true;

        while (_exists)
        {
            _performanceCounter.Update();
            _performanceCounter.FixedUpdate();

            while (SDL2.SDL_PollEvent(out var ev) != 0)
                EventDispatcher?.Dispatch(ev);

            DoTick(PerformanceCounter.Delta);
            DoFixedTicks(PerformanceCounter.FixedDelta);

            if (RenderSettings.AutoClearEnabled)
                _renderContext.Clear(RenderSettings.AutoClearColor);

            // This is a fix for Discord's screen sharing hooks fucking something up inside SDL_gpu.
            //
            // Apparently the first texture ever used in the application's
            // lifetime becomes a render target that's scaled down and flipped upside-down.
            //
            // This is a workaround for it until there's a better understanding of this bug.
            //
            // 22.01.2022: Guess it's going to stay like this for longer.
            //
            // 23.03.2022: Freetype. It's fucked. Badly.
            //
            // 23.03.2022 (later that day): It's not Freetype? See Game.cs for details.
            //
            SDL_gpu.GPU_BlitTransformX(
                EmbeddedAssets.DummyFixTexture.ImageHandle,
                IntPtr.Zero,
                RenderTargetHandle,
                -1, -1, 0, 0, 0, 1, 1
            );

            Draw?.Invoke(_renderContext);

            SDL_gpu.GPU_Flip(RenderTargetHandle);

            if (Game.Graphics.LimitFramerate)
                Thread.Sleep(1);
        }
    }

    internal void OnShown()
        => Shown?.Invoke(this, EventArgs.Empty);

    internal void OnHidden()
        => Hidden?.Invoke(this, EventArgs.Empty);

    internal void OnInvalidated()
    {
        SDL_gpu.GPU_Flip(RenderTargetHandle);
        Invalidated?.Invoke(this, EventArgs.Empty);
    }

    internal void OnMoved(WindowMoveEventArgs e)
    {
        _position = e.Position;
        Moved?.Invoke(this, e);
    }

    internal void OnResized(WindowSizeEventArgs e)
    {
        if (Game.Graphics.ViewportAutoResize)
        {
            SDL_gpu.GPU_SetWindowResolution(
                (ushort)e.Size.Width,
                (ushort)e.Size.Height
            );
        }

        Resized?.Invoke(this, e);
    }

    internal void OnSizeChanged(WindowSizeEventArgs e)
    {
        _size = e.Size;
        SizeChanged?.Invoke(this, e);
    }

    internal void OnStateChanged(WindowStateEventArgs e)
    {
        _state = e.State;
        StateChanged?.Invoke(this, e);
    }

    internal void OnMouseEntered()
        => MouseEntered?.Invoke(this, EventArgs.Empty);

    internal void OnMouseLeft()
        => MouseLeft?.Invoke(this, EventArgs.Empty);

    internal void OnFocusOffered()
    {
        SDL2.SDL_RaiseWindow(Handle);
    }

    internal void OnFocused()
        => Focused?.Invoke(this, EventArgs.Empty);

    internal void OnUnfocused()
        => Unfocused?.Invoke(this, EventArgs.Empty);

    internal void OnClosed()
        => Closed?.Invoke(this, EventArgs.Empty);

    internal void OnDisplayChanged(DisplayChangedEventArgs e)
        => DisplayChanged?.Invoke(this, e);

    internal void OnQuitRequested(CancelEventArgs e)
    {
        QuitRequested?.Invoke(this, e);

        if (!e.Cancel)
            _exists = false;
    }

    // both OnFileDropped and OnTextDropped are
    // called from DragDropManager instead of WindowEventHandlers
    // to provide atomic drop operation guarantee
    internal void OnFilesDropped(FileDragDropEventArgs e)
        => FilesDropped?.Invoke(this, e);

    internal void OnTextDropped(TextDragDropEventArgs e)
        => TextDropped?.Invoke(this, e);

    private void ExecuteScheduledActions()
    {
        while (true)
        {
            if (!Dispatcher.ActionQueue.TryDequeue(out var schedulerEntry))
                break;

            if (schedulerEntry is ScheduledValueAction scheduledValueAction)
            {
                try
                {
                    scheduledValueAction.ReturnValue = scheduledValueAction.ValueAction?.Invoke();
                    scheduledValueAction.Completed = true;
                }
                catch (Exception e)
                {
                    _log.Exception(e);
                }
            }
            else if (schedulerEntry is ScheduledAction scheduledAction)
            {
                try
                {
                    scheduledAction.Action.Invoke();
                    scheduledAction.Completed = true;
                }
                catch (Exception e)
                {
                    _log.Exception(e);
                }
            }
            else
            {
                _log.Error($"Unexpected scheduled action type '{schedulerEntry.GetType().FullName}'.");
            }
        }
    }

    private void DoTick(float delta)
    {
        if (Update == null)
            return;

        Update(delta);
        ExecuteScheduledActions();
    }

    private void DoFixedTicks(float delta)
    {
        while (PerformanceCounter.Lag >= PerformanceCounter.FixedDelta)
        {
            FixedUpdate?.Invoke(delta);
            PerformanceCounter.Lag -= PerformanceCounter.FixedDelta;
        }
    }

    private SDL2.SDL_HitTestResult HitTestCallback(IntPtr win, IntPtr area, IntPtr data)
    {
        if (_hitTestDelegate != null)
        {
            SDL2.SDL_Point point;

            unsafe
            {
                point = *(SDL2.SDL_Point*)area.ToPointer();
            }

            return (SDL2.SDL_HitTestResult)_hitTestDelegate(this, new Vector2(point.x, point.y));
        }

        return SDL2.SDL_HitTestResult.SDL_HITTEST_NORMAL;
    }

    protected override void FreeNativeResources()
    {
        EnsureOnMainThread();
            
        if (RenderTargetHandle != IntPtr.Zero)
        {
            SDL_gpu.GPU_FreeTarget(RenderTargetHandle);
            RenderTargetHandle = IntPtr.Zero;
        }

        if (_windowHandle != IntPtr.Zero)
        {
            SDL2.SDL_DestroyWindow(_windowHandle);
            _windowHandle = IntPtr.Zero;
        }
    }
}