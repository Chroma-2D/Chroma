using System;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio.Sources
{
    public class Music : AudioSource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public bool Loop { get; set; }

        public override byte Volume
        {
            get => AudioManager.Instance.MusicVolume;
            set => AudioManager.Instance.MusicVolume = value;
        }

        public static Music Current { get; private set; }

        public Music(string filePath)
        {
            Handle = SDL_mixer.Mix_LoadMUS(filePath);

            if (Handle == IntPtr.Zero)
            {
                _log.Error($"Failed to load music file from '{filePath}': {SDL2.SDL_GetError()}");
                
                Dispose();
                return;
            }

            AudioManager.Instance.RegisterMusicSource(this);
        }

        public override void Play()
        {
            EnsureNotDisposed();

            if (Current != null)
                Current.Stop();

            if (Status == PlaybackStatus.Playing)
                return;

            if (Status == PlaybackStatus.Stopped)
            {
                var loopCount = Loop ? -1 : 0;

                SDL_mixer.Mix_PlayMusic(Handle, loopCount);
                Current = this;
            }
            else if (Status == PlaybackStatus.Paused)
            {
                SDL_mixer.Mix_ResumeMusic();
            }

            Status = PlaybackStatus.Playing;
        }

        public override void Pause()
        {
            EnsureNotDisposed();

            if (Status != PlaybackStatus.Playing)
                return;

            SDL_mixer.Mix_PauseMusic();
            Status = PlaybackStatus.Paused;
        }

        public override void Stop()
        {
            EnsureNotDisposed();

            if (Status == PlaybackStatus.Stopped)
                return;

            SDL_mixer.Mix_HaltMusic();
            Current = null;

            Status = PlaybackStatus.Stopped;
        }

        protected override void FreeNativeResources()
        {
            SDL_mixer.Mix_FreeMusic(Handle);
        }

        internal void OnFinish()
        {
            Status = PlaybackStatus.Stopped;
            Current = null;
        }
    }
}