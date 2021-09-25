using System;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    internal static class PixelFormatConverter
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly(); 
        
        internal static IntPtr ConvertSurfaceToFormat(IntPtr surface, PixelFormat format, bool freeOriginal = true)
        {
            if (surface == IntPtr.Zero)
                throw new FrameworkException("Surface pointer cannot be null.");
            
            uint sdlFormat = 0;

            sdlFormat = format switch
            {
                PixelFormat.BGR => SDL2.SDL_PIXELFORMAT_BGR888,
                PixelFormat.RGB => SDL2.SDL_PIXELFORMAT_RGB888,
                PixelFormat.ABGR => SDL2.SDL_PIXELFORMAT_ABGR8888,
                PixelFormat.BGRA => SDL2.SDL_PIXELFORMAT_BGRA8888,
                PixelFormat.RGBA => SDL2.SDL_PIXELFORMAT_RGBA8888,
                _ => throw new FrameworkException("Unrecognized pixel format.")
            };

            var pixelFormat = SDL2.SDL_AllocFormat(sdlFormat);

            if (pixelFormat == IntPtr.Zero)
            {
                _log.Error($"Failed to allocate SDL pixel format: {SDL2.SDL_GetError()}");
                return IntPtr.Zero;
            }
            
            var newSurface = SDL2.SDL_ConvertSurface(surface, pixelFormat, 0);
            if (newSurface == IntPtr.Zero)
            {
                _log.Error($"Surface conversion has failed: {SDL2.SDL_GetError()}");
                return IntPtr.Zero;
            }

            SDL2.SDL_FreeFormat(pixelFormat);

            if (freeOriginal)
            {
                SDL2.SDL_FreeSurface(surface);
            }

            return newSurface;
        }
    }
}