using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public enum ControllerAxis
    {
        LeftStickX = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX,
        LeftStickY = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY,
        LeftTrigger = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT,
        RightStickX = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX,
        RightStickY = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY,
        RightTrigger = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT
    }
}