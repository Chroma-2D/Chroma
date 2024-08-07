namespace Chroma.Natives.Bindings.SDL;

using System;
using System.Runtime.InteropServices;

internal static partial class SDL2
{
    public static uint SDL_FOURCC(byte A, byte B, byte C, byte D)
    {
        return (uint) (A | (B << 8) | (C << 16) | (D << 24));
    }

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr SDL_malloc(IntPtr size);
        
    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void SDL_free(IntPtr memblock);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr SDL_memcpy(IntPtr dst, IntPtr src, IntPtr len);

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern unsafe void SDL_memset(void* buf, byte c, IntPtr len);
        
    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void SDL_OutOfMemory();
}