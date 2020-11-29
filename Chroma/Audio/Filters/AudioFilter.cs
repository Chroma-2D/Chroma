using System;
using Chroma.Audio.Sources;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Filters
{
    public abstract class AudioFilter : AudioObject
    {
        internal AudioSource Owner { get; private set; }
        internal int Slot { get; private set; } = -1;

        public bool IsAttachedToAudioSource => Owner != null && Slot >= 0;

        internal AudioFilter(IntPtr handle) : base(handle)
        {
        }

        protected void FadeParameter<T>(T parameter, float targetValue, double timeSeconds)
            where T : Enum
        {
            EnsureAttached();
            
            SoLoud.Soloud_fadeFilterParameter(
                AudioManager.Instance.Handle,
                Owner.VoiceHandle,
                (uint)Slot,
                (uint)(object)parameter,
                targetValue,
                timeSeconds
            );
        }

        internal abstract void UpdateParameters();
        
        internal virtual void OnClear()
        {
            if (Owner != null)
                Detach();
        }

        internal virtual void BeforeApply(AudioSource source, int slot)
        {
            if (Owner != null)
                Detach();
        }

        internal virtual void OnApplied(AudioSource source, int slot)
        {
            Owner = source;
            Slot = slot;
        }

        private void Detach()
        {
            Owner.SetFilter(Slot, null);

            Owner = null;
            Slot = -1;
        }

        private void EnsureAttached()
        {
            if (Owner == null || Slot == -1)
                throw new InvalidOperationException("Filter is not applied to any audio source.");
        }
    }
}