using Chroma.Input.EventArgs;
using Chroma.SDL2;
using System;

namespace Chroma.Input
{
    public static class Mouse
    {
        public static event EventHandler<MouseMoveEventArgs> Moved;
        public static event EventHandler<MouseButtonEventArgs> Pressed;
        public static event EventHandler<MouseButtonEventArgs> Released;
        public static event EventHandler<MouseWheelEventArgs> WheelMoved;

        public static Vector2 GetPosition()
        {
            _ = SDL.SDL_GetMouseState(out int x, out int y);
            return new Vector2(x, y);
        }

        internal static void OnMoved(MouseMoveEventArgs e)
            => Moved?.Invoke(null, e);

        internal static void OnPressed(MouseButtonEventArgs e)
            => Pressed?.Invoke(null, e);

        internal static void OnReleased(MouseButtonEventArgs e)
            => Released?.Invoke(null, e);

        internal static void OnWheelMoved(MouseWheelEventArgs e)
            => WheelMoved?.Invoke(null, e);
    }
}
