using Chroma.Natives.Bindings.SDL;

namespace Chroma.Input
{
    public enum MouseButton : uint
    {
        Left = SDL2.SDL_BUTTON_LEFT,
        Right = SDL2.SDL_BUTTON_RIGHT,
        Middle = SDL2.SDL_BUTTON_MIDDLE,
        X1 = SDL2.SDL_BUTTON_X1,
        X2 = SDL2.SDL_BUTTON_X2
    }
}