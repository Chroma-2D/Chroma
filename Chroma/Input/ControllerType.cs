using Chroma.Natives.SDL;

namespace Chroma.Input
{
    public enum ControllerType
    {
        Xbox360 = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_XBOX360,
        XboxOne = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_XBOXONE,
        PlayStation3 = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_PS3,
        PlayStation4 = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_PS4,
        PlayStation5 = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_PS5,
        NintendoSwitch = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_NINTENDO_SWITCH_PRO,
        GoogleStadia = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_GOOGLE_STADIA,
        AmazonLuna = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_AMAZON_LUNA,
        Virtual = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_VIRTUAL,
        Unknown = SDL2.SDL_GameControllerType.SDL_CONTROLLER_TYPE_UNKNOWN
    }
}