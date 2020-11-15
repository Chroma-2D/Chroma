using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public abstract class AudioSource : DisposableResource
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        
        internal uint Handle;
        internal uint Buffer;
        
        internal OutputDevice Device { get; }

        public bool IsValid => Handle != 0;
        public bool HasValidBuffer => Buffer != 0;

        public AudioSourceState State
        {
            get
            {
                Al.alGetSourcei(
                    Handle,
                    Al.AL_SOURCE_STATE,
                    out var state
                );

                var error = Device.Manager.LogOpenAlError("Failed to retrieve audio source state: ");

                if (error != Al.AL_NO_ERROR)
                    return AudioSourceState.Invalid;

                return (AudioSourceState)state;
            }
        }

        internal AudioSource(OutputDevice device)
        {
            Device = device;

            if (!CreateBuffer())
                return;

            CreateSource();
        }

        private bool CreateBuffer()
        {
            var buffer = Al.alGenBuffer();
            var error = Device.Manager.LogOpenAlError("Failed to create audio source: ");

            if (error != Al.AL_NO_ERROR)
                return false;

            Buffer = buffer;
            return true;
        }

        private void CreateSource()
        {
            var src = Al.alGenSource();

            var error = Device.Manager.LogOpenAlError("Failed to create audio source: ");
            if (error != Al.AL_NO_ERROR)
                return;

            Handle = src;
        }

        protected bool AttachBuffer()
        {
            if (!HasValidBuffer)
            {
                _log.Error("Cannot attach an invalid buffer to an audio source.");
                return false;
            }
            
            Al.alSourcei(Handle, Al.AL_BUFFER, (int)Buffer);
            
            return Device.Manager
                .LogOpenAlError("Failed to attach buffer to audio source: ") != Al.AL_NO_ERROR;
        }
    }
}