using System;
using Chroma.Audio.Filters;
using Chroma.Diagnostics.Logging;
using Chroma.Natives.SoLoud;

namespace Chroma.Audio.Sources
{
    public abstract class AudioSource : AudioObject
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        protected AudioFilter[] Filters { get; } = new AudioFilter[SoLoud.SOLOUD_MAX_FILTERS];

        internal uint VoiceHandle { get; private set; }

        internal bool VoiceHandleValid => SoLoud.Soloud_isValidVoiceHandle(
            AudioManager.Instance.Handle,
            VoiceHandle
        );

        public abstract bool IsLooping { get; set; }

        public abstract bool KeepTickingWhenInaudible { get; set; }
        public abstract bool KillAfterGoingInaudible { get; set; }

        public abstract double LoopingPoint { get; set; }
        
        public abstract bool SupportsLength { get; }
        public abstract double Length { get; }

        public abstract float Volume { get; set; }

        public PlaybackStatus Status
        {
            get
            {
                if (!SoLoud.Soloud_isValidVoiceHandle(AudioManager.Instance.Handle, VoiceHandle))
                    return PlaybackStatus.Stopped;
                
                if (SoLoud.Soloud_getPause(AudioManager.Instance.Handle, VoiceHandle))
                    return PlaybackStatus.Paused;

                return PlaybackStatus.Playing;
            }
        }

        public float Panning
        {
            get => SoLoud.Soloud_getPan(
                AudioManager.Instance.Handle,
                VoiceHandle
            );

            set => SoLoud.Soloud_setPan(
                AudioManager.Instance.Handle,
                VoiceHandle,
                value
            );
        }

        public double PositionSeconds => SoLoud.Soloud_getStreamTime(
            AudioManager.Instance.Handle,
            VoiceHandle
        );

        public double Position => SoLoud.Soloud_getStreamPosition(
            AudioManager.Instance.Handle,
            VoiceHandle
        );

        public int LoopCount => (int)SoLoud.Soloud_getLoopCount(
            Handle,
            VoiceHandle
        );
        
        internal AudioSource(IntPtr handle) : base(handle)
        {
        }

        protected void InitializeState()
        {
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

        public void FadeVolume(float targetValue, double fadeSeconds)
        {
            SoLoud.Soloud_fadeVolume(
                AudioManager.Instance.Handle,
                VoiceHandle,
                targetValue,
                fadeSeconds
            );
        }

        public void ScheduleStop(double secondsFromNow)
        {
            SoLoud.Soloud_scheduleStop(
                AudioManager.Instance.Handle,
                VoiceHandle,
                secondsFromNow
            );
        }

        public void SchedulePause(double secondsFromNow)
        {
            SoLoud.Soloud_schedulePause(
                AudioManager.Instance.Handle,
                VoiceHandle,
                secondsFromNow
            );
        }
        
        public void FadePan(float targetValue, double fadeSeconds)
        {
            SoLoud.Soloud_fadePan(
                AudioManager.Instance.Handle,
                VoiceHandle,
                targetValue,
                fadeSeconds
            );
        }

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
            }
        }

        public void Stop()
        {
            SoLoud.Soloud_stop(
                AudioManager.Instance.Handle,
                VoiceHandle
            );
        }

        public void Seek(double position)
        {
            var error = SoLoud.Soloud_seek(
                AudioManager.Instance.Handle,
                VoiceHandle,
                position
            );

            if (error > 0)
            {
                _log.Error(
                    $"Failed to seek to '{position}': " +
                    $"{SoLoud.Soloud_getErrorString(AudioManager.Instance.Handle, error)}"
                );
            }
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