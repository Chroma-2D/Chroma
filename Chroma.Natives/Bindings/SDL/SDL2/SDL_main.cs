using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetMainReady();

        /* This is used as a function pointer to a C main() function */
        public delegate int SDL_main_func(int argc, IntPtr argv);

        /* Use this function with UWP to call your C# Main() function! */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_WinRTRunApp(
            SDL_main_func mainFunction,
            IntPtr reserved
        );

        /* Use this function with iOS to call your C# Main() function!
        * Only available in SDL 2.0.10 or higher.
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UIKitRunApp(
            int argc,
            IntPtr argv,
            SDL_main_func mainFunction
        );
    }
}