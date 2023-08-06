using System;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Windowing
{
    internal sealed class PlatformWindowInformation
    {
        private SDL2.SDL_SysWMinfo _info;

        public IntPtr SystemWindowHandle
        {
            get
            {
                return _info.subsystem switch
                {
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_WINDOWS => _info.win.window,
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_WINRT => _info.winrt.window,
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_X11 => _info.x11.window,
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_WAYLAND => _info.wl.shell_surface,
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_DIRECTFB => _info.dfb.window,
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_COCOA => _info.cocoa.window,
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_UIKIT => _info.uikit.window,
                    SDL2.SDL_SYSWM_TYPE.SDL_SYSWM_ANDROID => _info.android.window,
                    _ => throw new PlatformNotSupportedException("Your platform is not supported.")
                };
            }
        }
        
        public PlatformWindowInformation(IntPtr sdlWindowHandle)
        {
            SDL2.SDL_GetVersion(out _info.version);

            if (sdlWindowHandle == IntPtr.Zero)
            {
                throw new FrameworkException("SDL window handle is null.");
            }

            if (!SDL2.SDL_GetWindowWMInfo(sdlWindowHandle, ref _info))
            {
                throw new FrameworkException("Unable to get platform window information.");
            }
        }
    }
}