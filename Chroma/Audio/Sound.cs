using Chroma.Diagnostics.Logging;
using Chroma.Natives.CRaudio;
using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public class Sound : AudioSource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();
        private CRaudio.CRaudio_LoadInfo _loadInfo;
        
        internal Sound(OutputDevice device) : base(device)
        {
        }

        public void Play()
        {
            Al.alSourcePlay(Handle);
        }

        public void Stop()
        {
            Al.alSourceStop(Handle);
        }

        public void Pause()
        {
            Al.alSourcePause(Handle);
        }

        internal bool LoadDataFromFile(string path)
        {
            if (!HasValidBuffer)
            {
                _log.Error("Cannot load data into a source without a valid buffer.");
                return false;
            }
            
            if (!CRaudio.CR_LoadOgg(path, out _loadInfo))
            {
                _log.Error($"Failed to load OGGvorbis file: 0x{CRaudio.CR_GetError():X4}");
                return false;
            }
            
            Al.alBufferData(
                Buffer, 
                _loadInfo.format,
                _loadInfo.data,
                _loadInfo.size,
                _loadInfo.freq
            );

            var error = Device.Manager.LogOpenAlError("Failed to buffer sound data: ");
            
            if (error != Al.AL_NO_ERROR)
                return false;

            return AttachBuffer();
        }

        protected override void FreeNativeResources()
        {
            Al.alSourcei(Handle, Al.AL_BUFFER, 0);
            Al.alDeleteBuffer(ref Buffer);
            Al.alDeleteSource(ref Handle);
            
            CRaudio.CR_FreeOgg(ref _loadInfo);
        }
    }
}