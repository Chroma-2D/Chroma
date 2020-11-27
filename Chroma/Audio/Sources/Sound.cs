using System;
using Chroma.Audio.Filters;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Sources
{
    public class Sound : AudioSource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public override double LoopingPoint
        {
            get => SoLoud.WavStream_getLoopPoint(Handle);
            set => SoLoud.WavStream_setLoopPoint(Handle, value);
        }
        
        public override bool SupportsLength => true;

        public override double Length
            => SoLoud.WavStream_getLength(Handle);
        
        public Sound(string filePath) 
            : base(SoLoud.Wav_create())
        {
            ValidateHandle();

            var error = SoLoud.Wav_load(Handle, filePath);
            if (error < 0)
            {
                _log.Error(
                    $"Failed to load music from file: " +
                    $"{SoLoud.Soloud_getErrorString(AudioManager.Instance.Handle, error)}"
                );

                Dispose();
                return;
            }
            
            InitializeState();
        }
        
        protected override void SetInaudibleBehavior(bool mustTick, bool killAfterGoingSilent)
            => SoLoud.Wav_setInaudibleBehavior(Handle, mustTick, killAfterGoingSilent);

        protected override void SetLooping(bool looping)
            => SoLoud.Wav_setLooping(Handle, looping);

        protected override void ApplyFilter(int slot, AudioFilter filter)
            => SoLoud.Wav_setFilter(Handle, (uint)slot, filter.Handle);

        protected override void ClearFilter(int slot)
            => SoLoud.Wav_setFilter(Handle, (uint)slot, IntPtr.Zero);
        
        protected override void SetVolume(float volume)
            => SoLoud.Wav_setVolume(Handle, volume);

        protected override void FreeNativeResources()
        {
            if (Handle != IntPtr.Zero)
            {
                SoLoud.Wav_stop(Handle);
                SoLoud.Wav_destroy(Handle);
                
                DestroyHandle();
            }
        }
    }
}