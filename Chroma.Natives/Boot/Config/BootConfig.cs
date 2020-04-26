using System;
using System.Text.Json.Serialization;

namespace Chroma.Natives.Boot.Config
{
    [Serializable]
    internal class BootConfig
    {
        [JsonPropertyName("natives_in_appdir")]
        public bool NativesInApplicationDirectory { get; private set; } = true;

        [JsonPropertyName("sdl_modules")]
        public SdlModuleConfig SdlModules { get; private set; } = new SdlModuleConfig();

        [JsonPropertyName("mixer_modules")]
        public SdlMixerConfig MixerModules { get; private set; } = new SdlMixerConfig();
    }
}