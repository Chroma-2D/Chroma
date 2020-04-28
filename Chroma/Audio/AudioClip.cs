using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using System;

namespace Chroma.Audio
{
    public class AudioClip : DisposableResource
    {
        private AudioManager AudioManager { get; }

        internal IntPtr Handle { get; }

        private byte _volume = 32;

        public byte Volume
        {
            get => _volume;

            set
            {
                EnsureNotDisposed();

                _volume = value;
                SDL_mixer.Mix_VolumeChunk(Handle, _volume);
            }
        }

        public int Channel { get; private set; }

        internal AudioClip(IntPtr handle, AudioManager audioManager)
        {
            Handle = handle;
            AudioManager = audioManager;

            if (Handle != IntPtr.Zero)
                SDL_mixer.Mix_VolumeChunk(Handle, Volume);
        }

        public void Play(int channel = -1)
        {
            EnsureNotDisposed();

            if (channel < 0)
                Channel = AudioManager.GetFreeChannel();

            Channel = channel;
            
            SDL_mixer.Mix_PlayChannel(
                Channel,
                Handle,
                0
            );
        }

        protected override void FreeNativeResources()
        {
            SDL_mixer.Mix_FreeChunk(Handle);
        }
    }
}