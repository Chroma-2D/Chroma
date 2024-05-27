using System.Numerics;

namespace Chroma.Input
{
    public sealed class MouseMoveEventArgs
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

        public static MouseMoveEventArgs With(Vector2 position, Vector2 delta, MouseButtonState buttonState) => new(
            position,
            delta,
            buttonState
        );
        
        public MouseMoveEventArgs WithPosition(Vector2 position) => With(
            position,
            Delta,
            ButtonState
        );
        
        public MouseMoveEventArgs WithDelta(Vector2 delta) => With(
            Position,
            delta,
            ButtonState
        );
        
        public MouseMoveEventArgs WithButtonState(MouseButtonState buttonState) => With(
            Position,
            Delta,
            buttonState
        );
    }
}