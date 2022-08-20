using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Keysym
		{
			public SDL_Scancode scancode;
			public SDL_Keycode sym;
			public SDL_Keymod mod; /* UInt16 */
			public UInt32 unicode; /* Deprecated */
		}

		/* Get the window which has kbd focus */
		/* Return type is an SDL_Window pointer */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GetKeyboardFocus();

		/* Get a snapshot of the keyboard state. */
		/* Return value is a pointer to a UInt8 array */
		/* Numkeys returns the size of the array if non-null */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GetKeyboardState(out int numkeys);

		/* Get the current key modifier state for the keyboard. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_Keymod SDL_GetModState();

		/* Set the current key modifier state */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetModState(SDL_Keymod modstate);

		/* Get the key code corresponding to the given scancode
	    * with the current keyboard layout.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_Keycode SDL_GetKeyFromScancode(SDL_Scancode scancode);

		/* Get the scancode for the given keycode */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode key);

		/* Wrapper for SDL_GetScancodeName */
		[DllImport(LibraryName, EntryPoint = "SDL_GetScancodeName", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GetScancodeName(SDL_Scancode scancode);
		public static string SDL_GetScancodeName(SDL_Scancode scancode)
		{
			return UTF8_ToManaged(
				INTERNAL_SDL_GetScancodeName(scancode)
			);
		}

		/* Get a scancode from a human-readable name */
		[DllImport(LibraryName, EntryPoint = "SDL_GetScancodeFromName", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe SDL_Scancode INTERNAL_SDL_GetScancodeFromName(
			byte* name
		);
		public static unsafe SDL_Scancode SDL_GetScancodeFromName(string name)
		{
			int utf8NameBufSize = Utf8Size(name);
			byte* utf8Name = stackalloc byte[utf8NameBufSize];
			return INTERNAL_SDL_GetScancodeFromName(
				Utf8Encode(name, utf8Name, utf8NameBufSize)
			);
		}

		/* Wrapper for SDL_GetKeyName */
		[DllImport(LibraryName, EntryPoint = "SDL_GetKeyName", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GetKeyName(SDL_Keycode key);
		public static string SDL_GetKeyName(SDL_Keycode key)
		{
			return UTF8_ToManaged(INTERNAL_SDL_GetKeyName(key));
		}

		/* Get a key code from a human-readable name */
		[DllImport(LibraryName, EntryPoint = "SDL_GetKeyFromName", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe SDL_Keycode INTERNAL_SDL_GetKeyFromName(
			byte* name
		);
		public static unsafe SDL_Keycode SDL_GetKeyFromName(string name)
		{
			int utf8NameBufSize = Utf8Size(name);
			byte* utf8Name = stackalloc byte[utf8NameBufSize];
			return INTERNAL_SDL_GetKeyFromName(
				Utf8Encode(name, utf8Name, utf8NameBufSize)
			);
		}

		/* Start accepting Unicode text input events, show keyboard */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_StartTextInput();

		/* Check if unicode input events are enabled */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_IsTextInputActive();

		/* Stop receiving any text input events, hide onscreen kbd */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_StopTextInput();

		/* Set the rectangle used for text input, hint for IME */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetTextInputRect(ref SDL_Rect rect);

		/* Does the platform support an on-screen keyboard? */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_HasScreenKeyboardSupport();

		/* Is the on-screen keyboard shown for a given window? */
		/* window is an SDL_Window pointer */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_IsScreenKeyboardShown(IntPtr window);
    }
}