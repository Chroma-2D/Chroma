using Chroma.MemoryManagement;
using Chroma.Natives.SDL;
using System;

namespace Chroma.Audio
{
    public class AudioClip : DisposableResource
    {
        private AudioManager AudioManager { get; }
        internal IntPtr Handle { get; }

        internal AudioClip(IntPtr handle, AudioManager audioManager)
        {
            Handle = handle;
            AudioManager = audioManager;
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
