using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
        public const uint SDL_TOUCH_MOUSEID = uint.MaxValue;

        public struct SDL_Finger
        {
            public long id; // SDL_FingerID
            public float x;
            public float y;
            public float pressure;
        }

        /* Only available in 2.0.10 or higher. */
        public enum SDL_TouchDeviceType
        {
            SDL_TOUCH_DEVICE_INVALID = -1,
            SDL_TOUCH_DEVICE_DIRECT,            /* touch screen with window-relative coordinates */
            SDL_TOUCH_DEVICE_INDIRECT_ABSOLUTE, /* trackpad with absolute device coordinates */
            SDL_TOUCH_DEVICE_INDIRECT_RELATIVE  /* trackpad with screen cursor-relative coordinates */
        }

        /**
	    *  \brief Get the number of registered touch devices.
 	    */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumTouchDevices();

        /**
	    *  \brief Get the touch ID with the given index, or 0 if the index is invalid.
	    */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_GetTouchDevice(int index);

        /**
	    *  \brief Get the number of active fingers for a given touch device.
	    */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetNumTouchFingers(long touchID);

        /**
	    *  \brief Get the finger object of the given touch, with the given index.
	    *  Returns pointer to SDL_Finger.
	    */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetTouchFinger(long touchID, int index);

        /* Only available in 2.0.10 or higher. */
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_TouchDeviceType SDL_GetTouchDeviceType(Int64 touchID);
    }
}