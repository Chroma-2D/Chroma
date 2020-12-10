using System;
using System.Collections.Generic;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;

namespace Chroma.Audio.Sources
{
    public abstract class AudioSource : DisposableResource
    {
        public delegate void AudioStreamDelegate(Span<byte> audioBufferData, AudioFormat format);

        protected IntPtr Handle { get; set; }
        
        internal unsafe SDL2_nmix.NMIX_Source* Source 
            => (SDL2_nmix.NMIX_Source*)Handle.ToPointer();
        
        public virtual PlaybackStatus Status { get; set; }
        
        public bool IsPlaying => SDL2_nmix.NMIX_IsPlaying(Handle);

        public float Panning
        {
            get => SDL2_nmix.NMIX_GetPan(Handle);
            set
            {
                var pan = value;

                if (pan < -1.0f)
                    pan = 1.0f;

                if (pan > 1.0f)
                    pan = 1.0f;

                SDL2_nmix.NMIX_SetPan(Handle, pan);
            }
        }

        public float Volume
        {
            get => SDL2_nmix.NMIX_GetGain(Handle);

            set
            {
                var vol = value;

                if (vol < 0f)
                    vol = 0f;

                if (vol > 2f)
                    vol = 2f;
                
                SDL2_nmix.NMIX_SetGain(Handle, vol);
            }
        }
        
        public List<AudioStreamDelegate> Filters { get; private set; } 
            = new List<AudioStreamDelegate>();

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

        public virtual void Stop()
        {
            throw new NotSupportedException("This audio source does not support stopping.");
        }

        protected void EnsureHandleValid()
        {
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException("Audio source handle is not valid.");
        }

        protected override void FreeNativeResources()
        {
            if (Handle != IntPtr.Zero)
            {
                SDL2_nmix.NMIX_FreeSource(Handle);
                Handle = IntPtr.Zero;
            }
        }
    }
}