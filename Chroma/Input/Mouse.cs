using System.Numerics;
using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public static class Mouse
    {
        public static Vector2 GetPosition()
        {
            _ = SDL2.SDL_GetMouseState(out var x, out var y);
            return new Vector2(x, y);
        }

        public static bool IsButtonDown(MouseButton button)
        {
            var state = SDL2.SDL_GetMouseState(out _, out _);
            var mask = SDL2.SDL_BUTTON((uint)button);

            return (state & mask) != 0;
        }

        public static bool IsButtonUp(MouseButton button)
            => !IsButtonDown(button);
    }
}
