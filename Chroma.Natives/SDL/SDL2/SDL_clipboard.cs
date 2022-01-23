using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL
{
    internal static partial class SDL2
    {
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SDL_HasClipboardText();

        [DllImport(LibraryName, EntryPoint = "SDL_GetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetClipboardText();
        public static string SDL_GetClipboardText()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetClipboardText(), true);
        }

        [DllImport(LibraryName, EntryPoint = "SDL_SetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe int INTERNAL_SDL_SetClipboardText(
            byte* text
        );
        public static unsafe int SDL_SetClipboardText(
            string text
        ) {
            byte* utf8Text = Utf8EncodeHeap(text);
            int result = INTERNAL_SDL_SetClipboardText(
                utf8Text
            );
            Marshal.FreeHGlobal((IntPtr) utf8Text);
            return result;
        }
    }
}