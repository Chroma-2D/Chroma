namespace Chroma.Natives.Bindings.SDL;

using System;
using System.Runtime.InteropServices;

internal static partial class SDL2
{
    [DllImport(LibraryName, EntryPoint = "SDL_GetPlatform", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr INTERNAL_SDL_GetPlatform();
    public static string SDL_GetPlatform()
    {
        return UTF8_ToManaged(INTERNAL_SDL_GetPlatform());
    }
}