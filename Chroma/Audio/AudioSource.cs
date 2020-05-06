using System;
using Chroma.MemoryManagement;

namespace Chroma.Audio
{
    public abstract class AudioSource : DisposableResource
    {
        protected AudioManager AudioManager { get; }
        
        internal IntPtr Handle { get; set; }
        
        public abstract byte Volume { get; set; }

        public virtual int LoopCount { get; set; }
        public virtual PlaybackStatus Status { get; protected set; } = PlaybackStatus.Stopped;
        
        public abstract void Play();
        public abstract void Pause();
        public abstract void Stop();

        public delegate void ChunkProcessorDelegate(Span<byte> chunk);

        internal AudioSource(IntPtr handle, AudioManager audioManager)
        {
            Handle = handle;
            AudioManager = audioManager;
        }
    }
}