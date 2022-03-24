using System;
using System.Text.Json.Serialization;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Natives.Boot.Config
{
    [Serializable]
    internal class SdlModuleConfig
    {
        [JsonPropertyName("audio")] public bool Audio { get; set; } = true;
        [JsonPropertyName("video")] public bool Video { get; set; } = true;
        [JsonPropertyName("timer")] public bool Timer { get; set; } = true;
        [JsonPropertyName("events")] public bool Events { get; set; } = true;
        [JsonPropertyName("haptic")] public bool Haptic { get; set; } = true;
        [JsonPropertyName("sensor")] public bool Sensor { get; set; } = true;
        [JsonPropertyName("joystick")] public bool Joystick { get; set; } = true;
        [JsonPropertyName("controller")] public bool Controller { get; set; } = true;

        [JsonIgnore]
        public uint SdlInitFlags
        {
            get
            {
                var ret = 0u;

                if (Audio)
                    ret |= SDL2.SDL_INIT_AUDIO;

                if (Video)
                    ret |= SDL2.SDL_INIT_VIDEO;
                
                if (Timer)
                    ret |= SDL2.SDL_INIT_TIMER;

                if (Events)
                    ret |= SDL2.SDL_INIT_EVENTS;

                if (Joystick)
                    ret |= SDL2.SDL_INIT_JOYSTICK;

                if (Controller)
                    ret |= SDL2.SDL_INIT_GAMECONTROLLER;

                if (Haptic)
                    ret |= SDL2.SDL_INIT_HAPTIC;

                if (Sensor)
                    ret |= SDL2.SDL_INIT_SENSOR;

                return ret;
            }
        }
    }
}