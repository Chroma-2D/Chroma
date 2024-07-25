namespace Chroma.Input.GameControllers;

using Chroma.Natives.Bindings.SDL;

public enum ControllerAxis
{
    LeftStickX = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX,
    LeftStickY = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY,
    RightStickX = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX,
    RightStickY = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY,
    LeftTrigger = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT,
    RightTrigger = SDL2.SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT
}