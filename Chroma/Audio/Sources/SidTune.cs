using System;
using Chroma.Audio.Filters;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Sources
{
    public class SidTune : AudioSource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public override bool SupportsLength => false;

        public override double LoopingPoint
        {
            get => SoLoud.TedSid_getLoopPoint(Handle);
            set => SoLoud.TedSid_setLoopPoint(Handle, value);
        }
        
        public SidTune(string filePath) 
            : base(SoLoud.TedSid_create())
        {
            TryInitialize(
                () => SoLoud.TedSid_load(Handle, filePath),
                $"Failed to load a SID dump file '{filePath}'"
            );
        }

        protected override void ApplyFilter(int slot, AudioFilter filter)
            => SoLoud.TedSid_setFilter(Handle, (uint)slot, filter.Handle);

        protected override void ClearFilter(int slot)
            => SoLoud.TedSid_setFilter(Handle, (uint)slot, IntPtr.Zero);

        protected override void SetInaudibleBehavior(bool tickWhenSilent, bool killAfterGoingSilent)
            => SoLoud.TedSid_setInaudibleBehavior(Handle, tickWhenSilent, killAfterGoingSilent);

        protected override void SetLooping(bool looping)
            => SoLoud.TedSid_setLooping(Handle, looping);

        protected override void SetVolume(float volume)
            => SoLoud.TedSid_setVolume(Handle, volume);

        protected override void FreeNativeResources()
        {
            if (Handle != IntPtr.Zero)
            {
                SoLoud.TedSid_stop(Handle);
                SoLoud.TedSid_destroy(Handle);
                
                DestroyHandle();
            }
        }
    }
}