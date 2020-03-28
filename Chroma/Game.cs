using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Chroma.Windowing;

namespace Chroma
{
    public class Game
    {
        public Window Window { get; }
        public GraphicsManager Graphics { get; }

        public bool Running { get; private set; }

        public Game()
        {
            Graphics = new GraphicsManager(this);

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
    }
}
