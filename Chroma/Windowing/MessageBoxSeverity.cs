using Chroma.Natives.SDL;

namespace Chroma.Windowing
{
    public enum MessageBoxSeverity : uint
    {
        Information = SDL2.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
        Warning = SDL2.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
        Error = SDL2.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR
    }
}