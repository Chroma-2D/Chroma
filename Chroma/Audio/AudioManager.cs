using System;
using System.Collections.Generic;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using System.Linq;
using static Chroma.Audio.AudioSource;

namespace Chroma.Audio
{
    public class AudioManager : DisposableResource
    {
        private SDL_mixer.ChannelFinishedDelegate _channelFinished;
        private SDL_mixer.MusicFinishedDelegate _musicFinished;
        private SDL_mixer.MixFuncDelegate _postMixFunc;

        private Delegate _postMixProcessor;

        private Dictionary<IntPtr, Sound> _soundBank;
        private Dictionary<IntPtr, Music> _musicBank;
        private int _mixingChannelCount;

        private bool _isOpen;

        private Log Log => LogManager.GetForCurrentAssembly();

        public AudioFormat AudioFormat { get; private set; }
        public int SamplingRate { get; private set; } // hz
        public int ChunkSize { get; private set; } // bytes

        public int MixingChannelCount
        {
            get => _mixingChannelCount;
            set
            {
                _mixingChannelCount = value;
                SDL_mixer.Mix_AllocateChannels(_mixingChannelCount);
            }
        }

        public int PlayingChannelCount => SDL_mixer.Mix_Playing(-1);
        public int PausedChannelCount => SDL_mixer.Mix_Paused(-1);

        public byte MusicVolume
        {
            get => (byte)SDL_mixer.Mix_VolumeMusic(-1);
            set
            {
                var volume = value;
                if (volume > 128)
                    volume = 128;

                SDL_mixer.Mix_VolumeMusic(volume);
            }
        }

        public Music CurrentlyPlayedMusic { get; private set; }

        public event EventHandler<SoundEventArgs> SoundPlaybackFinished;
        public event EventHandler<MusicEventArgs> MusicPlaybackFinished;

        internal AudioManager()
        {
            InitializeAudioMixer(AudioFormat.ChromaDefault, 44100, 4096);
        }

        public void InitializeAudioMixer(AudioFormat audioFormat, int samplingRate, int chunkSize)
        {
            if (_isOpen)
            {
                Log.Warning("The audio system was already open. Closing it beforehand for you...");
                ShutdownAudioMixer();
            }

            SamplingRate = samplingRate;
            AudioFormat = audioFormat;
            ChunkSize = chunkSize;

            var result = SDL_mixer.Mix_OpenAudio(
                SamplingRate,
                AudioFormat.SdlMixerFormat,
                SDL_mixer.MIX_DEFAULT_CHANNELS,
                ChunkSize
            );

            if (result == -1)
            {
                Log.Error($"SDL_mixer: {SDL2.SDL_GetError()}");
            }
            else
            {
                _soundBank = new Dictionary<IntPtr, Sound>();
                _musicBank = new Dictionary<IntPtr, Music>();

                _channelFinished = OnChannelFinished;
                _musicFinished = OnMusicFinished;
                _postMixFunc = OnPostMix;

                MixingChannelCount = 16;

                SDL_mixer.Mix_ChannelFinished(_channelFinished);
                SDL_mixer.Mix_HookMusicFinished(_musicFinished);
                SDL_mixer.Mix_SetPostMix(_postMixFunc, IntPtr.Zero);

                _isOpen = true;
            }
        }

        public void ShutdownAudioMixer()
        {
            UnhookPostMixProcessor();
            SDL_mixer.Mix_ChannelFinished(null);
            SDL_mixer.Mix_HookMusicFinished(null);

            foreach (var sound in _soundBank.Values)
            {
                sound.Disposing -= AudioResourceDisposing;
                sound.Dispose();
            }

            _soundBank.Clear();

            foreach (var track in _musicBank.Values)
            {
                track.Disposing -= AudioResourceDisposing;
                track.Dispose();
            }

            _musicBank.Clear();

            SDL_mixer.Mix_CloseAudio();
            _isOpen = false;
        }

