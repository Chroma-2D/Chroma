using System;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class Sound : AudioSource
    {
        private Log Log => LogManager.GetForCurrentAssembly();

        public override byte Volume
        {
            get => (byte)SDL_mixer.Mix_VolumeChunk(Handle, -1);
            set
            {
                EnsureNotDisposed();

                var volume = value;

                if (volume > 128)
                    volume = 128;

                SDL_mixer.Mix_VolumeChunk(Handle, volume);
            }
        }

        public int LoopCount { get; set; } = 0;
        public int PreferredChannel { get; set; } = -1;
        public int ActualChannel { get; private set; }

        internal Sound(IntPtr handle, AudioManager audioManager) : base(handle, audioManager)
        {
        }

        public override void Play()
        {
            EnsureNotDisposed();

            if (Status == PlaybackStatus.Paused)
            {
                SDL_mixer.Mix_Resume(ActualChannel);
                Status = PlaybackStatus.Playing;

                return;
            }

            if (Status == PlaybackStatus.Playing)
                return;

            ActualChannel = SDL_mixer.Mix_PlayChannel(PreferredChannel, Handle, LoopCount);

            if (ActualChannel >= 0)
            {
                Status = PlaybackStatus.Playing;
            }
            else
            {
                Log.Error($"Failed to play the sound: {SDL2.SDL_GetError()}");
            }
        }

        public void PlayOneShot()
        {
            EnsureNotDisposed();

            if (Status == PlaybackStatus.Paused || Status == PlaybackStatus.Playing)
                SDL_mixer.Mix_HaltChannel(ActualChannel);

            ActualChannel = SDL_mixer.Mix_PlayChannel(PreferredChannel, Handle, 0);

            if (ActualChannel >= 0)
            {
                Status = PlaybackStatus.Playing;
            }
            else
            {
                Log.Error($"Failed to one-shot play the sound: {SDL2.SDL_GetError()}");
            }
        }

        public override void Pause()
        {
            EnsureNotDisposed();

            if (Status == PlaybackStatus.Paused || Status == PlaybackStatus.Stopped)
                return;

            SDL_mixer.Mix_Pause(ActualChannel);
            Status = PlaybackStatus.Paused;
        }

        public override void Stop()
        {
            EnsureNotDisposed();

            if (Status == PlaybackStatus.Stopped)
                return;

            SDL_mixer.Mix_HaltChannel(ActualChannel);
            Status = PlaybackStatus.Stopped;
        }

        protected override void FreeManagedResources()
        {
            if (Status == PlaybackStatus.Playing)
                Stop();
        }

        protected override void FreeNativeResources()
        {
            SDL_mixer.Mix_FreeChunk(Handle);
        }
    }
}