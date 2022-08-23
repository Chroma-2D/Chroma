using System;
using System.Collections.Generic;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.Bindings.SDL;
using Chroma.Natives.Ports.NMIX;

namespace Chroma.Audio.Sources
{
    public abstract class AudioSource : DisposableResource
    {
        public delegate void AudioStreamDelegate(Span<byte> audioBufferData, AudioFormat format);

        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        protected IntPtr Handle { get; set; }

        internal unsafe SDL2_nmix.NMIX_Source* Source
            => (SDL2_nmix.NMIX_Source*)Handle.ToPointer();

        public virtual PlaybackStatus Status { get; set; }

        public bool IsValid => Handle != IntPtr.Zero;

        public bool IsPlaying
        {
            get
            {
                EnsureHandleValid();
                return SDL2_nmix.NMIX_IsPlaying(Handle);
            }
        }

        public float Panning
        {
            get
            {
                EnsureHandleValid();
                return SDL2_nmix.NMIX_GetPan(Handle);
            }

            set
            {
                EnsureHandleValid();

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
            get
            {
                EnsureHandleValid();

                return SDL2_nmix.NMIX_GetGain(Handle);
            }

            set
            {
                EnsureHandleValid();

                var vol = value;

                if (vol < 0f)
                    vol = 0f;

                if (vol > 2f)
                    vol = 2f;

                SDL2_nmix.NMIX_SetGain(Handle, vol);
            }
        }

        public AudioFormat Format
        {
            get
            {
                EnsureHandleValid();

                unsafe
                {
                    return AudioFormat.FromSdlFormat(
                        Source->format
                    );
                }
            }
        }

        public byte ChannelCount
        {
            get
            {
                EnsureHandleValid();

                unsafe
                {
                    return Source->channels;
                }
            }
        }

        public Span<byte> InBuffer
        {
            get
            {
                EnsureHandleValid();

                unsafe
                {
                    return new Span<byte>(
                        Source->in_buffer,
                        Source->in_buffer_size
                    );
                }
            }
        }

        public Span<byte> OutBuffer
        {
            get
            {
                EnsureHandleValid();

                unsafe
                {
                    return new Span<byte>(
                        Source->out_buffer,
                        Source->out_buffer_size
                    );
                }
            }
        }

        public List<AudioStreamDelegate> Filters { get; } = new();

        public virtual void Play()
        {
            EnsureHandleValid();
            
            if (SDL2_nmix.NMIX_Play(Handle) < 0)
            {
                _log.Error($"Failed to play the audio source: {SDL2.SDL_GetError()}");
            }
        }

        public virtual void Pause()
        {
            EnsureHandleValid();
            SDL2_nmix.NMIX_Pause(Handle);
        }

        public virtual void Stop()
            => throw new AudioException("This audio source does not support stopping.");

        protected void EnsureHandleValid()
        {
            if (!IsValid)
                throw new AudioException("Audio source handle is not valid.");
        }

        protected override void FreeNativeResources()
        {
            EnsureOnMainThread();
            
            if (IsValid)
            {
                SDL2_nmix.NMIX_FreeSource(Handle);
                Handle = IntPtr.Zero;
            }
        }
    }
}