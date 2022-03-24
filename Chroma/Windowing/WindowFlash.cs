using Chroma.Natives.Bindings.SDL;

namespace Chroma.Windowing
{
    public enum WindowFlash
    {
        Brief = SDL2.SDL_FlashOperation.SDL_FLASH_BRIEFLY,
        Continuous = SDL2.SDL_FlashOperation.SDL_FLASH_UNTIL_FOCUSED
    }
}