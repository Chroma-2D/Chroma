﻿namespace Chroma;

using System;
using Chroma.Audio;
using Chroma.Boot;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Extensibility;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.GameControllers;
using Chroma.Natives.Bindings.SDL;
using Chroma.Natives.Boot;
using Chroma.Threading;
using Chroma.Windowing;

public class Game : IDisposable
{
    private static DefaultScene? _defaultScene;
    private static BootScene? _bootScene;
        
    private static readonly Log _log = LogManager.GetForCurrentAssembly();
        
    private int _fixedTimeStepTarget;
    private IContentProvider? _content;

    internal static bool WasConstructed { get; private set; }

    public int FixedTimeStepTarget
    {
        get => _fixedTimeStepTarget;

        protected set
        {
            _fixedTimeStepTarget = value;
            PerformanceCounter.FixedDelta = 1f / value;
        }
    }

    public GameStartupOptions StartupOptions { get; }
    public Window Window { get; private set; } = null!;
    public GraphicsManager Graphics { get; private set; } = null!;
    public AudioManager Audio { get; private set; } = null!;
    
    public Game(GameStartupOptions? options = null)
    {
#if !DEBUG
            _log.LogLevel = LogLevel.Info | LogLevel.Warning | LogLevel.Error;
#endif

        if (WasConstructed)
        {
            throw new InvalidOperationException(
                "An instance of the Game class can only be constructed " +
                "once in the entire application's lifetime."
            );
        }

        BootHints.Lock();
        
        // This breaks the architecture, but
        // I don't see a better way of doing it
        // right now.
        if (ModuleInitializer.BootConfig.HookSdlLog)
        {
            SdlLogHook.Enable();
        }

        options ??= GameStartupOptions.Default;

        StartupOptions = options;
        WasConstructed = true;

        if (StartupOptions.ConstructDefaultScene)
            _defaultScene = new DefaultScene(this);

        InitializeThreading();
        InitializeGraphics();
        InitializeAudio();

        AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;

        HookRegistry.Initialize(this);
    }

    public void Run()
    {
        if (Window.Exists)
            throw new InvalidOperationException("The game is already running.");

        if (!StartupOptions.UseBootSplash)
        {
            FinishBoot();
        }

        Window.Run();
    }

    public void Quit(int exitCode = 0)
    {
        Dispose();
            
        SDL_gpu.GPU_Quit();
        SDL2.SDL_Quit();

        Environment.Exit(exitCode);
    }
        
    protected virtual void Initialize(IContentProvider content)
    {
    }

    protected virtual void Draw(RenderContext context)
        => _defaultScene?.Draw(context);

    protected virtual void Update(float delta)
        => _defaultScene?.Update(delta);

    protected virtual void FixedUpdate(float delta)
    {
    }

    protected virtual IContentProvider InitializeContentPipeline()
    {
        return new FileSystemContentProvider();
    }

    protected virtual void MouseMoved(MouseMoveEventArgs e)
    {
    }

    protected virtual void MousePressed(MouseButtonEventArgs e)
    {
    }

    protected virtual void MouseReleased(MouseButtonEventArgs e)
    {
    }

    protected virtual void WheelMoved(MouseWheelEventArgs e)
    {
    }

    protected virtual void KeyPressed(KeyEventArgs e)
    {
    }

    protected virtual void KeyReleased(KeyEventArgs e)
    {
    }

    protected virtual void TextInput(TextInputEventArgs e)
    {
    }

    protected virtual void ControllerConnected(ControllerEventArgs e)
    {
    }

    protected virtual void ControllerDisconnected(ControllerEventArgs e)
    {
    }

    protected virtual void ControllerButtonPressed(ControllerButtonEventArgs e)
    {
    }

    protected virtual void ControllerButtonReleased(ControllerButtonEventArgs e)
    {
    }

    protected virtual void ControllerAxisMoved(ControllerAxisEventArgs e)
    {
    }

    protected virtual void ControllerTouchpadMoved(ControllerTouchpadEventArgs e)
    {
    }

    protected virtual void ControllerTouchpadTouched(ControllerTouchpadEventArgs e)
    {
    }

    protected virtual void ControllerTouchpadReleased(ControllerTouchpadEventArgs e)
    {
    }

    protected virtual void ControllerGyroscopeStateChanged(ControllerSensorEventArgs e)
    {
    }

    protected virtual void ControllerAccelerometerStateChanged(ControllerSensorEventArgs e)
    {
    }
        
    protected virtual void OnDispose()
    {
    }

    internal void OnInitialize(IContentProvider content)
        => HookRegistry.WrapCall(HookPoint.Initialize, content, Initialize);

    internal void OnDraw(RenderContext context)
        => HookRegistry.WrapCall(HookPoint.Draw, context, Draw);

