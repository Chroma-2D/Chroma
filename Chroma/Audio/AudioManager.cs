using System;
using System.Collections.Generic;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioManager
    {
        private List<string> _audioDrivers = new();
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public AudioInput Input => AudioInput.Instance;
        public AudioOutput Output => AudioOutput.Instance;

        public IReadOnlyList<string> AudioDrivers => _audioDrivers;
        
        public event EventHandler<AudioDeviceEventArgs> DeviceConnected;
        public event EventHandler<AudioDeviceEventArgs> DeviceDisconnected;

        internal AudioManager()
        {
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
        
        internal void OnDeviceAdded(uint index, bool isCapture)
        {
            DeviceConnected?.Invoke(
                this,
                new AudioDeviceEventArgs(
                    new AudioDevice((int)index, isCapture)
                )
            );
        }

        internal void OnDeviceRemoved(uint index, bool isCapture)
        {
            DeviceDisconnected?.Invoke(
                this,
                new AudioDeviceEventArgs(
                    new AudioDevice((int)index, isCapture)
                )
            );
        }
    }
}