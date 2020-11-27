using System;
using Chroma.Audio.Filters;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Sources
{
    public class TrackerModule : AudioSource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();
        
        public override double LoopingPoint { get; set; }
        
        public override bool SupportsLength => false;

        public TrackerModule(string filePath)
            : base(SoLoud.Openmpt_create())
        {
            ValidateHandle();

            var error = SoLoud.Openmpt_load(Handle, filePath);
            if (error > 0)
            {
                _log.Error($"Failed to load a module file '{filePath}': '{SoLoud.Soloud_getErrorString(AudioManager.Instance.Handle, error)}'");
                
                Dispose();
                return;
            }
            
            InitializeState();
        }

        protected override void ApplyFilter(int slot, AudioFilter filter)
            => SoLoud.Openmpt_setFilter(Handle, (uint)slot, filter.Handle);

        protected override void ClearFilter(int slot)
            => SoLoud.Openmpt_setFilter(Handle, (uint)slot, IntPtr.Zero);

        protected override void SetInaudibleBehavior(bool tickWhenSilent, bool killAfterGoingSilent)
            => SoLoud.Openmpt_setInaudibleBehavior(Handle, tickWhenSilent, killAfterGoingSilent);

        protected override void SetLooping(bool looping)
            => SoLoud.Openmpt_setLooping(Handle, looping);

        protected override void SetVolume(float volume)
            => SoLoud.Openmpt_setVolume(Handle, volume);
    }
}