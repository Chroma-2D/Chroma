using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
        [DllImport(LibraryName, EntryPoint = "SDL_GetPlatform", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetPlatform();
        public static string SDL_GetPlatform()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetPlatform());
        }
    }
}