using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public enum ControllerButton
    {
        X = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_X,
        Y = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_Y,
        A = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_A,
        B = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_B,
        View = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_BACK,
        Menu = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_START,
        Logo = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_GUIDE,
        LeftStick = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSTICK,
        RightStick = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSTICK,
        DpadLeft = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_LEFT,
        DpadRight = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_RIGHT,
        DpadUp = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_UP,
        DpadDown = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_DPAD_DOWN,
        RightBumper = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_RIGHTSHOULDER,
        LeftBumper = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_LEFTSHOULDER,
        Touchpad = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_TOUCHPAD,
        RightTopPaddle = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE1,
        RightBottomPaddle = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE2,
        LeftTopPaddle = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE3,
        LeftBottomPaddle = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_PADDLE4,
        Special = SDL2.SDL_GameControllerButton.SDL_CONTROLLER_BUTTON_MISC1
    }
}
