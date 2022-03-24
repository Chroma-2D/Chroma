using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
        /* Note: SDL_Cursor is a typedef normally. We'll treat it as
        * an IntPtr, because C# doesn't do typedefs. Yay!
        */
        public const uint SDL_BUTTON_LEFT = 1;
        public const uint SDL_BUTTON_MIDDLE = 2;
        public const uint SDL_BUTTON_RIGHT = 3;
        public const uint SDL_BUTTON_X1 = 4;
        public const uint SDL_BUTTON_X2 = 5;

        public static readonly uint SDL_BUTTON_LMASK = SDL_BUTTON(SDL_BUTTON_LEFT);
        public static readonly uint SDL_BUTTON_MMASK = SDL_BUTTON(SDL_BUTTON_MIDDLE);
        public static readonly uint SDL_BUTTON_RMASK = SDL_BUTTON(SDL_BUTTON_RIGHT);
        public static readonly uint SDL_BUTTON_X1MASK = SDL_BUTTON(SDL_BUTTON_X1);
        public static readonly uint SDL_BUTTON_X2MASK = SDL_BUTTON(SDL_BUTTON_X2);

        /* System cursor types */
        public enum SDL_SystemCursor
        {
            SDL_SYSTEM_CURSOR_ARROW, // Arrow
            SDL_SYSTEM_CURSOR_IBEAM, // I-beam
            SDL_SYSTEM_CURSOR_WAIT, // Wait
            SDL_SYSTEM_CURSOR_CROSSHAIR, // Crosshair
            SDL_SYSTEM_CURSOR_WAITARROW, // Small wait cursor (or Wait if not available)
            SDL_SYSTEM_CURSOR_SIZENWSE, // Double arrow pointing northwest and southeast
            SDL_SYSTEM_CURSOR_SIZENESW, // Double arrow pointing northeast and southwest
            SDL_SYSTEM_CURSOR_SIZEWE, // Double arrow pointing west and east
            SDL_SYSTEM_CURSOR_SIZENS, // Double arrow pointing north and south
            SDL_SYSTEM_CURSOR_SIZEALL, // Four pointed arrow pointing north, south, east, and west
            SDL_SYSTEM_CURSOR_NO, // Slashed circle or crossbones
            SDL_SYSTEM_CURSOR_HAND, // Hand
            SDL_NUM_SYSTEM_CURSORS
        }

        /* Get the window which currently has mouse focus */
        /* Return value is an SDL_Window pointer */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetMouseFocus();

        /* Get the current state of the mouse */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetMouseState(out int x, out int y);

        /* Get the current state of the mouse */
        /* This overload allows for passing NULL to x */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetMouseState(IntPtr x, out int y);

        /* Get the current state of the mouse */
        /* This overload allows for passing NULL to y */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetMouseState(out int x, IntPtr y);

        /* Get the current state of the mouse */
        /* This overload allows for passing NULL to both x and y */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetMouseState(IntPtr x, IntPtr y);

        /* Get the current state of the mouse, in relation to the desktop.
        * Only available in 2.0.4 or higher.
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetGlobalMouseState(out int x, out int y);

        /* Get the mouse state with relative coords*/
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetRelativeMouseState(out int x, out int y);

        /* Set the mouse cursor's position (within a window) */
        /* window is an SDL_Window pointer */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_WarpMouseInWindow(IntPtr window, int x, int y);

        /* Set the mouse cursor's position in global screen space.
        * Only available in 2.0.4 or higher.
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_WarpMouseGlobal(int x, int y);

        /* Enable/Disable relative mouse mode (grabs mouse, rel coords) */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_SetRelativeMouseMode(bool enabled);

        /* Capture the mouse, to track input outside an SDL window.
        * Only available in 2.0.4 or higher.
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_CaptureMouse(bool enabled);

        /* Query if the relative mouse mode is enabled */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SDL_GetRelativeMouseMode();

        /* Create a cursor from bitmap data (amd mask) in MSB format.
        * data and mask are byte arrays, and w must be a multiple of 8.
        * return value is an SDL_Cursor pointer.
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateCursor(
            IntPtr data,
            IntPtr mask,
            int w,
            int h,
            int hot_x,
            int hot_y
        );

        /* Create a cursor from an SDL_Surface.
        * IntPtr refers to an SDL_Cursor*, surface to an SDL_Surface*
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateColorCursor(
            IntPtr surface,
            int hot_x,
            int hot_y
        );

        /* Create a cursor from a system cursor id.
        * return value is an SDL_Cursor pointer
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateSystemCursor(SDL_SystemCursor id);

        /* Set the active cursor.
        * cursor is an SDL_Cursor pointer
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetCursor(IntPtr cursor);

        /* Return the active cursor
        * return value is an SDL_Cursor pointer
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetCursor();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetDefaultCursor();

        /* Frees a cursor created with one of the CreateCursor functions.
        * cursor in an SDL_Cursor pointer
        */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeCursor(IntPtr cursor);

        /* Toggle whether or not the cursor is shown */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_ShowCursor(int toggle);

        public static uint SDL_BUTTON(uint X)
        {
            // If only there were a better way of doing this in C#
            return (uint)(1 << ((int)X - 1));
        }
    }
}