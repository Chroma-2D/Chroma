using System;
using System.Text.Json.Serialization;
using Chroma.Natives.SDL;

namespace Chroma.Natives.Boot.Config
{
    [Serializable]
    internal class SdlModuleConfig
    {
        [JsonPropertyName("audio")] public bool Audio { get; private set; } = true;
        [JsonPropertyName("video")] public bool Video { get; private set; } = true;
        [JsonPropertyName("timer")] public bool Timer { get; private set; } = true;
        [JsonPropertyName("events")] public bool Events { get; private set; } = true;
        [JsonPropertyName("haptic")] public bool Haptic { get; private set; } = true;
        [JsonPropertyName("sensor")] public bool Sensor { get; private set; } = true;
        [JsonPropertyName("joystick")] public bool Joystick { get; private set; } = true;
        [JsonPropertyName("controller")] public bool Controller { get; private set; } = true;

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