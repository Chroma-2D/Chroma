namespace Chroma.Windowing;

using Chroma.Natives.Bindings.SDL;

public enum WindowFlash
{
    Brief = SDL2.SDL_FlashOperation.SDL_FLASH_BRIEFLY,
    Continuous = SDL2.SDL_FlashOperation.SDL_FLASH_UNTIL_FOCUSED
}