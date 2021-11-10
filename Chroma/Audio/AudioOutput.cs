using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Chroma.Audio.Sources;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioOutput
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        
        private readonly List<AudioDevice> _devices = new();
        private readonly List<Decoder> _decoders = new();
        
        private static AudioOutput _instance;
        internal static AudioOutput Instance => _instance ??= new();

        private bool _mixerInitialized;
        private bool _backendInitialized;
        private bool _playbackPaused;

        public IReadOnlyList<AudioDevice> Devices => _devices;
        public IReadOnlyList<Decoder> Decoders => _decoders;

        public int Frequency { get; private set; }
        public int SampleCount { get; private set; }

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
            x => x.OpenIndex == SDL2_nmix.NMIX_GetAudioDevice()
        );

        private AudioOutput()
        {
        }

        public void PauseAll()
        {
            _playbackPaused = !_playbackPaused;
            SDL2_nmix.NMIX_PausePlayback(_playbackPaused);
        }

        public void Open(AudioDevice device = null, int frequency = 44100, int sampleCount = 1024)
        {
            Close();

            if (SDL2.SDL_WasInit(SDL2.SDL_INIT_AUDIO) == 0)
            {
                if (SDL2.SDL_AudioInit(null) < 0)
                {
                    _log.Error($"Failed to initialize SDL audio sub-system: {SDL2.SDL_GetError()}");
                    return;
                }
            }

            EnumerateDevices();

            Frequency = frequency;
            SampleCount = sampleCount;

            if (SDL2_sound.Sound_Init() < 0)
            {
                _log.Error($"Failed to initialize audio backend: {SDL2.SDL_GetError()}");
                return;
            }

            _backendInitialized = true;
            EnumerateDecoders();

            if (SDL2_nmix.NMIX_OpenAudio(device?.Name, Frequency, SampleCount) < 0)
            {
                _log.Error($"Failed to initialize audio mixer: {SDL2.SDL_GetError()}");
                return;
            }

            _mixerInitialized = true;

            device ??= AudioDevice.DefaultOutput;
            device.Lock(SDL2_nmix.NMIX_GetAudioDevice());
        }

        public void Close()
        {
            var device = SDL2_nmix.NMIX_GetAudioDevice();

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

            if (device > 0)
            {
                _devices.First(x => x.OpenIndex == device).Unlock();
            }

            _devices.Clear();
        }

        public void OnAudioSourceFinished(AudioSource audioSource, bool isLooping)
        {
            if (!_backendInitialized || !_mixerInitialized)
                return;

            AudioSourceFinished?.Invoke(
                this,
                new AudioSourceEventArgs(audioSource, isLooping)
            );
        }

        internal void Initialize()
        {
            Open();
        }

        private void EnumerateDevices()
        {
            _devices.Clear();

            var numberOfOutputDevices = SDL2.SDL_GetNumAudioDevices(0);
            for (var i = 0; i < numberOfOutputDevices; i++)
            {
                _devices.Add(new AudioDevice(i, false, SDL2.SDL_GetAudioDeviceName(i, false)));
            }
            
            _devices.Add(AudioDevice.DefaultOutput);
        }

        private void EnumerateDecoders()
        {
            _decoders.Clear();

            unsafe
            {
                var decoderList = (SDL2_sound.Sound_DecoderInfo**)SDL2_sound.Sound_AvailableDecoders();

                if (decoderList == null)
                    return;

                for (var i = 0;; i++)
                {
                    if (decoderList[i] == null)
                        break;

                    var decoder = Marshal.PtrToStructure<SDL2_sound.Sound_DecoderInfo>(new IntPtr(decoderList[i]));
                    var extensionList = (byte**)decoder.extensions;

                    var fmts = new List<string>();

                    if (extensionList != null)
                    {
                        for (var j = 0;; j++)
                        {
                            var ext = Marshal.PtrToStringAnsi(new IntPtr(extensionList[j]));

                            if (ext == null)
                                break;

                            fmts.Add(ext);
                        }
                    }

                    _decoders.Add(
                        new Decoder(
                            Marshal.PtrToStringAnsi(decoderList[i]->description),
                            Marshal.PtrToStringUTF8(decoderList[i]->author),
                            Marshal.PtrToStringAnsi(decoderList[i]->url)
                        ) { SupportedFormats = fmts }
                    );
                }
            }
        }
    }
}