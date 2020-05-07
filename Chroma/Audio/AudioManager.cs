using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using System;
using System.Collections.Generic;

namespace Chroma.Audio
{
    public class AudioManager : DisposableResource
    {
        private SDL_mixer.ChannelFinishedDelegate _channelFinished;
        private SDL_mixer.MusicFinishedDelegate _musicFinished;

        private Dictionary<IntPtr, Sound> _soundBank;
        private Dictionary<IntPtr, Music> _musicBank;
        private int _mixingChannelCount;

        private Log Log => LogManager.GetForCurrentAssembly();

        public int SamplingRate { get; } = 44100; // hz
        public int ChunkSize { get; } = 8192; // bytes

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
            set => SDL_mixer.Mix_VolumeMusic(NormalizeByteToMixerVolume(value));
        }

        public Music CurrentlyPlayedMusic { get; private set; }

        public event EventHandler<SoundEventArgs> SoundPlaybackFinished;
        public event EventHandler<MusicEventArgs> MusicPlaybackFinished;

        internal AudioManager()
        {
            var result = SDL_mixer.Mix_OpenAudio(
                SamplingRate,
                AudioFormat.Default.SdlMixerFormat,
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

                MixingChannelCount = 16;

                SDL_mixer.Mix_ChannelFinished(_channelFinished);
                SDL_mixer.Mix_HookMusicFinished(_musicFinished);
            }
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

        internal PlaybackStatus BeginMusicPlayback(Music music, bool fadeIn, int fadeMillis = 0)
        {
            if (music.Status == PlaybackStatus.Playing)
                return music.Status;

            if (music.Status == PlaybackStatus.Stopped)
            {
                var loopCount = music.Loop ? -1 : 0;

                if (fadeIn)
                {
                    SDL_mixer.Mix_FadeInMusic(music.Handle, loopCount, fadeMillis);
                }
                else
                {
                    SDL_mixer.Mix_PlayMusic(music.Handle, loopCount);
                }
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

            if (_soundBank.TryGetValue(handle, out Sound sound))
                SoundPlaybackFinished?.Invoke(this, new SoundEventArgs(sound));
        }

        internal void OnMusicFinished()
        {
            var music = CurrentlyPlayedMusic;
            CurrentlyPlayedMusic = null;

            MusicPlaybackFinished?.Invoke(this, new MusicEventArgs(music));
        }

        internal void AudioResourceDisposing(object sender, EventArgs e)
        {
            (sender as AudioSource).Disposing -= AudioResourceDisposing;

            if (sender is Sound sound)
                _soundBank.Remove(sound.Handle);
            else if (sender is Music music)
                _musicBank.Remove(music.Handle);
        }

        internal static byte NormalizeByteToMixerVolume(byte volume)
            => (byte)(volume / 255f * 128);

        protected override void FreeManagedResources()
        {
            foreach (var sound in _soundBank.Values)
                sound.Dispose();
        }
    }
}
