using Chroma.Natives.Bindings.SDL;

namespace Chroma.Hardware
{
    public enum BatteryStatus
    {
        Unknown = SDL2.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_UNKNOWN,
        Dead = SDL2.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_EMPTY,
        Low = SDL2.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_LOW,
        Medium = SDL2.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_MEDIUM,
        High = SDL2.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_FULL,
        Charging = SDL2.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_WIRED,
        FullyCharged = SDL2.SDL_JoystickPowerLevel.SDL_JOYSTICK_POWER_MAX
    }
}