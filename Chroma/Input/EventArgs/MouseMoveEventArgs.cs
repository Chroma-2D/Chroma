using System.Numerics;

namespace Chroma.Input.EventArgs
{
    public class MouseMoveEventArgs : System.EventArgs
    {
        public Vector2 Position { get; }
        public Vector2 Delta { get; }

        public MouseButtonState ButtonState { get; }

        internal MouseMoveEventArgs(Vector2 position, Vector2 delta, MouseButtonState buttonState)
        {
            Position = position;
            Delta = delta;

            ButtonState = buttonState;
        }
    }
}