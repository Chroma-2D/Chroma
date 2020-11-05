using System;
using System.Text.Json.Serialization;

namespace Chroma.Natives.Boot.Config
{
    [Serializable]
    internal class BootConfig
    {
        [JsonPropertyName("natives_in_appdir")]
        public bool NativesInApplicationDirectory { get; set; } = true;

        [JsonPropertyName("skip_checksum_verification")]
        public bool SkipChecksumVerification { get; set; }

        [JsonPropertyName("enable_sdlgpu_debug")]
        public bool EnableSdlGpuDebugging { get; set; }
        
        [JsonPropertyName("sdl_modules")]
        public SdlModuleConfig SdlModules { get; private set; } = new SdlModuleConfig();

        [JsonPropertyName("mixer_modules")]
        public SdlMixerConfig MixerModules { get; private set; } = new SdlMixerConfig();
    }
}