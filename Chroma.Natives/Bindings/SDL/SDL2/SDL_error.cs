namespace Chroma.Natives.Bindings.SDL;

using System;
using System.Runtime.InteropServices;

internal static partial class SDL2
{
    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_ClearError();

    [DllImport(LibraryName, EntryPoint = "SDL_GetError", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr INTERNAL_SDL_GetError();
    public static string SDL_GetError()
    {
        return UTF8_ToManaged(INTERNAL_SDL_GetError());
    }

    [DllImport(LibraryName, EntryPoint = "SDL_SetError", CallingConvention = CallingConvention.Cdecl)]
    private static extern unsafe void INTERNAL_SDL_SetError(byte* fmtAndArglist);
    public static unsafe void SDL_SetError(string fmtAndArglist)
    {
        int utf8FmtAndArglistBufSize = Utf8Size(fmtAndArglist);
        byte* utf8FmtAndArglist = stackalloc byte[utf8FmtAndArglistBufSize];
        INTERNAL_SDL_SetError(
            Utf8Encode(fmtAndArglist, utf8FmtAndArglist, utf8FmtAndArglistBufSize)
        );
    }

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr SDL_GetErrorMsg(IntPtr errstr, int maxlength);
}