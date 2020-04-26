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

        internal AudioClip(IntPtr handle, AudioManager audioManager)
        {
            Handle = handle;
            AudioManager = audioManager;

            if (Handle != IntPtr.Zero)
                SDL_mixer.Mix_VolumeChunk(Handle, Volume);
        }

        public void Play()
        {
            SDL_mixer.Mix_PlayChannel(
                AudioManager.GetFreeChannel(),
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
