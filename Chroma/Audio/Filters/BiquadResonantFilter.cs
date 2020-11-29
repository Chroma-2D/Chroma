using System;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Filters
{
    public class BiquadResonantFilter : AudioFilter
    {
        public enum FilterType
        {
            LowPass = SoLoud.BIQUAD_RESONANT_LOWPASS,
            BandPass = SoLoud.BIQUAD_RESONANT_BANDPASS,
            HighPass = SoLoud.BIQUAD_RESONANT_HIGHPASS
        }

        private readonly Log _log = LogManager.GetForCurrentAssembly();

        private FilterType _type = FilterType.LowPass;
        private float _cutoffFrequency = 44100;
        private float _resonance = 2;

        public FilterType Type
        {
            get => _type;
            set
            {
                _type = value;
                UpdateParameters();
            }
        }

        public float MaximumRecommendedCutoffFrequency
            => SoLoud.BiquadResonantFilter_getParamMax(Handle, SoLoud.BIQUAD_RESONANT_FREQUENCY);

        public float MinimumRecommendedCutoffFrequency
            => SoLoud.BiquadResonantFilter_getParamMin(Handle, SoLoud.BIQUAD_RESONANT_FREQUENCY);

        public float CutoffFrequency
        {
            get => _cutoffFrequency;
            set
            {
                _cutoffFrequency = value;
                UpdateParameters();
            }
        }

        public float MaximumRecommendedResonance
            => SoLoud.BiquadResonantFilter_getParamMax(Handle, SoLoud.BIQUAD_RESONANT_RESONANCE);

        public float MinimumRecommendedResonance
            => SoLoud.BiquadResonantFilter_getParamMin(Handle, SoLoud.BIQUAD_RESONANT_RESONANCE);

        public float Resonance
        {
            get => _resonance;
            set
            {
                _resonance = value;
                UpdateParameters();
            }
        }

        public BiquadResonantFilter()
            : base(SoLoud.BiquadResonantFilter_create())
        {
        }

        internal override void UpdateParameters()
        {
            var error = SoLoud.BiquadResonantFilter_setParams(
                Handle,
                (int)_type,
                _cutoffFrequency,
                _resonance
            );

            if (error > 0)
            {
                _log.Error(
                    $"Failed to update parameters: " +
                    $"{SoLoud.Soloud_getErrorString(AudioManager.Instance.Handle, error)}"
                );
            }
        }

        protected override void FreeNativeResources()
        {
            if (Handle != IntPtr.Zero)
            {
                SoLoud.BiquadResonantFilter_destroy(Handle);
                DestroyHandle();
            }
        }
    }
}