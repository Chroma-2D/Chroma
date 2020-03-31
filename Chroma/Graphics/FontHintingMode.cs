using Chroma.SDL2;

namespace Chroma.Graphics
{
    public enum FontHintingMode
    {
        None = SDL_ttf.TTF_HINTING_NONE,
        Normal = SDL_ttf.TTF_HINTING_NORMAL,
        Light = SDL_ttf.TTF_HINTING_LIGHT,
        LightSubpixel = SDL_ttf.TTF_HINTING_LIGHT_SUBPIXEL,
        Mono = SDL_ttf.TTF_HINTING_MONO
    }
}