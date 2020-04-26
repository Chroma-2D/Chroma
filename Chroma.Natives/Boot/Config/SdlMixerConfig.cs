using Chroma.Natives.SDL;
using System;
using System.Text.Json.Serialization;

namespace Chroma.Natives.Boot.Config
{
    [Serializable]
    internal class SdlMixerConfig
    {
        [JsonPropertyName("flac")]
        public bool Flac { get; private set; } = false;

        [JsonPropertyName("midi")]
        public bool Midi { get; private set; } = false;

        [JsonPropertyName("mp3")]
        public bool Mp3 { get; private set; } = false;

        [JsonPropertyName("mod")]
        public bool Mod { get; private set; } = false;

        [JsonPropertyName("ogg")]
        public bool Ogg { get; private set; } = true;

        [JsonPropertyName("opus")]
        public bool Opus { get; private set; } = false;

        [JsonIgnore]
        public SDL_mixer.MIX_InitFlags SdlMixerFlags
        {
            get
            {
                SDL_mixer.MIX_InitFlags flags = 0u;

                if (Flac)
                    flags |= SDL_mixer.MIX_InitFlags.MIX_INIT_FLAC;

                if (Midi)
                    flags |= SDL_mixer.MIX_InitFlags.MIX_INIT_MID;

                if (Mp3)
                    flags |= SDL_mixer.MIX_InitFlags.MIX_INIT_MP3;

                if (Mod)
                    flags |= SDL_mixer.MIX_InitFlags.MIX_INIT_MOD;

                if (Ogg)
                    flags |= SDL_mixer.MIX_InitFlags.MIX_INIT_OGG;

                if (Opus)
                    flags |= SDL_mixer.MIX_InitFlags.MIX_INIT_OPUS;

                return flags;
            }
        }
    }
}
