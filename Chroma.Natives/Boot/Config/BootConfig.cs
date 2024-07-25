namespace Chroma.Natives.Boot.Config;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Chroma.Natives.Bindings.SDL;

[Serializable]
internal class BootConfig
{
    [JsonPropertyName("natives_in_appdir")]
    public bool NativesInApplicationDirectory { get; set; } = true;

    [JsonPropertyName("skip_checksum_verification")]
    public bool SkipChecksumVerification { get; set; }

    [JsonPropertyName("enable_sdlgpu_debug")]
    public bool EnableSdlGpuDebugging { get; set; }

    [JsonPropertyName("hook_sdl_log")]
    public bool HookSdlLog { get; set; }

    [JsonPropertyName("sdl_modules")]
    public SdlModuleConfig SdlModules { get; set; } = new();

    [JsonPropertyName("sdl_initialization_hints")]
    public Dictionary<string, string> SdlInitializationHints { get; set; } = new()
    {
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_PS4, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_PS5, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_SWITCH, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_XBOX, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_LUNA, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_STADIA, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_STEAM, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_JOY_CONS, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_PS4_RUMBLE, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_PS5_RUMBLE, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_GAMECUBE, "1" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_SWITCH_HOME_LED, "0" },
        { SDL2.SDL_HINT_JOYSTICK_HIDAPI_PS5_PLAYER_LED, "0" }
    };
}