using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio
{
    public class AudioManager : AudioObject
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();
        
        internal static AudioManager Instance { get; private set; }

        public float MasterVolume
        {
            get => SoLoud.Soloud_getGlobalVolume(Handle);
            set => SoLoud.Soloud_setGlobalVolume(Handle, value);
        }

        public float PostClipScaler
        {
            get => SoLoud.Soloud_getPostClipScaler(Handle);
            set => SoLoud.Soloud_setPostClipScaler(Handle, value);
        }
        
        internal AudioManager() 
            : base(SoLoud.Soloud_create())
        {
            SoLoud.Soloud_initEx(
                Handle,
                SoLoud.SoLoud_InitFlags.CLIP_ROUNDOFF
                | SoLoud.SoLoud_InitFlags.ENABLE_VISUALIZATION,
                SoLoud.SoLoud_Backend.AUTO,
                0, 0, 0
            );
            
            _log.Info($"SoLoud Audio {SoLoud.Soloud_getVersion(Handle)} [{SoLoud.Soloud_getBackendString(Handle)}]");

            Instance = this;
        }

        protected override void FreeNativeResources()
        {
            SoLoud.Soloud_deinit(Handle);
            SoLoud.Soloud_destroy(Handle);
        }
    }
}