using System;
using System.Numerics;
using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public class MouseButtonEventArgs : System.EventArgs
    {
        public Vector2 Position { get; }
        public MouseButton Button { get; }

        public bool Pressed { get; }
        public byte ClickCount { get; }

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
                _ => throw new Exception("Unexpected mouse button constant.")
            };

            Pressed = state == SDL2.SDL_PRESSED;
            ClickCount = clickCount;
        }
    }
}