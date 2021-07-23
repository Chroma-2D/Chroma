using System;
using System.IO;
using Chroma;
using Chroma.Audio;
using Chroma.Audio.Captures;
using Chroma.Audio.Sources;
using Chroma.Graphics;
using Chroma.Input;

namespace AudioRecording
{
    public class GameCore : Game
    {
        private AudioCapture _recording;
        private MemoryStream _stream = new();

        private Waveform _waveform;

        private byte[] _data;
        private int _dataPointer;

        public GameCore() : base(new(false, false))
        {
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "Press <F1> to start recording.\n" +
                "Press <F2> to stop the recording.\n" +
                "Press <F3> to pause/resume the recording.\n" +
                "Press <F4> to play the recorded sample back.",
                new(8)
            );

            if (_recording != null)
            {
                Window.Title = $"{_recording.Status} | {_recording.TotalSize}";
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                if (_recording != null)
                    _recording.Dispose();

                _recording = new BufferedAudioCapture(_stream);
                _recording.Start();
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                if (_recording == null)
                    return;

                _recording.Stop();
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                if (_recording == null)
                    return;

                if (_recording.Status == CaptureStatus.Paused)
                    _recording.Resume();
                else if (_recording.Status == CaptureStatus.Recording)
                    _recording.Pause();
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                if (_recording.Status != CaptureStatus.Stopped)
                    _recording.Stop();

                if (_waveform != null)
                    _waveform.Dispose();

                _data = _stream.GetBuffer();

                _waveform = new Waveform(
                    _recording.Format,
                    PlaybackDelegate,
                    _recording.ChannelMode
                );

                _waveform.Play();
            }
        }

        private void PlaybackDelegate(Span<byte> data, AudioFormat format)
        {
            for (var i = 0; i < data.Length && _dataPointer < _data.Length; i++, _dataPointer++)
            {
                data[i] = _data[_dataPointer];
            }
        }
    }
}