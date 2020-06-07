using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading;
using Chroma.Audio;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Chroma.Natives.SDL;
using Chroma.Windowing;

namespace Chroma
{
    public class Game
    {
        private readonly Thread _fixedUpdateThread;

        public Texture LogoTexture { get; }

        public Window Window { get; }
        public GraphicsManager Graphics { get; }
        public AudioManager Audio { get; }
        public IContentProvider Content { get; }

        public int FixedUpdateFrequency { get; protected set; } = 75;

        public Game()
        {
            _fixedUpdateThread = new Thread(FixedUpdateThread);

            Graphics = new GraphicsManager(this);
            Audio = new AudioManager();

            Window = new Window(this)
            {
                Draw = Draw,
                Update = Update
            };

            using var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Chroma.Resources.logo.png");
            LogoTexture = new Texture(resourceStream);

            Content = new FileSystemContentProvider(this);
        }

        public void Run()
        {
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
            if (LogoTexture == null || LogoTexture.Disposed)
                return;

            context.Clear(Color.Black);
            context.DrawTexture(
                LogoTexture,
                new Vector2(
                    (Window.Properties.Width / 2) - (LogoTexture.Width / 2),
                    (Window.Properties.Height / 2) - (LogoTexture.Height / 2)
                ),
                Vector2.One,
                Vector2.Zero,
                0f
            );
        }

        protected virtual void LoadContent()
        {
        }

        protected virtual void Update(float delta)
        {
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
                if (!Window.Running)
                    break;

                var waitTime = 1f / FixedUpdateFrequency;

                lock (this)
                {
                    FixedUpdate(waitTime);
                }
                Thread.Sleep((int)(waitTime * 1000));
            }
        }
    }
}
