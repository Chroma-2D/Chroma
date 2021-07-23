using System.Collections.Generic;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioManager
    {
        private List<string> _audioDrivers = new();
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public AudioInput Input { get; }
        public AudioOutput Output { get; }

        public IReadOnlyList<string> AudioDrivers => _audioDrivers;

        internal AudioManager()
        {
            Input = AudioInput.Instance;
            Output = AudioOutput.Instance;

            EnumerateAudioDrivers();
            EchoCurrentAudioDriver();
        }

        internal void Initialize()
        {
            Output.Initialize();
            Input.Initialize();
        }

        internal void EnumerateAudioDrivers()
        {
            var driverCount = SDL2.SDL_GetNumAudioDrivers();
            for (var i = 0; i < driverCount; i++)
            {
                _audioDrivers.Add(SDL2.SDL_GetAudioDriver(i));
            }
        }

        internal void EchoCurrentAudioDriver()
        {
            _log.Info($"Using driver '{SDL2.SDL_GetCurrentAudioDriver()}'.");
        }
    }
}