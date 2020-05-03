using System;
using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class Sound : AudioSource
    {
        private SDL_mixer.ChannelFinishedDelegate OnFinished { get; set; }

        private byte _volume;
        public override byte Volume
        {
            get => _volume;
            set
            {
                EnsureNotDisposed();
                
                _volume = value;
                SDL_mixer.Mix_VolumeChunk(Handle, _volume);
            }
        }

        public int PreferredChannel { get; set; } = -1;
        public int ActualChannel { get; private set; }

        public Sound(IntPtr handle, AudioManager audioManager) : base(handle, audioManager)
        {
            OnFinished = OnFinishedHandler;
            SDL_mixer.Mix_ChannelFinished(OnFinished);
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
                // TODO: let the user know something's wrong
            }
        }

        public void ForcePlay()
        {
            EnsureNotDisposed();

            if (Status == PlaybackStatus.Paused || Status == PlaybackStatus.Playing)
                SDL_mixer.Mix_HaltChannel(ActualChannel);

            ActualChannel = SDL_mixer.Mix_PlayChannel(PreferredChannel, Handle, LoopCount);

            if (ActualChannel >= 0)
            {
                Status = PlaybackStatus.Playing;
            }
            else
            {
                // TODO: let the user know something's wrong
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

            OnFinished = null;
        }

        protected override void FreeNativeResources()
        {           
            SDL_mixer.Mix_FreeChunk(Handle);
        }
        
        private void OnFinishedHandler(int channel)
        {
            if (channel == ActualChannel)
                OnPlaybackFinished();
        }
    }
}