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
    }
}
