using Chroma.SDL2;

namespace Chroma.Hardware
{
    public enum BatteryStatus
    {
        Unknown = SDL.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_UNKNOWN,
        Dead = SDL.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_EMPTY,
        Low = SDL.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_LOW,
        Medium = SDL.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_MEDIUM,
        High = SDL.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_FULL,
        Charging = SDL.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_WIRED,
        FullyCharged = SDL.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_MAX
    }
}
