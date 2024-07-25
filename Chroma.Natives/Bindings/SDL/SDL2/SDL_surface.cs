namespace Chroma.Natives.Bindings.SDL;

using System;
using System.Runtime.InteropServices;

internal static partial class SDL2
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SDL_Surface
	{
		public uint flags;
		public IntPtr format; // SDL_PixelFormat*
		public int w;
		public int h;
		public int pitch;
		public IntPtr pixels; // void*
		public IntPtr userdata; // void*
		public int locked;
		public IntPtr list_blitmap; // void*
		public SDL_Rect clip_rect;
		public IntPtr map; // SDL_BlitMap*
		public int refcount;
	}

	/* IntPtr refers to an SDL_Surface*
	 * src refers to an SDL_Surface*
	 * fmt refers to an SDL_PixelFormat*
	 */
	[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr SDL_ConvertSurface(
		IntPtr src,
		IntPtr fmt,
		uint flags
	);

	/* IntPtr refers to an SDL_Surface*, src to an SDL_Surface* */
	[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr SDL_ConvertSurfaceFormat(
		IntPtr src,
		uint pixel_format,
		uint flags
	);

	/* IntPtr refers to an SDL_Surface*, pixels to a void* */
	[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr SDL_CreateRGBSurfaceFrom(
		IntPtr pixels,
		int width,
		int height,
		int depth,
		int pitch,
		uint Rmask,
		uint Gmask,
		uint Bmask,
		uint Amask
	);

	/* surface refers to an SDL_Surface* */
	[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void SDL_FreeSurface(IntPtr surface);

	/* surface refers to an SDL_Surface* */
	[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
	public static extern int SDL_LockSurface(IntPtr surface);

	/* These are for SDL_SaveBMP, which is a macro in the SDL headers. */
	/* IntPtr refers to an SDL_Surface* */
	/* THIS IS AN RWops FUNCTION! */
	[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
	public static extern int SDL_SaveBMP_RW(
		IntPtr surface,
		IntPtr src,
		bool freesrc
	);

	/* surface refers to an SDL_Surface* */
	[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void SDL_UnlockSurface(IntPtr surface);
}