using Chroma.MemoryManagement;
using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public abstract class AudioSource : DisposableResource
    {
        internal OutputDevice Device { get; }
        internal uint Handle { get; private set; }
        internal uint Buffer { get; private set; }

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
            var bufs = new uint[1];
            Al.alGenBuffers(1, bufs);
            var error = Device.Manager.LogOpenAlError("Failed to create audio source: ");

            if (error != Al.AL_NO_ERROR)
                return false;

            Buffer = bufs[0];
            return true;
        }

        private void CreateSource()
        {
            var srcs = new uint[1];
            Al.alGenSources(1, srcs);

            var error = Device.Manager.LogOpenAlError("Failed to create audio source: ");
            if (error != Al.AL_NO_ERROR)
                return;

            Handle = srcs[0];
        }

        protected void ConfigureSourceDefaults()
        {
            Al.alSourcei(Handle, Al.AL_BUFFER, (int)Buffer);
            Device.Manager.LogOpenAlError("Failed to attach buffer to audio source: ");
            
            Al.alSourcef(Handle, Al.AL_GAIN, 1.0f);
            Al.alSourcef(Handle, Al.AL_SOURCE_TYPE, Al.AL_STREAMING);
        }
    }
}