    internal void OnUpdate(float delta)
        => HookRegistry.WrapCall(HookPoint.Update, delta, Update);

    internal void OnFixedUpdate(float delta)
        => HookRegistry.WrapCall(HookPoint.FixedUpdate, delta, FixedUpdate);

    internal void OnMouseMoved(MouseMoveEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.MouseMoved, ref e, MouseMoved);

    internal void OnMousePressed(MouseButtonEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.MousePressed, ref e, MousePressed);

    internal void OnMouseReleased(MouseButtonEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.MouseReleased, ref e, MouseReleased);

    internal void OnWheelMoved(MouseWheelEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.WheelMoved, ref e, WheelMoved);

    internal void OnKeyPressed(KeyEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.KeyPressed, ref e, KeyPressed);

    internal void OnKeyReleased(KeyEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.KeyReleased, ref e, KeyReleased);

    internal void OnTextInput(TextInputEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.TextInput, ref e, TextInput);

    internal void OnControllerConnected(ControllerEventArgs e)
        => HookRegistry.WrapCall(HookPoint.ControllerConnected, e, ControllerConnected);

    internal void OnControllerDisconnected(ControllerEventArgs e)
        => HookRegistry.WrapCall(HookPoint.ControllerDisconnected, e, ControllerDisconnected);

    internal void OnControllerButtonPressed(ControllerButtonEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerButtonPressed, ref e, ControllerButtonPressed);

    internal void OnControllerButtonReleased(ControllerButtonEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerButtonReleased, ref e, ControllerButtonReleased);

    internal void OnControllerAxisMoved(ControllerAxisEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerAxisMoved, ref e, ControllerAxisMoved);

    internal void OnControllerTouchpadMoved(ControllerTouchpadEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerTouchpadMoved, ref e, ControllerTouchpadMoved);

    internal void OnControllerTouchpadTouched(ControllerTouchpadEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerTouchpadTouched, ref e, ControllerTouchpadTouched);

    internal void OnControllerTouchpadReleased(ControllerTouchpadEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerTouchpadReleased, ref e, ControllerTouchpadReleased);

    internal void OnControllerGyroscopeStateChanged(ControllerSensorEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerGyroscopeStateChanged, ref e, ControllerGyroscopeStateChanged);

    internal void OnControllerAccelerometerStateChanged(ControllerSensorEventArgs e)
        => HookRegistry.WrapCallMutable(HookPoint.ControllerAccelerometerStateChanged, ref e, ControllerAccelerometerStateChanged);

    private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        _log.Error(
            $"Unhandled exception. There are two people who could've fucked this up. " +
            $"You or me.\n\n{e.ExceptionObject}"
        );
    }

    private void InitializeThreading()
    {
        Dispatcher.MainThreadId = Environment.CurrentManagedThreadId;
    }

    private void InitializeGraphics()
    {
        Graphics = new GraphicsManager(StartupOptions);
        Window = new Window(this);

        // This is a kludge.
        // Do not touch or shit will break.
        //
        // What shit you wonder?
        // Something related to texture initialization
        // overwriting things in memory but it happens
        // very early so I am not sure if it's FreeType
        // fucking things up or SDL_gpu or whatever.
        //
        // This fixes it. So do not touch.
        // - 23.03.2022
        //
        EmbeddedAssets.LoadEmbeddedAssets();

        if (StartupOptions.UseBootSplash)
        {
            FixedTimeStepTarget = 60;

            _bootScene = new BootScene(this);
            _bootScene.Finished += BootSceneFinished;

            Window.Draw = _bootScene.Draw;
            Window.FixedUpdate = _bootScene.FixedUpdate;
        }
        else
        {
            InitializeGraphicsDefaults();
        }

        Graphics.VerticalSyncMode = VerticalSyncMode.Retrace;
        Window.SetIcon(EmbeddedAssets.DefaultIconTexture);
    }

    private void InitializeGraphicsDefaults()
    {
        FixedTimeStepTarget = 75;

        Window.Draw = OnDraw;
        Window.Update = OnUpdate;
        Window.FixedUpdate = OnFixedUpdate;
        Window.InitializeEventDispatcher();
    }

    private void InitializeAudio()
    {
        Audio = new AudioManager();
        Audio.Initialize();
    }

    private void BootSceneFinished(object? sender, EventArgs e)
    {
        if (_bootScene != null)
        {
            _bootScene.Finished -= BootSceneFinished;
        }

        FinishBoot();
        InitializeGraphicsDefaults();
    }

    private void FinishBoot()
    {
        _content = InitializeContentPipeline();
            
        OnInitialize(_content);
    }

    public void Dispose()
    {
        OnDispose();
            
        Window.Dispose();
        _content?.Dispose();

        AudioOutput.Instance.Close();
        AudioInput.Instance.Close();
    }
}