using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Chroma.Audio;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Natives.SDL;
using Chroma.Threading;
using Chroma.Windowing;

namespace Chroma
{
    public class Game
    {
        private static bool _wasConstructedAlready;
        private static DefaultScene _defaultScene;
        private int _fixedTimeStepTarget;

        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public int FixedTimeStepTarget
        {
            get => _fixedTimeStepTarget;

            protected set
            {
                _fixedTimeStepTarget = value;
                PerformanceCounter.FixedDelta = 1f / value;
            }
        }

        public Window Window { get; private set; }
        public GraphicsManager Graphics { get; private set; }
        public AudioManager Audio => AudioManager.Instance;

        public IContentProvider Content { get; protected set; }

        public static string LocationOnDisk => Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location
        );

        public Game(bool constructDefaultScene = true)
        {
            if (_wasConstructedAlready)
            {
                throw new InvalidOperationException(
                    "An instance of the Game class can only be constructed " +
                    "once in the entire application's lifetime."
                );
            }

            _wasConstructedAlready = true;

            if (constructDefaultScene)
                _defaultScene = new DefaultScene(this);

            InitializeThreading();
            InitializeGraphics();
            InitializeAudio();
            InitializeContent();

            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        }

        public void Run()
        {
            if (Window.Exists)
                throw new InvalidOperationException("The game is already running.");

            LoadContent();
            Window.Run();
        }

        public void Quit()
        {
            Content.Dispose();

            AudioManager.Instance.Close();
            SDL_gpu.GPU_Quit();
            SDL2.SDL_Quit();

            Environment.Exit(0);
        }

        protected virtual void Draw(RenderContext context)
            => _defaultScene?.Draw(context);

        protected virtual void Update(float delta)
            => _defaultScene.Update(delta);

        protected virtual void FixedUpdate(float delta)
        {
        }

        protected virtual void LoadContent()
        {
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

        internal void OnMouseMoved(MouseMoveEventArgs e)
            => MouseMoved(e);

        internal void OnMousePressed(MouseButtonEventArgs e)
            => MousePressed(e);

        internal void OnMouseReleased(MouseButtonEventArgs e)
            => MouseReleased(e);

        internal void OnWheelMoved(MouseWheelEventArgs e)
            => WheelMoved(e);

        internal void OnKeyPressed(KeyEventArgs e)
            => KeyPressed(e);

        internal void OnKeyReleased(KeyEventArgs e)
            => KeyReleased(e);

        internal void OnTextInput(TextInputEventArgs e)
            => TextInput(e);

        internal void OnControllerConnected(ControllerEventArgs e)
            => ControllerConnected(e);

        internal void OnControllerDisconnected(ControllerEventArgs e)
            => ControllerDisconnected(e);

        internal void OnControllerButtonPressed(ControllerButtonEventArgs e)
            => ControllerButtonPressed(e);

        internal void OnControllerButtonReleased(ControllerButtonEventArgs e)
            => ControllerButtonReleased(e);

        internal void OnControllerAxisMoved(ControllerAxisEventArgs e)
            => ControllerAxisMoved(e);

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Error(
                $"Unhandled exception. There are two people who could've fucked this up. You or me.\n\n{e.ExceptionObject}");
        }

        private void InitializeThreading()
        {
            Dispatcher.MainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        private void InitializeGraphics()
        {
            FixedTimeStepTarget = 75;

            Graphics = new GraphicsManager(this);
            Window = new Window(this)
            {
                Draw = Draw,
                Update = Update,
                FixedUpdate = FixedUpdate
            };

            Graphics.VerticalSyncMode = VerticalSyncMode.Retrace;
            Window.SetIcon(EmbeddedAssets.DefaultIconTexture);
        }

        private void InitializeAudio()
        {
            AudioManager.Instance.Initialize();
        }

        private void InitializeContent()
        {
            Content = new FileSystemContentProvider();
        }
    }
}