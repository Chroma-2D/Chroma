using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
        public enum SDL_JoystickPowerLevel
        {
            SDL_JOYSTICK_POWER_UNKNOWN = -1,
            SDL_JOYSTICK_POWER_EMPTY,
            SDL_JOYSTICK_POWER_LOW,
            SDL_JOYSTICK_POWER_MEDIUM,
            SDL_JOYSTICK_POWER_FULL,
            SDL_JOYSTICK_POWER_WIRED,
            SDL_JOYSTICK_POWER_MAX
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Guid SDL_JoystickGetGUID(IntPtr joystick);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_JoystickInstanceID(IntPtr joystick);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_JoystickPowerLevel SDL_JoystickCurrentPowerLevel(
            IntPtr joystick
        );
    }
}