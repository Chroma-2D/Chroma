using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Chroma.Audio;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Chroma.Natives.SDL;
using Chroma.Windowing;

namespace Chroma
{
    public class Game
    {
        private readonly Thread _fixedUpdateThread;
        private static bool _wasConstructedAlready;
        private static DefaultScene _defaultScene;

        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        public Window Window { get; }
        public GraphicsManager Graphics { get; }
        public AudioManager Audio { get; }
        public IContentProvider Content { get; protected set; }

        public static string LocationOnDisk => Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location
        );

        public int FixedUpdateFrequency { get; protected set; } = 75;

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
            
            _fixedUpdateThread = new Thread(FixedUpdateThread);

            Graphics = new GraphicsManager(this);
            Audio = new AudioManager();

            Window = new Window(this)
            {
                Draw = Draw,
                Update = Update
            };

            Window.SetIcon(EmbeddedAssets.DefaultIconTexture);
            Content = new FileSystemContentProvider(this);
            
            // Can only initialize these after Window creates OpenGL context.
            GraphicsManager.LineThickness = 1;
            GraphicsManager.DisplaySynchronization = DisplaySynchronization.VerticalRetrace;
            
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        }

        public void Run()
        {
            if (Window.Exists)
                throw new InvalidOperationException("The game is already running.");
            
            LoadContent();
            Window.Run(() => _fixedUpdateThread.Start());
        }

        public void Quit()
        {
            Audio.Dispose();
            Content.Dispose();

            SDL_mixer.Mix_Quit();
            SDL_gpu.GPU_Quit();
            SDL2.SDL_Quit();

            Environment.Exit(0);
        }

        protected virtual void Draw(RenderContext context)
        {
            if (_defaultScene != null)
                _defaultScene.Draw(context);
        }

        protected virtual void LoadContent()
        {
        }

        protected virtual void Update(float delta)
        {
            if (_defaultScene != null)
                _defaultScene.Update(delta);
        }

        protected virtual void FixedUpdate(float fixedDelta)
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

        private void FixedUpdateThread()
        {
            while (true)
            {
                if (!Window.Exists)
                    break;

                var waitTime = 1f / FixedUpdateFrequency;

                lock (this)
                {
                    FixedUpdate(waitTime);
                }

                Thread.Sleep((int)(waitTime * 1000));
            }
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error(
                $"Unhandled exception. There are two people who could've fucked this up. You or me.\n\n{e.ExceptionObject}");
        }
    }
}