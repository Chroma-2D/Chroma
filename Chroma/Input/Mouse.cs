using Chroma.SDL2;

namespace Chroma.Input
{
    public static class Mouse
    {
        public static Vector2 GetPosition()
        {
            _ = SDL.SDL_GetMouseState(out int x, out int y);
            return new Vector2(x, y);
        }

        public static bool IsButtonDown(MouseButton button)
        {
            var state = SDL.SDL_GetMouseState(out int _, out int _);
            var mask = SDL.SDL_BUTTON((uint)button);

            return (state & mask) != 0;
        }

        public static bool IsButtonUp(MouseButton button)
            => !IsButtonDown(button);
    }
}
