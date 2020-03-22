using Chroma.SDL2;

namespace Chroma.Input
{
    public enum MouseButton : uint
    {
        Left = SDL.SDL_BUTTON_LEFT,
        Right = SDL.SDL_BUTTON_RIGHT,
        Middle = SDL.SDL_BUTTON_MIDDLE,
        X1 = SDL.SDL_BUTTON_X1,
        X2 = SDL.SDL_BUTTON_X2
    }
}
