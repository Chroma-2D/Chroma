using System;

namespace Chroma.Audio
{
    public class Music : AudioSource
    {
        public override byte Volume
        {
            get => AudioManager.MusicVolume;
            set => AudioManager.MusicVolume = value;
        }

        internal Music(IntPtr handle, AudioManager audioManager) : base(handle, audioManager)
        { }

        public override void Play()
        {
            EnsureNotDisposed();

            Status = AudioManager.BeginMusicPlayback(this);
        }

        public override void Pause()
        {
            EnsureNotDisposed();

            AudioManager.PauseMusicPlayback(this);
            Status = PlaybackStatus.Paused;
        }

        public override void Stop()
        {
            EnsureNotDisposed();

            AudioManager.StopMusicPlayback(this);
            Status = PlaybackStatus.Stopped;
        }
    }
}