        public Sound CreateSound(string filePath)
        {
            var handle = SDL_mixer.Mix_LoadWAV(filePath);

            if (handle != IntPtr.Zero)
            {
                var sound = new Sound(handle, this);

                if (!_soundBank.TryAdd(handle, sound))
                {
                    Log.Error("Failed to add a sound effect to the internal sound bank.");
                }

                sound.Disposing += AudioResourceDisposing;
                return sound;
            }

            Log.Error($"CreateSound: {SDL2.SDL_GetError()}");
            // TODO: throw an instance of audioexception here?
            return null;
        }

        public Music CreateMusic(string filePath)
        {
            var handle = SDL_mixer.Mix_LoadMUS(filePath);

            if (handle != IntPtr.Zero)
            {
                var music = new Music(handle, this);

                if (!_musicBank.TryAdd(handle, music))
                {
                    Log.Error("Failed to add a music object to the internal music bank.");
                }

                music.Disposing += AudioResourceDisposing;
                return music;
            }

            Log.Error($"CreateMusic: {SDL2.SDL_GetError()}");
            // TODO: throw an instance of audioexception here?
            return null;
        }

        public IEnumerable<Sound> FindSoundsByPreferredChannel(int channel)
            => _soundBank.Values.Where(sound => sound.PreferredChannel == channel);

        public IEnumerable<Sound> FindSoundsByActualChannel(int channel)
            => _soundBank.Values.Where(sound => sound.ActualChannel == channel);

        public void HookPostMixProcessor<T>(PostMixWaveformProcessor<T> func) where T : struct
        {
            if (typeof(T) == typeof(float) && AudioFormat.SampleFormat == SampleFormat.F32)
            {
                _postMixProcessor = func.Method.CreateDelegate(
                    typeof(PostMixWaveformProcessor<float>),
                    func.Target
                );
            }
            else if (typeof(T) == typeof(int) && AudioFormat.SampleFormat == SampleFormat.S32)
            {
                _postMixProcessor = func.Method.CreateDelegate(
                    typeof(PostMixWaveformProcessor<int>),
                    func.Target
                );
            }
            else if (typeof(T) == typeof(short) && AudioFormat.SampleFormat == SampleFormat.S16)
            {
                _postMixProcessor = func.Method.CreateDelegate(
                    typeof(PostMixWaveformProcessor<short>),
                    func.Target
                );
            }
            else if (typeof(T) == typeof(ushort) && AudioFormat.SampleFormat == SampleFormat.U16)
            {
                _postMixProcessor = func.Method.CreateDelegate(
                    typeof(PostMixWaveformProcessor<ushort>),
                    func.Target
                );
            }
            else if (typeof(T) == typeof(sbyte) && AudioFormat.SampleFormat == SampleFormat.S8)
            {
                _postMixProcessor = func.Method.CreateDelegate(
                    typeof(PostMixWaveformProcessor<sbyte>),
                    func.Target
                );
            }
            else if (typeof(T) == typeof(byte) && AudioFormat.SampleFormat == SampleFormat.U8)
            {
                _postMixProcessor = func.Method.CreateDelegate(
                    typeof(PostMixWaveformProcessor<byte>),
                    func.Target
                );
            }
            else throw new ArgumentException("Unsupported sample type or sample type mismatch.");
        }

        public void UnhookPostMixProcessor()
            => _postMixProcessor = null;

        internal PlaybackStatus BeginMusicPlayback(Music music)
        {
            if (music.Status == PlaybackStatus.Playing)
                return music.Status;

            if (music.Status == PlaybackStatus.Stopped)
            {
                var loopCount = music.Loop ? -1 : 0;

                SDL_mixer.Mix_PlayMusic(music.Handle, loopCount);
                CurrentlyPlayedMusic = music;
            }
            else if (music.Status == PlaybackStatus.Paused)
            {
                SDL_mixer.Mix_ResumeMusic();
            }

            return PlaybackStatus.Playing;
        }

