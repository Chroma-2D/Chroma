using Chroma.SDL2;

namespace Chroma.Input
{
    public enum ControllerButton
    {
        X = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X,
        Y = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y,
        A = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A,
        B = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B,
        View = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK,
        Menu = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START,
        Xbox = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE,
        LeftStick = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSTICK,
        RightStick = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSTICK,
        DpadLeft = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_LEFT,
        DpadRight = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT,
        DpadUp = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP,
        DpadDown = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_DOWN,
        RightBumper = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER,
        LeftBumper = SDL.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER
    }
}
