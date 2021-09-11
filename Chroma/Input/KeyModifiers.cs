using System;
using Chroma.Natives.SDL;

namespace Chroma.Input
{
    [Flags]
    public enum KeyModifiers
    {
        None = SDL2.SDL_Keymod.KMOD_NONE,
        
        LeftShift = SDL2.SDL_Keymod.KMOD_LSHIFT,
        RightShift = SDL2.SDL_Keymod.KMOD_RSHIFT,
        
        LeftControl = SDL2.SDL_Keymod.KMOD_LCTRL,
        RightControl = SDL2.SDL_Keymod.KMOD_RCTRL,
        
        LeftAlt = SDL2.SDL_Keymod.KMOD_LALT,
        RightAlt = SDL2.SDL_Keymod.KMOD_RALT,
        
        LeftSuper = SDL2.SDL_Keymod.KMOD_LGUI,
        RightSuper = SDL2.SDL_Keymod.KMOD_RGUI,
        
        NumLock = SDL2.SDL_Keymod.KMOD_NUM,
        CapsLock = SDL2.SDL_Keymod.KMOD_CAPS,
        
        Mode = SDL2.SDL_Keymod.KMOD_MODE
    }
}