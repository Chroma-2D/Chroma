using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public enum DisplayOrientation
    {
        Unknown = SDL2.SDL_DisplayOrientation.SDL_ORIENTATION_UNKNOWN,
        Landscape = SDL2.SDL_DisplayOrientation.SDL_ORIENTATION_LANDSCAPE,
        LandscapeInverted = SDL2.SDL_DisplayOrientation.SDL_ORIENTATION_LANDSCAPE_FLIPPED,
        Portrait = SDL2.SDL_DisplayOrientation.SDL_ORIENTATION_PORTRAIT,
        PortraitInverted = SDL2.SDL_DisplayOrientation.SDL_ORIENTATION_PORTRAIT_FLIPPED
    }
}