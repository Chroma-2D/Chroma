using Chroma.Natives.Bindings.SDL;

namespace Chroma.Windowing
{
    public enum MessageBoxSeverity : uint
    {
        Information = SDL2.SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
        Warning = SDL2.SDL_MessageBoxFlags.SDL_MESSAGEBOX_WARNING,
        Error = SDL2.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR
    }
}