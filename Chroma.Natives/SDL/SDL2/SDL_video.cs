using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL
{
    internal static partial class SDL2
    {
		public enum SDL_GLattr
		{
			SDL_GL_RED_SIZE,
			SDL_GL_GREEN_SIZE,
			SDL_GL_BLUE_SIZE,
			SDL_GL_ALPHA_SIZE,
			SDL_GL_BUFFER_SIZE,
			SDL_GL_DOUBLEBUFFER,
			SDL_GL_DEPTH_SIZE,
			SDL_GL_STENCIL_SIZE,
			SDL_GL_ACCUM_RED_SIZE,
			SDL_GL_ACCUM_GREEN_SIZE,
			SDL_GL_ACCUM_BLUE_SIZE,
			SDL_GL_ACCUM_ALPHA_SIZE,
			SDL_GL_STEREO,
			SDL_GL_MULTISAMPLEBUFFERS,
			SDL_GL_MULTISAMPLESAMPLES,
			SDL_GL_ACCELERATED_VISUAL,
			SDL_GL_RETAINED_BACKING,
			SDL_GL_CONTEXT_MAJOR_VERSION,
			SDL_GL_CONTEXT_MINOR_VERSION,
			SDL_GL_CONTEXT_EGL,
			SDL_GL_CONTEXT_FLAGS,
			SDL_GL_CONTEXT_PROFILE_MASK,
			SDL_GL_SHARE_WITH_CURRENT_CONTEXT,
			SDL_GL_FRAMEBUFFER_SRGB_CAPABLE,
			SDL_GL_CONTEXT_RELEASE_BEHAVIOR,
			SDL_GL_CONTEXT_RESET_NOTIFICATION,	/* Requires >= 2.0.6 */
			SDL_GL_CONTEXT_NO_ERROR,		/* Requires >= 2.0.6 */
		}

		[Flags]
		public enum SDL_GLprofile
		{
			SDL_GL_CONTEXT_PROFILE_CORE				= 0x0001,
			SDL_GL_CONTEXT_PROFILE_COMPATIBILITY	= 0x0002,
			SDL_GL_CONTEXT_PROFILE_ES				= 0x0004
		}

		public enum SDL_WindowEventID : byte
		{
			SDL_WINDOWEVENT_NONE,
			SDL_WINDOWEVENT_SHOWN,
			SDL_WINDOWEVENT_HIDDEN,
			SDL_WINDOWEVENT_EXPOSED,
			SDL_WINDOWEVENT_MOVED,
			SDL_WINDOWEVENT_RESIZED,
			SDL_WINDOWEVENT_SIZE_CHANGED,
			SDL_WINDOWEVENT_MINIMIZED,
			SDL_WINDOWEVENT_MAXIMIZED,
			SDL_WINDOWEVENT_RESTORED,
			SDL_WINDOWEVENT_ENTER,
			SDL_WINDOWEVENT_LEAVE,
			SDL_WINDOWEVENT_FOCUS_GAINED,
			SDL_WINDOWEVENT_FOCUS_LOST,
			SDL_WINDOWEVENT_CLOSE,
			/* Only available in 2.0.5 or higher. */
			SDL_WINDOWEVENT_TAKE_FOCUS,
			SDL_WINDOWEVENT_HIT_TEST,
			/* Only available in 2.0.18 or higher. */
			SDL_WINDOWEVENT_ICCPROF_CHANGED,
			SDL_WINDOWEVENT_DISPLAY_CHANGED
		}

		public enum SDL_DisplayEventID : byte
		{
			SDL_DISPLAYEVENT_NONE,
			SDL_DISPLAYEVENT_ORIENTATION,
			SDL_DISPLAYEVENT_CONNECTED,	/* Requires >= 2.0.14 */
			SDL_DISPLAYEVENT_DISCONNECTED	/* Requires >= 2.0.14 */
		}

		public enum SDL_DisplayOrientation
		{
			SDL_ORIENTATION_UNKNOWN,
			SDL_ORIENTATION_LANDSCAPE,
			SDL_ORIENTATION_LANDSCAPE_FLIPPED,
			SDL_ORIENTATION_PORTRAIT,
			SDL_ORIENTATION_PORTRAIT_FLIPPED
		}

		/* Only available in 2.0.16 or higher. */
		public enum SDL_FlashOperation
		{
			SDL_FLASH_CANCEL,
			SDL_FLASH_BRIEFLY,
			SDL_FLASH_UNTIL_FOCUSED
		}

		[Flags]
		public enum SDL_WindowFlags : uint
		{
			SDL_WINDOW_FULLSCREEN =		0x00000001,
			SDL_WINDOW_OPENGL =		0x00000002,
			SDL_WINDOW_SHOWN =		0x00000004,
			SDL_WINDOW_HIDDEN =		0x00000008,
			SDL_WINDOW_BORDERLESS =		0x00000010,
			SDL_WINDOW_RESIZABLE =		0x00000020,
			SDL_WINDOW_MINIMIZED =		0x00000040,
			SDL_WINDOW_MAXIMIZED =		0x00000080,
			SDL_WINDOW_MOUSE_GRABBED =	0x00000100,
			SDL_WINDOW_INPUT_FOCUS =	0x00000200,
			SDL_WINDOW_MOUSE_FOCUS =	0x00000400,
			SDL_WINDOW_FULLSCREEN_DESKTOP =
				(SDL_WINDOW_FULLSCREEN | 0x00001000),
			SDL_WINDOW_FOREIGN =		0x00000800,
			SDL_WINDOW_ALLOW_HIGHDPI =	0x00002000,	/* Requires >= 2.0.1 */
			SDL_WINDOW_MOUSE_CAPTURE =	0x00004000,	/* Requires >= 2.0.4 */
			SDL_WINDOW_ALWAYS_ON_TOP =	0x00008000,	/* Requires >= 2.0.5 */
			SDL_WINDOW_SKIP_TASKBAR =	0x00010000,	/* Requires >= 2.0.5 */
			SDL_WINDOW_UTILITY =		0x00020000,	/* Requires >= 2.0.5 */
			SDL_WINDOW_TOOLTIP =		0x00040000,	/* Requires >= 2.0.5 */
			SDL_WINDOW_POPUP_MENU =		0x00080000,	/* Requires >= 2.0.5 */
			SDL_WINDOW_KEYBOARD_GRABBED =	0x00100000,	/* Requires >= 2.0.16 */
			SDL_WINDOW_VULKAN =		0x10000000,	/* Requires >= 2.0.6 */
			SDL_WINDOW_METAL =		0x2000000,	/* Requires >= 2.0.14 */

			SDL_WINDOW_INPUT_GRABBED =
				SDL_WINDOW_MOUSE_GRABBED,
		}

		/* Only available in 2.0.4 or higher. */
		public enum SDL_HitTestResult
		{
			SDL_HITTEST_NORMAL,		/* Region is normal. No special properties. */
			SDL_HITTEST_DRAGGABLE,		/* Region can drag entire window. */
			SDL_HITTEST_RESIZE_TOPLEFT,
			SDL_HITTEST_RESIZE_TOP,
			SDL_HITTEST_RESIZE_TOPRIGHT,
			SDL_HITTEST_RESIZE_RIGHT,
			SDL_HITTEST_RESIZE_BOTTOMRIGHT,
			SDL_HITTEST_RESIZE_BOTTOM,
			SDL_HITTEST_RESIZE_BOTTOMLEFT,
			SDL_HITTEST_RESIZE_LEFT
		}

		public const int SDL_WINDOWPOS_UNDEFINED_MASK =	0x1FFF0000;
		public const int SDL_WINDOWPOS_CENTERED_MASK =	0x2FFF0000;
		public const int SDL_WINDOWPOS_UNDEFINED =	0x1FFF0000;
		public const int SDL_WINDOWPOS_CENTERED =	0x2FFF0000;

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_DisplayMode
		{
			public uint format;
			public int w;
			public int h;
			public int refresh_rate;
			public IntPtr driverdata; // void*
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public unsafe struct SDL_Window
		{
			public void* magic; // const void*
			public uint id;
			public IntPtr title;
			public SDL_Surface* icon; // SDL_Surface*
			public int x, y;
			public int w, h;
			public int min_w, min_h;
			public int max_w, max_h;
			public uint flags;
			public uint last_fullscren_flags;
			
			public SDL_Rect windowed;

			public SDL_DisplayMode fullscreen_mode;

			public float brightness;
			public ushort* gamma; // ushort*
			public ushort* saved_gama; // ushort*

			public SDL_Surface* surface; // SDL_Surface*
			public bool surface_valid;
			
			public bool is_hiding;
			public bool is_destroying;

			public IntPtr shaper; // SDL_WindowShaper*
			
			public void* hit_test;
			public void* hit_test_data; // void*

			public void* data; // SDL_WindowUserData*
			
			public void* driverdata; // void*

			public SDL_Window* prev; // SDL_Window*
			public SDL_Window* next; // SDL_Window*
		}

		/* win refers to an SDL_Window*, area to a const SDL_Point*, data to a void*.
	    * Only available in 2.0.4 or higher.
	    */
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate SDL_HitTestResult SDL_HitTest(IntPtr win, IntPtr area, IntPtr data);

		/* IntPtr refers to an SDL_Window* */
		[DllImport(LibraryName, EntryPoint = "SDL_CreateWindow", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe IntPtr INTERNAL_SDL_CreateWindow(
			byte* title,
			int x,
			int y,
			int w,
			int h,
			SDL_WindowFlags flags
		);
		public static unsafe IntPtr SDL_CreateWindow(
			string title,
			int x,
			int y,
			int w,
			int h,
			SDL_WindowFlags flags
		) {
			int utf8TitleBufSize = Utf8Size(title);
			byte* utf8Title = stackalloc byte[utf8TitleBufSize];
			return INTERNAL_SDL_CreateWindow(
				Utf8Encode(title, utf8Title, utf8TitleBufSize),
				x, y, w, h,
				flags
			);
		}
		
		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_DestroyWindow(IntPtr window);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_DisableScreenSaver();

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_EnableScreenSaver();

		/* IntPtr refers to an SDL_DisplayMode. Just use closest. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GetClosestDisplayMode(
			int displayIndex,
			ref SDL_DisplayMode mode,
			out SDL_DisplayMode closest
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetCurrentDisplayMode(
			int displayIndex,
			out SDL_DisplayMode mode
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetDesktopDisplayMode(
			int displayIndex,
			out SDL_DisplayMode mode
		);

		[DllImport(LibraryName, EntryPoint = "SDL_GetDisplayName", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GetDisplayName(int index);
		public static string SDL_GetDisplayName(int index)
		{
			return UTF8_ToManaged(INTERNAL_SDL_GetDisplayName(index));
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetDisplayBounds(
			int displayIndex,
			out SDL_Rect rect
		);

		/* Only available in 2.0.4 or higher. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetDisplayDPI(
			int displayIndex,
			out float ddpi,
			out float hdpi,
			out float vdpi
		);

		/* Only available in 2.0.9 or higher. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_DisplayOrientation SDL_GetDisplayOrientation(
			int displayIndex
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetDisplayMode(
			int displayIndex,
			int modeIndex,
			out SDL_DisplayMode mode
		);

		/* Only available in 2.0.5 or higher. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetDisplayUsableBounds(
			int displayIndex,
			out SDL_Rect rect
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetNumDisplayModes(
			int displayIndex
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetNumVideoDisplays();

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float SDL_GetWindowBrightness(
			IntPtr window
		);

		/* modal_window and parent_window refer to an SDL_Window*s
	    * Only available in 2.0.5 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowModalFor(
			IntPtr modal_window,
			IntPtr parent_window
		);

		/* window refers to an SDL_Window*
	    * Only available in 2.0.5 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowInputFocus(IntPtr window);

		/* window refers to an SDL_Window*, IntPtr to a void* */
		[DllImport(LibraryName, EntryPoint = "SDL_GetWindowData", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe IntPtr INTERNAL_SDL_GetWindowData(
			IntPtr window,
			byte* name
		);
		public static unsafe IntPtr SDL_GetWindowData(
			IntPtr window,
			string name
		) {
			int utf8NameBufSize = Utf8Size(name);
			byte* utf8Name = stackalloc byte[utf8NameBufSize];
			return INTERNAL_SDL_GetWindowData(
				window,
				Utf8Encode(name, utf8Name, utf8NameBufSize)
			);
		}

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetWindowDisplayIndex(
			IntPtr window
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetWindowDisplayMode(
			IntPtr window,
			out SDL_DisplayMode mode
		);
		
		/* IntPtr refers to a void*
		 * window refers to an SDL_Window*
		 * mode refers to a size_t*
		 * Only available in 2.0.18 or higher.
		 */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GetWindowICCProfile(
			IntPtr window,
			out IntPtr mode
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_WindowFlags SDL_GetWindowFlags(IntPtr window);

		/* IntPtr refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GetWindowFromID(uint id);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetWindowGammaRamp(
			IntPtr window,
			[Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
				ushort[] red,
			[Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
				ushort[] green,
			[Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
				ushort[] blue
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GetWindowGrab(IntPtr window);

		/* window refers to an SDL_Window*
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GetWindowKeyboardGrab(IntPtr window);

		/* window refers to an SDL_Window*
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GetWindowMouseGrab(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint SDL_GetWindowID(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint SDL_GetWindowPixelFormat(
			IntPtr window
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GetWindowMaximumSize(
			IntPtr window,
			out int max_w,
			out int max_h
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GetWindowMinimumSize(
			IntPtr window,
			out int min_w,
			out int min_h
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GetWindowPosition(
			IntPtr window,
			out int x,
			out int y
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GetWindowSize(
			IntPtr window,
			out int w,
			out int h
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, EntryPoint = "SDL_GetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GetWindowTitle(
			IntPtr window
		);
		public static string SDL_GetWindowTitle(IntPtr window)
		{
			return UTF8_ToManaged(
				INTERNAL_SDL_GetWindowTitle(window)
			);
		}

		/* IntPtr and window refer to an SDL_GLContext and SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GL_CreateContext(IntPtr window);

		/* context refers to an SDL_GLContext */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GL_DeleteContext(IntPtr context);

		/* IntPtr refers to a function pointer, proc to a const char* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GL_GetProcAddress(IntPtr proc);

		/* IntPtr refers to a function pointer */
		public static unsafe IntPtr SDL_GL_GetProcAddress(string proc)
		{
			int utf8ProcBufSize = Utf8Size(proc);
			byte* utf8Proc = stackalloc byte[utf8ProcBufSize];
			return SDL_GL_GetProcAddress(
				(IntPtr) Utf8Encode(proc, utf8Proc, utf8ProcBufSize)
			);
		}

		[DllImport(LibraryName, EntryPoint = "SDL_GL_ExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe bool INTERNAL_SDL_GL_ExtensionSupported(
			byte* extension
		);
		public static unsafe bool SDL_GL_ExtensionSupported(string extension)
		{
			int utf8ExtensionBufSize = Utf8Size(extension);
			byte* utf8Extension = stackalloc byte[utf8ExtensionBufSize];
			return INTERNAL_SDL_GL_ExtensionSupported(
				Utf8Encode(extension, utf8Extension, utf8ExtensionBufSize)
			);
		}

		/* Only available in SDL 2.0.2 or higher. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GL_ResetAttributes();

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GL_GetAttribute(
			SDL_GLattr attr,
			out int value
		);

		/* window and context refer to an SDL_Window* and SDL_GLContext */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GL_MakeCurrent(
			IntPtr window,
			IntPtr context
		);

		/* IntPtr refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GL_GetCurrentWindow();

		/* IntPtr refers to an SDL_Context */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GL_GetCurrentContext();

		/* window refers to an SDL_Window*.
	    * Only available in SDL 2.0.1 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GL_GetDrawableSize(
			IntPtr window,
			out int w,
			out int h
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GL_SetAttribute(
			SDL_GLattr attr,
			int value
		);

		public static int SDL_GL_SetAttribute(
			SDL_GLattr attr,
			SDL_GLprofile profile
		) {
			return SDL_GL_SetAttribute(attr, (int)profile);
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GL_SetSwapInterval(int interval);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_HideWindow(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_MaximizeWindow(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_MinimizeWindow(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_RaiseWindow(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_RestoreWindow(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowBrightness(
			IntPtr window,
			float brightness
		);

		/* IntPtr and userdata are void*, window is an SDL_Window* */
		[DllImport(LibraryName, EntryPoint = "SDL_SetWindowData", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe IntPtr INTERNAL_SDL_SetWindowData(
			IntPtr window,
			byte* name,
			IntPtr userdata
		);
		public static unsafe IntPtr SDL_SetWindowData(
			IntPtr window,
			string name,
			IntPtr userdata
		) {
			int utf8NameBufSize = Utf8Size(name);
			byte* utf8Name = stackalloc byte[utf8NameBufSize];
			return INTERNAL_SDL_SetWindowData(
				window,
				Utf8Encode(name, utf8Name, utf8NameBufSize),
				userdata
			);
		}

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowDisplayMode(
			IntPtr window,
			ref SDL_DisplayMode mode
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowFullscreen(
			IntPtr window,
			uint flags
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowGammaRamp(
			IntPtr window,
			[In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
				ushort[] red,
			[In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
				ushort[] green,
			[In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeConst = 256)]
				ushort[] blue
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowGrab(
			IntPtr window,
			bool grabbed
		);

		/* window refers to an SDL_Window*
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowKeyboardGrab(
			IntPtr window,
			bool grabbed
		);

		/* window refers to an SDL_Window*
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowMouseGrab(
			IntPtr window,
			bool grabbed
		);


		/* window refers to an SDL_Window*, icon to an SDL_Surface* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowIcon(
			IntPtr window,
			IntPtr icon
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowMaximumSize(
			IntPtr window,
			int max_w,
			int max_h
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowMinimumSize(
			IntPtr window,
			int min_w,
			int min_h
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowPosition(
			IntPtr window,
			int x,
			int y
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowSize(
			IntPtr window,
			int w,
			int h
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowBordered(
			IntPtr window,
			bool bordered
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetWindowBordersSize(
			IntPtr window,
			out int top,
			out int left,
			out int bottom,
			out int right
		);

		/* window refers to an SDL_Window*
	    * Only available in 2.0.5 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowResizable(
			IntPtr window,
			bool resizable
		);

		/* window refers to an SDL_Window*
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_SetWindowAlwaysOnTop(
			IntPtr window,
			bool on_top
		);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, EntryPoint = "SDL_SetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe void INTERNAL_SDL_SetWindowTitle(
			IntPtr window,
			byte* title
		);
		public static unsafe void SDL_SetWindowTitle(
			IntPtr window,
			string title
		) {
			int utf8TitleBufSize = Utf8Size(title);
			byte* utf8Title = stackalloc byte[utf8TitleBufSize];
			INTERNAL_SDL_SetWindowTitle(
				window,
				Utf8Encode(title, utf8Title, utf8TitleBufSize)
			);
		}

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_ShowWindow(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_UpdateWindowSurface(IntPtr window);

		/* window refers to an SDL_Window* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_UpdateWindowSurfaceRects(
			IntPtr window,
			[In] SDL_Rect[] rects,
			int numrects
		);

		[DllImport(LibraryName, EntryPoint = "SDL_VideoInit", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe int INTERNAL_SDL_VideoInit(
			byte* driver_name
		);
		public static unsafe int SDL_VideoInit(string driver_name)
		{
			int utf8DriverNameBufSize = Utf8Size(driver_name);
			byte* utf8DriverName = stackalloc byte[utf8DriverNameBufSize];
			return INTERNAL_SDL_VideoInit(
				Utf8Encode(driver_name, utf8DriverName, utf8DriverNameBufSize)
			);
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_VideoQuit();

		/* window refers to an SDL_Window*, callback_data to a void*
	    * Only available in 2.0.4 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowHitTest(
			IntPtr window,
			SDL_HitTest callback,
			IntPtr callback_data
		);

		/* IntPtr refers to an SDL_Window*
	    * Only available in 2.0.4 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GetGrabbedWindow();

		/* window refers to an SDL_Window*
		 * Only available in 2.0.18 or higher.
		 */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowMouseRect(
			IntPtr window,
			ref SDL_Rect rect
		);
		
		/* window refers to an SDL_Window*
		 * rect refers to an SDL_Rect*
		 * This overload allows for IntPtr.Zero (null) to be passed for rect.
		 * Only available in 2.0.18 or higher.
		 */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_SetWindowMouseRect(
			IntPtr window,
			IntPtr rect
		);
		
		/* window refers to an SDL_Window*
		 * IntPtr refers to an SDL_Rect*
		 * Only available in 2.0.18 or higher.
		 */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GetWindowMouseRect(
			IntPtr window
		);
		
		/* window refers to an SDL_Window*
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_FlashWindow(
			IntPtr window,
			SDL_FlashOperation operation
		);
    }
}