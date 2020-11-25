using System;
using Chroma.Audio.Filters;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio
{
    public class AudioManager : AudioObject
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();
        private readonly AudioFilter[] _globalFilters = new AudioFilter[SoLoud.SOLOUD_MAX_FILTERS];

        private bool _isVisualizationEnabled = true;
        
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

        public int OutputChannelCount => (int)SoLoud.Soloud_getBackendChannels(Handle);
        public int ActiveVoiceCount => (int)SoLoud.Soloud_getActiveVoiceCount(Handle);

        public int MaximumActiveVoiceCount
        {
            get => (int)SoLoud.Soloud_getMaxActiveVoiceCount(Handle);
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(string.Empty, "Maximum active voice count cannot be negative.");
                
                SoLoud.Soloud_setMaxActiveVoiceCount(Handle, (uint)value);
            }
        }

        public bool IsVisualizationEnabled
        {
            get => _isVisualizationEnabled;
            set
            {
                _isVisualizationEnabled = value;
                SoLoud.Soloud_setVisualizationEnable(
                    Handle,
                    _isVisualizationEnabled
                );
            }
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

        public void FadeGlobalVolume(float targetValue, double fadeSeconds)
        {
            ValidateHandle();
            
            SoLoud.Soloud_fadeGlobalVolume(
                Handle,
                targetValue,
                fadeSeconds
            );
        }

        public void SetGlobalFilter(int slot, AudioFilter filter)
        {
            ValidateHandle();

            if (slot >= SoLoud.SOLOUD_MAX_FILTERS || slot < 0)
            {
                _log.Warning($"Refusing to set out-of-range global filter slot '{slot}'.");
                return;
            }

            if (filter == null)
            {
                _globalFilters[slot] = null;
                SoLoud.Soloud_setGlobalFilter(
                    Handle,
                    (uint)slot,
                    IntPtr.Zero
                );
                return;
            }

            if (filter.Disposed)
                throw new InvalidOperationException("Filter you're trying to apply was already disposed.");

            _globalFilters[slot] = filter;
            SoLoud.Soloud_setGlobalFilter(
                Handle,
                (uint)slot,
                filter.Handle
            );
        }

        public float ApproximateChannelVolume(int channel)
        {
            ValidateHandle();

            if (channel < 0)
                throw new ArgumentOutOfRangeException(nameof(channel), "Channel ID cannot be negative.");
            
            return SoLoud.Soloud_getApproximateVolume(
                Handle,
                (uint)channel
            );
        }

        public float[] CalculateFFT()
        {
            ValidateHandle();
            return SoLoud.Soloud_calcFFT(Handle);
        }

        public float[] SampleWaveform()
        {
            ValidateHandle();
            return SoLoud.Soloud_getWave(Handle);
        }

        protected override void FreeNativeResources()
        {
            SoLoud.Soloud_deinit(Handle);
            SoLoud.Soloud_destroy(Handle);
        }
    }
}