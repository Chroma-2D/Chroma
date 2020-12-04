using System;
using Chroma.MemoryManagement;

namespace Chroma.Audio
{
    public abstract class AudioSource : DisposableResource
    {
        internal IntPtr Handle { get; set; }
        
        public abstract byte Volume { get; set; }

        public virtual PlaybackStatus Status { get; internal set; } = PlaybackStatus.Stopped;
        
        public abstract void Play();
        public abstract void Pause();
        public abstract void Stop();

        public delegate void PostMixWaveformProcessor<T>(Span<T> chunk, Span<byte> bytes);
    }
}