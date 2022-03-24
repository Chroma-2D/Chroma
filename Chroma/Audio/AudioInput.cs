using System.Collections.Generic;
using System.Linq;
using Chroma.Audio.Captures;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Audio
{
    public class AudioInput
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        
        private readonly List<AudioDevice> _devices = new();
        private readonly List<AudioCapture> _activeCaptures = new();
        
        private static AudioInput _instance;
        internal static AudioInput Instance => _instance ??= new();

        public IReadOnlyList<AudioDevice> Devices => _devices;

        public AudioDevice DefaultDevice => 
            _devices.FirstOrDefault() ?? throw new AudioException("No audio input devices found.");

        private AudioInput()
        {
        }

        internal uint OpenDevice(
            AudioDevice device, 
            AudioFormat format, 
            ChannelMode channelMode, 
            int frequency,
            ushort bufferSize)
        {           
            var spec = new SDL2.SDL_AudioSpec
            {
                freq = frequency,
                channels = (byte)channelMode,
                format = format.SdlFormat,
                samples = bufferSize
            };

            uint deviceId;
            if ((deviceId = SDL2.SDL_OpenAudioDevice(device.Name, 1, ref spec, out _, 0)) == 0)
            {
                _log.Error($"Could not open audio device for recording: {SDL2.SDL_GetError()}");
                return 0;
            }

            device.Lock(deviceId);
            return deviceId;
        }

        internal void CloseDevice(uint deviceId, AudioDevice device)
        {
            SDL2.SDL_CloseAudioDevice(deviceId);
            device.Unlock();
        }

        internal void Initialize()
        {            
            if (SDL2.SDL_WasInit(SDL2.SDL_INIT_AUDIO) == 0)
            {
                if (SDL2.SDL_Init(SDL2.SDL_INIT_AUDIO) < 0)
                {
                    _log.Error($"Could not initialize SDL audio: {SDL2.SDL_GetError()}");
                    return;
                }
            }
            else
            {
                Close();
            }
            
            EnumerateDevices();
        }

        internal void Close()
        {
            for (var i = 0; i < _activeCaptures.Count; i++)
                _activeCaptures[i].Terminate();
            
            _activeCaptures.Clear();
        }

        internal void Track(AudioCapture capture)
        {
            _activeCaptures.Add(capture);
        }

        internal void Untrack(AudioCapture capture)
        {
            _activeCaptures.Remove(capture);
        }
        
        private void EnumerateDevices()
        {
            _devices.Clear();

            var numberOfInputDevices = SDL2.SDL_GetNumAudioDevices(1);

            for (var i = 0; i < numberOfInputDevices; i++)
                _devices.Add(new AudioDevice(i, true, SDL2.SDL_GetAudioDeviceName(i, true)));
        }
    }
}