using System;
using Chroma.Audio.Filters;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Sources
{
    public abstract class AudioSource : AudioObject
    {
        protected AudioFilter[] Filters { get; } = new AudioFilter[SoLoud.SOLOUD_MAX_FILTERS];

        internal uint VoiceHandle { get; private set; }

        public abstract bool IsLooping { get; set; }

        public abstract bool KeepTickingWhenInaudible { get; set; }
        public abstract bool KillAfterGoingInaudible { get; set; }

        public abstract double LoopingPoint { get; set; }
        public abstract double Length { get; }

        public abstract float Volume { get; set; }

        public PlaybackStatus Status { get; private set; }

        internal AudioSource(IntPtr handle) : base(handle)
        {
        }

        protected void InitializeState()
        {
            Status = PlaybackStatus.Paused;

            VoiceHandle = SoLoud.Soloud_playEx(
                AudioManager.Instance.Handle,
                Handle,
                1.0f,
                0.0f,
                true,
                0
            );
        }

        protected abstract void ApplyFilter(int slot, AudioFilter filter);
        protected abstract void ClearFilter(int slot);

        public void Play()
        {
            if (Status == PlaybackStatus.Paused)
            {
                SoLoud.Soloud_setPause(
                    AudioManager.Instance.Handle,
                    VoiceHandle,
                    false
                );
            }
            else if (Status == PlaybackStatus.Stopped)
            {
                VoiceHandle = SoLoud.Soloud_playEx(
                    AudioManager.Instance.Handle,
                    Handle,
                    1.0f,
                    0.0f,
                    false,
                    0
                );

            }
            else
            {
                Stop();
                Play();
            }
            
            Status = PlaybackStatus.Playing;
        }

        public void Pause()
        {
            if (Status == PlaybackStatus.Playing)
            {
                SoLoud.Soloud_setPause(
                    AudioManager.Instance.Handle,
                    VoiceHandle,
                    true
                );

                Status = PlaybackStatus.Paused;
            }
        }

        public void Resume()
        {
            if (Status == PlaybackStatus.Paused)
            {
                SoLoud.Soloud_setPause(
                    AudioManager.Instance.Handle,
                    VoiceHandle,
                    false
                );

                Status = PlaybackStatus.Playing;
            }
        }

        public void Stop()
        {
            SoLoud.Soloud_stop(
                AudioManager.Instance.Handle,
                VoiceHandle
            );

            Status = PlaybackStatus.Stopped;
        }

        public void SetFilter(int slot, AudioFilter filter)
        {
            if (slot >= SoLoud.SOLOUD_MAX_FILTERS || slot < 0)
                return;

            if (filter == null)
            {
                ClearFilter(slot);
                return;
            }

            if (filter.Disposed)
                throw new InvalidOperationException("Filter you're trying to apply was already disposed.");

            ApplyFilter(slot, filter);
            Filters[slot] = filter;
        }

        public T GetFilter<T>(int slot) where T : AudioFilter
        {
            if (slot >= SoLoud.SOLOUD_MAX_FILTERS || slot < 0)
                return default;

            return Filters[slot] as T;
        }
    }
}