        internal void PauseMusicPlayback()
        {
            if (CurrentlyPlayedMusic == null)
                return;

            if (CurrentlyPlayedMusic.Status == PlaybackStatus.Playing)
                SDL_mixer.Mix_PauseMusic();
        }

        internal void StopMusicPlayback()
        {
            if (CurrentlyPlayedMusic == null)
                return;

            if (CurrentlyPlayedMusic.Status == PlaybackStatus.Stopped)
                return;

            SDL_mixer.Mix_HaltMusic();
            CurrentlyPlayedMusic = null;
        }

        internal void OnChannelFinished(int channel)
        {
            var handle = SDL_mixer.Mix_GetChunk(channel);

            if (_soundBank.TryGetValue(handle, out var sound))
            {
                sound.Status = PlaybackStatus.Stopped;
                SoundPlaybackFinished?.Invoke(this, new SoundEventArgs(sound));
            }
        }

        internal void OnMusicFinished()
        {
            CurrentlyPlayedMusic.Status = PlaybackStatus.Stopped;
            var music = CurrentlyPlayedMusic;

            CurrentlyPlayedMusic = null;

            MusicPlaybackFinished?.Invoke(this, new MusicEventArgs(music));
        }

        internal void OnPostMix(IntPtr udata, IntPtr stream, int length)
        {
            unsafe
            {
                var streamPtr = stream.ToPointer();

                if (AudioFormat.SampleFormat == SampleFormat.F32)
                {
                    var deleg = _postMixProcessor as PostMixWaveformProcessor<float>;
                    deleg?.Invoke(new Span<float>(streamPtr, length / sizeof(float)),
                        new Span<byte>(streamPtr, length));
                }
                else if (AudioFormat.SampleFormat == SampleFormat.S32)
                {
                    var deleg = _postMixProcessor as PostMixWaveformProcessor<int>;
                    deleg?.Invoke(new Span<int>(streamPtr, length / sizeof(int)), new Span<byte>(streamPtr, length));
                }
                else if (AudioFormat.SampleFormat == SampleFormat.S16)
                {
                    var deleg = _postMixProcessor as PostMixWaveformProcessor<short>;
                    deleg?.Invoke(new Span<short>(streamPtr, length / sizeof(short)),
                        new Span<byte>(streamPtr, length));
                }
                else if (AudioFormat.SampleFormat == SampleFormat.U16)
                {
                    var deleg = _postMixProcessor as PostMixWaveformProcessor<ushort>;
                    deleg?.Invoke(new Span<ushort>(streamPtr, length / sizeof(ushort)),
                        new Span<byte>(streamPtr, length));
                }
                else if (AudioFormat.SampleFormat == SampleFormat.S8)
                {
                    var deleg = _postMixProcessor as PostMixWaveformProcessor<sbyte>;
                    deleg?.Invoke(new Span<sbyte>(streamPtr, length), new Span<byte>(streamPtr, length));
                }
                else if (AudioFormat.SampleFormat == SampleFormat.U8)
                {
                    var deleg = _postMixProcessor as PostMixWaveformProcessor<byte>;
                    deleg?.Invoke(new Span<byte>(streamPtr, length), new Span<byte>(streamPtr, length));
                }
            }
        }

        internal void AudioResourceDisposing(object sender, EventArgs e)
        {
            var audioSource = (sender as AudioSource);

            if (audioSource == null)
            {
                Log.Warning($"Tried to dispose {sender.GetType()}, which is definitely not an AudioSource.");
                return;
            }

            audioSource.Disposing -= AudioResourceDisposing;

            if (sender is Sound sound)
                _soundBank.Remove(sound.Handle);
            else if (sender is Music music)
                _musicBank.Remove(music.Handle);
        }

        protected override void FreeManagedResources()
        {
            foreach (var sound in _soundBank.Values)
                sound.Dispose();

            foreach (var music in _musicBank.Values)
                music.Dispose();
        }
    }
}