using System;
using System.Numerics;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Input
{
    public sealed class MouseButtonEventArgs
    {
        public Vector2 Position { get; }
        public MouseButton Button { get; }

        public bool Pressed { get; }
        public byte ClickCount { get; }

        private MouseButtonEventArgs(Vector2 position, bool pressed, MouseButton button, byte clickCount)
        {
            Position = position;
            Button = button;
            Pressed = pressed;
            ClickCount = clickCount;
        }
        
        internal MouseButtonEventArgs(Vector2 position, byte state, uint button, byte clickCount)
        {
            Position = position;

            Button = button switch
            {
                SDL2.SDL_BUTTON_LEFT => MouseButton.Left,
                SDL2.SDL_BUTTON_RIGHT => MouseButton.Right,
                SDL2.SDL_BUTTON_MIDDLE => MouseButton.Middle,
                SDL2.SDL_BUTTON_X1 => MouseButton.X1,
                SDL2.SDL_BUTTON_X2 => MouseButton.X2,
                _ => throw new ArgumentException("Unexpected mouse button constant.")
            };

            Pressed = state == SDL2.SDL_PRESSED;
            ClickCount = clickCount;
        }

        public static MouseButtonEventArgs With(Vector2 position, bool pressed, MouseButton mouseButton, byte clickCount) => new(
            position,
            pressed,
            mouseButton,
            clickCount
        );

        public MouseButtonEventArgs WithPosition(Vector2 position) => With(
            position,
            Pressed,
            Button,
            ClickCount
        );

        public MouseButtonEventArgs WithPressState(bool pressed) => With(
            Position,
            pressed,
            Button,
            ClickCount
        );

        public MouseButtonEventArgs WithButton(MouseButton button) => With(
            Position,
            Pressed,
            button,
            ClickCount
        );

        public MouseButtonEventArgs WithClickCount(byte clickCount) => With(
            Position,
            Pressed,
            Button,
            clickCount
        );
    }
}