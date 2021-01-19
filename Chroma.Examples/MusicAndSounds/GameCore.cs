using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Chroma;
using Chroma.Audio;
using Chroma.Audio.Sources;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace MusicAndSounds
{
    public class GameCore : Game
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        private Sound _doomShotgun;
        private Music _groovyMusic;
        private Music _elysiumMod;
        private Waveform _waveform;

        private double[] _frequencies;

        public GameCore()
        {
            Content = new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
            
            Window.GoWindowed(new Size(800, 600));
            Audio.DeviceConnected += (_, e) =>
            {
                _log.Info(
                    $"Connected {(e.Device.IsCapture ? "input" : "output")} device {e.Device.Index}: '{e.Device.Name}'.");
            };

            foreach (var e in Audio.Decoders)
            {
                _log.Info($"Decoder: {string.Join(',', e.SupportedFormats)}");
            }

            FixedTimeStepTarget = 75;
        }

        protected override void LoadContent()
        {
            _doomShotgun = Content.Load<Sound>("Sounds/doomsg.wav");

            _elysiumMod = Content.Load<Music>("Music/elysium.mod");

            _groovyMusic = Content.Load<Music>("Music/groovy.mp3");

            _waveform = new Waveform(
                new AudioFormat(SampleFormat.F32),
                (s, _) =>
                {
                    var floats = MemoryMarshal.Cast<byte, float>(s);

                    for (var i = 0; i < floats.Length; i++)
                    {
                        floats[i] = MathF.Sin(2 * MathF.PI * 13f * (i / (float)floats.Length));
                    }
                }, ChannelMode.Mono
            );
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to start/stop the groovy music ({_groovyMusic.Status}).\n" +
                "Use <F2> to pause/unpause the groovy music.\n" +
                $"Use <space> to play the shotgun sound. ({_doomShotgun.Status})\n" +
                $"Use <F3>/<F4> to tweak the shotgun sound volume -/+ ({_doomShotgun.Volume}).\n" +
                $"Use <F5>/<F6> to tweak master volume -/+ ({Audio.MasterVolume}).\n" +
                $"Use <F7> to play/pause the sine waveform.",
                new Vector2(8)
            );

            if (_frequencies == null)
                return;

            context.LineThickness = 1;

            var upBeat = 128 * (_frequencies[0] + _frequencies[1] + _frequencies[2] + _frequencies[3]);
            var upBeat2 = 128 * (_frequencies[4] + _frequencies[5] + _frequencies[6] + _frequencies[7]);
            var upBeat3 = 128 * (_frequencies[8] + _frequencies[9] + _frequencies[10] + _frequencies[11]);

            context.Rectangle(
                ShapeMode.Fill,
                Window.Center - new Vector2(0, (float)upBeat),
                new Size(32, 32),
                new Color(0, (byte)(upBeat % 255), 200)
            );

            context.Rectangle(
                ShapeMode.Fill,
                Window.Center - new Vector2(32, 0 + (float)upBeat2),
                new Size(32, 32),
                new Color(0, 200, (byte)(upBeat2 % 255))
            );
            
            context.Rectangle(
                ShapeMode.Fill,
                Window.Center - new Vector2(64, 0 + (float)upBeat3),
                new Size(32, 32),
                new Color(200, 0, (byte)(upBeat3 % 255))
            );

            for (var i = 0; i < _frequencies.Length / 2; i++)
            {
                context.Line(
                    new Vector2(
                        (2 + i) * (2 + context.LineThickness - 1),
                        Window.Size.Height
                    ),
                    new Vector2(
                        (2 + i) * (2 + context.LineThickness - 1),
                        Window.Size.Height - 1 - (float)_frequencies[i] * 768
                    ),
                    new Color((byte)(255f * (_frequencies.Length / (float)i)), 55,
                        (255 / ((float)_frequencies[i] * 768)) % 255)
                );
            }
        }

        protected override void FixedUpdate(float delta)
        {
            DoFFT(_groovyMusic.InBuffer, _groovyMusic.Format);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    if (_groovyMusic.IsPlaying)
                        _groovyMusic.Pause();
                    else
                        _groovyMusic.Play();
                    break;

                case KeyCode.F2:
                    _groovyMusic.Stop();
                    break;

                case KeyCode.Space:
                    _doomShotgun.Play();
                    break;

                case KeyCode.F3:
                    _doomShotgun.Volume -= 0.1f;
                    break;

                case KeyCode.F4:
                    _doomShotgun.Volume += 0.1f;
                    break;

                case KeyCode.F5:
                    Audio.MasterVolume -= 0.1f;
                    break;

                case KeyCode.F6:
                    Audio.MasterVolume += 0.1f;
                    break;

                case KeyCode.F7:
                    if (_waveform.IsPlaying)
                        _waveform.Pause();
                    else
                        _waveform.Play();
                    break;
            }
        }

        private void DoFFT(Span<byte> audioBufferData, AudioFormat format)
        {
            float[] chunk;

            if (format.SampleFormat != SampleFormat.F32)
            {
                var shortSamples = MemoryMarshal.Cast<byte, short>(audioBufferData);
                chunk = new float[shortSamples.Length];

                for (var i = 0; i < shortSamples.Length; i++)
                    chunk[i] = shortSamples[i] / 32767f;
            }
            else
            {
                chunk = MemoryMarshal.Cast<byte, float>(audioBufferData).ToArray();
            }

            _frequencies = FftSharp.Transform.FFTmagnitude(chunk.Select(x => (double)x).ToArray());
        }
    }
}