using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
		public enum SDL_GameControllerBindType
		{
			SDL_CONTROLLER_BINDTYPE_NONE,
			SDL_CONTROLLER_BINDTYPE_BUTTON,
			SDL_CONTROLLER_BINDTYPE_AXIS,
			SDL_CONTROLLER_BINDTYPE_HAT
		}

		public enum SDL_GameControllerAxis
		{
			SDL_CONTROLLER_AXIS_INVALID = -1,
			SDL_CONTROLLER_AXIS_LEFTX,
			SDL_CONTROLLER_AXIS_LEFTY,
			SDL_CONTROLLER_AXIS_RIGHTX,
			SDL_CONTROLLER_AXIS_RIGHTY,
			SDL_CONTROLLER_AXIS_TRIGGERLEFT,
			SDL_CONTROLLER_AXIS_TRIGGERRIGHT,
			SDL_CONTROLLER_AXIS_MAX
		}

		public enum SDL_GameControllerButton
		{
			SDL_CONTROLLER_BUTTON_INVALID = -1,
			SDL_CONTROLLER_BUTTON_A,
			SDL_CONTROLLER_BUTTON_B,
			SDL_CONTROLLER_BUTTON_X,
			SDL_CONTROLLER_BUTTON_Y,
			SDL_CONTROLLER_BUTTON_BACK,
			SDL_CONTROLLER_BUTTON_GUIDE,
			SDL_CONTROLLER_BUTTON_START,
			SDL_CONTROLLER_BUTTON_LEFTSTICK,
			SDL_CONTROLLER_BUTTON_RIGHTSTICK,
			SDL_CONTROLLER_BUTTON_LEFTSHOULDER,
			SDL_CONTROLLER_BUTTON_RIGHTSHOULDER,
			SDL_CONTROLLER_BUTTON_DPAD_UP,
			SDL_CONTROLLER_BUTTON_DPAD_DOWN,
			SDL_CONTROLLER_BUTTON_DPAD_LEFT,
			SDL_CONTROLLER_BUTTON_DPAD_RIGHT,
			SDL_CONTROLLER_BUTTON_MISC1,
			SDL_CONTROLLER_BUTTON_PADDLE1,
			SDL_CONTROLLER_BUTTON_PADDLE2,
			SDL_CONTROLLER_BUTTON_PADDLE3,
			SDL_CONTROLLER_BUTTON_PADDLE4,
			SDL_CONTROLLER_BUTTON_TOUCHPAD,
			SDL_CONTROLLER_BUTTON_MAX,
		}

		public enum SDL_GameControllerType
		{
			SDL_CONTROLLER_TYPE_UNKNOWN = 0,
			SDL_CONTROLLER_TYPE_XBOX360,
			SDL_CONTROLLER_TYPE_XBOXONE,
			SDL_CONTROLLER_TYPE_PS3,
			SDL_CONTROLLER_TYPE_PS4,
			SDL_CONTROLLER_TYPE_NINTENDO_SWITCH_PRO,
			SDL_CONTROLLER_TYPE_VIRTUAL,		/* Requires >= 2.0.14 */
			SDL_CONTROLLER_TYPE_PS5,		/* Requires >= 2.0.14 */
			SDL_CONTROLLER_TYPE_AMAZON_LUNA,	/* Requires >= 2.0.16 */
			SDL_CONTROLLER_TYPE_GOOGLE_STADIA	/* Requires >= 2.0.16 */
		}

		// FIXME: I'd rather this somehow be private...
		[StructLayout(LayoutKind.Sequential)]
		public struct INTERNAL_GameControllerButtonBind_hat
		{
			public int hat;
			public int hat_mask;
		}

		// FIXME: I'd rather this somehow be private...
		[StructLayout(LayoutKind.Explicit)]
		public struct INTERNAL_GameControllerButtonBind_union
		{
			[FieldOffset(0)]
			public int button;
			[FieldOffset(0)]
			public int axis;
			[FieldOffset(0)]
			public INTERNAL_GameControllerButtonBind_hat hat;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_GameControllerButtonBind
		{
			public SDL_GameControllerBindType bindType;
			public INTERNAL_GameControllerButtonBind_union value;
		}

		/* This exists to deal with C# being stupid about blittable types. */
		[StructLayout(LayoutKind.Sequential)]
		private struct INTERNAL_SDL_GameControllerButtonBind
		{
			public int bindType;
			/* Largest data type in the union is two ints in size */
			public int unionVal0;
			public int unionVal1;
		}

		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerAddMapping", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe int INTERNAL_SDL_GameControllerAddMapping(
			byte* mappingString
		);
		public static unsafe int SDL_GameControllerAddMapping(
			string mappingString
		) {
			byte* utf8MappingString = Utf8EncodeHeap(mappingString);
			int result = INTERNAL_SDL_GameControllerAddMapping(
				utf8MappingString
			);
			Marshal.FreeHGlobal((IntPtr) utf8MappingString);
			return result;
		}

		/* Only available in 2.0.6 or higher. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerNumMappings();

		/* Only available in 2.0.6 or higher. */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerMappingForIndex", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerMappingForIndex(int mapping_index);
		public static string SDL_GameControllerMappingForIndex(int mapping_index)
		{
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerMappingForIndex(
					mapping_index
				),
				true
			);
		}

		/* THIS IS AN RWops FUNCTION! */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerAddMappingsFromRW", CallingConvention = CallingConvention.Cdecl)]
		private static extern int INTERNAL_SDL_GameControllerAddMappingsFromRW(
			IntPtr rw,
			int freerw
		);
		public static int SDL_GameControllerAddMappingsFromFile(string file)
		{
			IntPtr rwops = SDL_RWFromFile(file, "rb");
			return INTERNAL_SDL_GameControllerAddMappingsFromRW(rwops, 1);
		}

		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerMappingForGUID", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerMappingForGUID(
			Guid guid
		);
		public static string SDL_GameControllerMappingForGUID(Guid guid)
		{
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerMappingForGUID(guid),
				true
			);
		}

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerMapping", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerMapping(
			IntPtr gamecontroller
		);
		public static string SDL_GameControllerMapping(
			IntPtr gamecontroller
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerMapping(
					gamecontroller
				),
				true
			);
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_IsGameController(int joystick_index);

		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerNameForIndex", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerNameForIndex(
			int joystick_index
		);
		public static string SDL_GameControllerNameForIndex(
			int joystick_index
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerNameForIndex(joystick_index)
			);
		}

		/* Only available in 2.0.9 or higher. */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerMappingForDeviceIndex", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerMappingForDeviceIndex(
			int joystick_index
		);
		public static string SDL_GameControllerMappingForDeviceIndex(
			int joystick_index
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerMappingForDeviceIndex(joystick_index),
				true
			);
		}

		/* IntPtr refers to an SDL_GameController* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GameControllerOpen(int joystick_index);

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerName", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerName(
			IntPtr gamecontroller
		);
		public static string SDL_GameControllerName(
			IntPtr gamecontroller
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerName(gamecontroller)
			);
		}

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.6 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern ushort SDL_GameControllerGetVendor(
			IntPtr gamecontroller
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.6 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern ushort SDL_GameControllerGetProduct(
			IntPtr gamecontroller
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.6 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern ushort SDL_GameControllerGetProductVersion(
			IntPtr gamecontroller
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerGetSerial", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerGetSerial(
			IntPtr gamecontroller
		);
		public static string SDL_GameControllerGetSerial(
			IntPtr gamecontroller
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerGetSerial(gamecontroller)
			);
		}

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GameControllerGetAttached(
			IntPtr gamecontroller
		);

		/* IntPtr refers to an SDL_Joystick*
	    * gamecontroller refers to an SDL_GameController*
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GameControllerGetJoystick(
			IntPtr gamecontroller
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerEventState(int state);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GameControllerUpdate();

		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerGetAxisFromString", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe SDL_GameControllerAxis INTERNAL_SDL_GameControllerGetAxisFromString(
			byte* pchString
		);
		public static unsafe SDL_GameControllerAxis SDL_GameControllerGetAxisFromString(
			string pchString
		) {
			int utf8PchStringBufSize = Utf8Size(pchString);
			byte* utf8PchString = stackalloc byte[utf8PchStringBufSize];
			return INTERNAL_SDL_GameControllerGetAxisFromString(
				Utf8Encode(pchString, utf8PchString, utf8PchStringBufSize)
			);
		}

		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerGetStringForAxis", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerGetStringForAxis(
			SDL_GameControllerAxis axis
		);
		public static string SDL_GameControllerGetStringForAxis(
			SDL_GameControllerAxis axis
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerGetStringForAxis(
					axis
				)
			);
		}

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerGetBindForAxis", CallingConvention = CallingConvention.Cdecl)]
		private static extern INTERNAL_SDL_GameControllerButtonBind INTERNAL_SDL_GameControllerGetBindForAxis(
			IntPtr gamecontroller,
			SDL_GameControllerAxis axis
		);
		public static SDL_GameControllerButtonBind SDL_GameControllerGetBindForAxis(
			IntPtr gamecontroller,
			SDL_GameControllerAxis axis
		) {
			// This is guaranteed to never be null
			INTERNAL_SDL_GameControllerButtonBind dumb = INTERNAL_SDL_GameControllerGetBindForAxis(
				gamecontroller,
				axis
			);
			
			SDL_GameControllerButtonBind result = new SDL_GameControllerButtonBind
			{
				bindType = (SDL_GameControllerBindType) dumb.bindType
			};
			
			result.value.hat.hat = dumb.unionVal0;
			result.value.hat.hat_mask = dumb.unionVal1;
			return result;
		}

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern short SDL_GameControllerGetAxis(
			IntPtr gamecontroller,
			SDL_GameControllerAxis axis
		);

		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerGetButtonFromString", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe SDL_GameControllerButton INTERNAL_SDL_GameControllerGetButtonFromString(
			byte* pchString
		);
		public static unsafe SDL_GameControllerButton SDL_GameControllerGetButtonFromString(
			string pchString
		) {
			int utf8PchStringBufSize = Utf8Size(pchString);
			byte* utf8PchString = stackalloc byte[utf8PchStringBufSize];
			return INTERNAL_SDL_GameControllerGetButtonFromString(
				Utf8Encode(pchString, utf8PchString, utf8PchStringBufSize)
			);
		}

		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerGetStringForButton", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GameControllerGetStringForButton(
			SDL_GameControllerButton button
		);
		public static string SDL_GameControllerGetStringForButton(
			SDL_GameControllerButton button
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GameControllerGetStringForButton(button)
			);
		}

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, EntryPoint = "SDL_GameControllerGetBindForButton", CallingConvention = CallingConvention.Cdecl)]
		private static extern INTERNAL_SDL_GameControllerButtonBind INTERNAL_SDL_GameControllerGetBindForButton(
			IntPtr gamecontroller,
			SDL_GameControllerButton button
		);
		public static SDL_GameControllerButtonBind SDL_GameControllerGetBindForButton(
			IntPtr gamecontroller,
			SDL_GameControllerButton button
		) {
			// This is guaranteed to never be null
			INTERNAL_SDL_GameControllerButtonBind dumb = INTERNAL_SDL_GameControllerGetBindForButton(
				gamecontroller,
				button
			);
			
			SDL_GameControllerButtonBind result = new SDL_GameControllerButtonBind
			{
				bindType = (SDL_GameControllerBindType) dumb.bindType
			};
			
			result.value.hat.hat = dumb.unionVal0;
			result.value.hat.hat_mask = dumb.unionVal1;
			return result;
		}

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte SDL_GameControllerGetButton(
			IntPtr gamecontroller,
			SDL_GameControllerButton button
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.9 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerRumble(
			IntPtr gamecontroller,
			UInt16 low_frequency_rumble,
			UInt16 high_frequency_rumble,
			UInt32 duration_ms
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerRumbleTriggers(
			IntPtr gamecontroller,
			UInt16 left_rumble,
			UInt16 right_rumble,
			UInt32 duration_ms
		);

		/* gamecontroller refers to an SDL_GameController* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GameControllerClose(
			IntPtr gamecontroller
		);

		/* int refers to an SDL_JoystickID, IntPtr to an SDL_GameController*.
	    * Only available in 2.0.4 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GameControllerFromInstanceID(int joyid);

		/* Only available in 2.0.11 or higher. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_GameControllerType SDL_GameControllerTypeForIndex(
			int joystick_index
		);

		/* IntPtr refers to an SDL_GameController*.
	    * Only available in 2.0.11 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_GameControllerType SDL_GameControllerGetType(
			IntPtr gamecontroller
		);

		/* IntPtr refers to an SDL_GameController*.
	    * Only available in 2.0.11 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_GameControllerFromPlayerIndex(
			int player_index
		);

		/* IntPtr refers to an SDL_GameController*.
	    * Only available in 2.0.11 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_GameControllerSetPlayerIndex(
			IntPtr gamecontroller,
			int player_index
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GameControllerHasLED(
			IntPtr gamecontroller
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerSetLED(
			IntPtr gamecontroller,
			byte red,
			byte green,
			byte blue
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GameControllerHasAxis(
			IntPtr gamecontroller,
			SDL_GameControllerAxis axis
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GameControllerHasButton(
			IntPtr gamecontroller,
			SDL_GameControllerButton button
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerGetNumTouchpads(
			IntPtr gamecontroller
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerGetNumTouchpadFingers(
			IntPtr gamecontroller,
			int touchpad
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerGetTouchpadFinger(
			IntPtr gamecontroller,
			int touchpad,
			int finger,
			out byte state,
			out float x,
			out float y,
			out float pressure
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GameControllerHasSensor(
			IntPtr gamecontroller,
			SDL_SensorType type
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerSetSensorEnabled(
			IntPtr gamecontroller,
			SDL_SensorType type,
			bool enabled
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.14 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool SDL_GameControllerIsSensorEnabled(
			IntPtr gamecontroller,
			SDL_SensorType type
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern unsafe int SDL_GameControllerGetSensorData(
			IntPtr gamecontroller,
			SDL_SensorType type,
			float* data,
			int num_values
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern float SDL_GameControllerGetSensorDataRate(
			IntPtr gamecontroller,
			SDL_SensorType type
		);

		/* gamecontroller refers to an SDL_GameController*.
	    * data refers to a const void*.
	    * Only available in 2.0.16 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GameControllerSendEffect(
			IntPtr gamecontroller,
			IntPtr data,
			int size
		);
		
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern unsafe int SDL_GameControllerSendEffect(
			IntPtr gamecontroller,
			void* data,
			int size
		);
    }
}