using System;
using System.Collections.Generic;
using System.Linq;
using Chroma.Audio.Sources;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioManager
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        private static AudioManager _instance;
        internal static AudioManager Instance => _instance ?? (_instance = new AudioManager());

        private List<AudioDevice> _devices = new();

        private bool _mixerInitialized;
        private bool _backendInitialized;
        private bool _playbackPaused;

        public IReadOnlyList<AudioDevice> Devices => _devices;

        public int Frequency { get; private set; }
        public int SampleCount { get; private set; }

        public event EventHandler<AudioDeviceEventArgs> DeviceConnected;
        public event EventHandler<AudioDeviceEventArgs> DeviceDisconnected;
        public event EventHandler<AudioSourceEventArgs> AudioSourceFinished;

        public float MasterVolume
        {
            get => SDL2_nmix.NMIX_GetMasterGain();
            set
            {
                var vol = value;

                if (vol < 0f)
                    vol = 0f;

                if (vol > 2f)
                    vol = 2f;

                SDL2_nmix.NMIX_SetMasterGain(vol);
            }
        }

        public AudioDevice CurrentOutputDevice => _devices.FirstOrDefault(
            x => x.Index == SDL2_nmix.NMIX_GetAudioDevice()
        );

        private AudioManager()
        {
            Initialize();
        }

        public void PauseAll()
        {
            _playbackPaused = !_playbackPaused;
            SDL2_nmix.NMIX_PausePlayback(_playbackPaused);
        }

        public void Open(AudioDevice device = null, int frequency = 44100, int sampleCount = 1024)
        {
            Close();

            Frequency = frequency;
            SampleCount = sampleCount;

            if (SDL2_sound.Sound_Init() < 0)
            {
                _log.Error($"Failed to initialize audio backend: {SDL2.SDL_GetError()}");
                return;
            }
            _backendInitialized = true;

            if (SDL2_nmix.NMIX_OpenAudio(device?.Name, Frequency, SampleCount) < 0)
            {
                _log.Error($"Failed to initialize audio mixer: {SDL2.SDL_GetError()}");
                return;
            }
            _mixerInitialized = true;
        }

        public void Close()
        {
            if (_mixerInitialized)
            {
                if (SDL2_nmix.NMIX_CloseAudio() < 0)
                {
                    _log.Error($"Failed to stop the audio mixer: {SDL2.SDL_GetError()}");
                    return;
                }
                _mixerInitialized = false;
            }

            if (_backendInitialized)
            {
                if (SDL2_sound.Sound_Quit() < 0)
                {
                    _log.Error($"Failed to stop the audio backend: {SDL2.SDL_GetError()}");
                    return;
                }
                _backendInitialized = false;
            }
        }

        public void EnumerateDevices()
        {
            _devices.Clear();

            var numberOfOutputDevices = SDL2.SDL_GetNumAudioDevices(0);
            var numberOfInputDevices = SDL2.SDL_GetNumAudioDevices(1);

            for (var i = 0; i < numberOfOutputDevices; i++)
                _devices.Add(new AudioDevice(i, false));

            for (var i = 0; i < numberOfInputDevices; i++)
                _devices.Add(new AudioDevice(i, true));
        }

        public void OnAudioSourceFinished(AudioSource s)
        {
            AudioSourceFinished?.Invoke(
                this,
                new AudioSourceEventArgs(s)
            );
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
        
        internal void Initialize()
        {
            EnumerateDevices();
            Open();
        }
    }
}