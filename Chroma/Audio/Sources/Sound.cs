using System;
using System.IO;
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
            TryInitialize(
                () => SoLoud.Wav_load(Handle, filePath),
                "Failed to load sound from file"
            );
        }

        public Sound(Stream stream)
            : base(SoLoud.Wav_create())
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
                            fixed (byte* data = &bytes[0])
                            {
                                return SoLoud.Wav_loadMemEx(
                                    Handle,
                                    new IntPtr(data),
                                    (uint)bytes.Length,
                                    true,
                                    true
                                );
                            }
                        }
                    }
                },
                "Failed to load sound from memory"
            );
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