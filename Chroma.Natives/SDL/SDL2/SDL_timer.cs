using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL
{
    internal static partial class SDL2
    {
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_GetTicks64();
		
        /* Get the current value of the high resolution counter */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_GetPerformanceCounter();

        /* Get the count per second of the high resolution counter */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_GetPerformanceFrequency();
    }
}