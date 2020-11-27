using System;
using Chroma.Audio.Filters;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Sources
{
    public class Speech : AudioSource
    {
        private string _text = string.Empty;
        private int _baseFrequency = 1330;
        private float _baseSpeed = 10f;
        private float _baseDeclination = 0.5f;
        private SpeechWaveform _baseWaveform = SpeechWaveform.Square;
        
        public override bool SupportsLength => false;

        public override double LoopingPoint
        {
            get => SoLoud.Speech_getLoopPoint(Handle);
            set => SoLoud.Speech_setLoopPoint(Handle, value);
        }

        public int BaseFrequency
        {
            get => _baseFrequency;
            set
            {
                _baseFrequency = value;
                UpdateParameters();
            }
        }

        public float BaseSpeed
        {
            get => _baseSpeed;
            set
            {
                _baseSpeed = value;
                UpdateParameters();
            }
        }

        public float BaseDeclination
        {
            get => _baseDeclination;
            set
            {
                _baseDeclination = value; 
                UpdateParameters();
            }
        }

        public SpeechWaveform BaseWaveform
        {
            get => _baseWaveform;
            set
            {
                _baseWaveform = value;
                UpdateParameters();
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                SoLoud.Speech_setText(Handle, _text);
            }
        }
        
        public Speech() 
            : base(SoLoud.Speech_create())
        {
        }

        protected override void ApplyFilter(int slot, AudioFilter filter)
            => SoLoud.Speech_setFilter(Handle, (uint)slot, filter.Handle);

        protected override void ClearFilter(int slot)
            => SoLoud.Speech_setFilter(Handle, (uint)slot, IntPtr.Zero);

        protected override void SetInaudibleBehavior(bool tickWhenSilent, bool killAfterGoingSilent)
            => SoLoud.Speech_setInaudibleBehavior(Handle, tickWhenSilent, killAfterGoingSilent);

        protected override void SetLooping(bool looping)
            => SoLoud.Speech_setLooping(Handle, looping);

        protected override void SetVolume(float volume)
            => SoLoud.Speech_setVolume(Handle, volume);

        private void UpdateParameters()
        {
            SoLoud.Speech_setParamsEx(
                Handle,
                (uint)_baseFrequency,
                _baseSpeed,
                _baseDeclination,
                (int)_baseWaveform
            );
        }
    }
}