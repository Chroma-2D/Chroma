using Chroma.SDL2;

namespace Chroma.Input
{
    public enum ControllerAxis
    {
        LeftStickX = SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX,
        LeftStickY = SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY,
        LeftTrigger = SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT,
        RightStickX = SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX,
        RightStickY = SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY,
        RightTrigger = SDL.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT
    }
}
