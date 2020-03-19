using Chroma.Windowing;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Chroma.Diagnostics;
using System;
using Chroma.Input;

namespace Chroma
{
    public class Game
    {
        protected Window Window { get; }
        protected GraphicsManager Graphics => GraphicsManager.Instance;

        public bool Running { get; private set; }

        public Game()
        {
            Window = new Window(this)
            {
                Draw = Draw,
                Update = Update
            };
        }

        public void Run()
        {
            Window.Run();
        }

        protected virtual void Draw(RenderContext context)
        {
        }

        protected virtual void Update(float delta)
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
    }
}
