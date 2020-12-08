using System;
using System.Runtime.InteropServices;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public abstract class AudioSource : DisposableResource
    {
        internal IntPtr Handle { get; set; }
        internal SDL2_nmix.NMIX_Source Source => Marshal.PtrToStructure<SDL2_nmix.NMIX_Source>(Handle);

        public bool IsPlaying => SDL2_nmix.NMIX_IsPlaying(Handle);

        internal AudioSource()
        {
        }

        internal abstract void Initialize();
        internal abstract void SourceCallback(IntPtr userData, IntPtr stream, int streamSize);
        
        public virtual void Play()
        {
            EnsureHandleValid();
            SDL2_nmix.NMIX_Play(Handle);
        }

        public virtual void Pause()
        {
            EnsureHandleValid();
            SDL2_nmix.NMIX_Pause(Handle);
        }

        protected void EnsureHandleValid()
        {
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Audio source handle is not valid.");
        }

        protected override void FreeNativeResources()
        {
            EnsureHandleValid();
            
            SDL2_nmix.NMIX_FreeSource(Handle);
            Handle = IntPtr.Zero;
        }
    }
}