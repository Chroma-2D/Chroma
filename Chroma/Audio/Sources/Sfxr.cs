using System;
using System.IO;
using Chroma.Audio.Filters;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Sources
{
    public class Sfxr : AudioSource
    {
        private Log _log = LogManager.GetForCurrentAssembly();

        public override bool SupportsLength => false;

        public override double LoopingPoint
        {
            get => SoLoud.Sfxr_getLoopPoint(Handle);
            set => SoLoud.Sfxr_setLoopPoint(Handle, value);
        }

        public Sfxr(string filePath)
            : base(SoLoud.Sfxr_create())
        {
            TryInitialize(
                () => SoLoud.Sfxr_loadParams(Handle, filePath),
                $"Failed to load SFXR parameter file '{filePath}'"
            );
        }
        
        public Sfxr(SfxrPreset preset, int randomSeed)
            : base(SoLoud.Sfxr_create())
        {
            TryInitialize(
                () => SoLoud.Sfxr_loadPreset(Handle, (int)preset, randomSeed),
                "Failed to load SFXR preset"
            );
        }    

        public Sfxr(Stream stream)
            : base(SoLoud.Sfxr_create())
        {
            TryInitialize(
                () =>
                {
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);

                        var bytes = ms.ToArray();

                        unsafe
                        {
                            fixed (byte* b = &bytes[0])
                            {
                                return SoLoud.Sfxr_loadParamsMemEx(
                                    Handle,
                                    new IntPtr(b),
                                    (uint)bytes.Length,
                                    true,
                                    true
                                );
                            }
                        }
                    }
                },
                "Failed to load SFXR parameter data from memory"
            );
        }

        public void Reset()
        {
            ValidateHandle();
            SoLoud.Sfxr_resetParams(Handle);
        }

        protected override void ApplyFilter(int slot, AudioFilter filter)
            => SoLoud.Sfxr_setFilter(Handle, (uint)slot, filter.Handle);

        protected override void ClearFilter(int slot)
            => SoLoud.Sfxr_setFilter(Handle, (uint)slot, IntPtr.Zero);

        protected override void SetInaudibleBehavior(bool tickWhenSilent, bool killAfterGoingSilent)
            => SoLoud.Sfxr_setInaudibleBehavior(Handle, tickWhenSilent, killAfterGoingSilent);

        protected override void SetLooping(bool looping)
            => SoLoud.Sfxr_setLooping(Handle, looping);

        protected override void SetVolume(float volume)
            => SoLoud.Sfxr_setVolume(Handle, volume);

        protected override void FreeNativeResources()
        {
            if (Handle != IntPtr.Zero)
            {
                SoLoud.Sfxr_stop(Handle);
                SoLoud.Sfxr_destroy(Handle);

                DestroyHandle();
            }
        }
    }
}