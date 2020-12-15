using System;
using System.Drawing;
using System.IO;
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

        private float[] _averaged = new float[512];
        private float[] _result = new float[1024];
        private Complex[] _samples = new Complex[2048];

        public GameCore()
        {
            Content = new FileSystemContentProvider(Path.Combine(LocationOnDisk, "../../../../_common"));

            Window.GoWindowed(new Size(800, 600));
            Audio.DeviceConnected += (sender, e) =>
            {
                _log.Info(
                    $"Connected {(e.Device.IsCapture ? "input" : "output")} device {e.Device.Index}: '{e.Device.Name}'.");
            };
        }

        protected override void LoadContent()
        {
            _doomShotgun = Content.Load<Sound>("Sounds/doomsg.wav");

            _elysiumMod = Content.Load<Music>("Music/elysium.mod");

            _groovyMusic = Content.Load<Music>("Music/groovy.mp3");
            _groovyMusic.Filters.Add(DoFFT);

            _waveform = new Waveform(
                new AudioFormat(SampleFormat.F32),
                (s, f) =>
                {
                    var floats = MemoryMarshal.Cast<byte, float>(s);
                    
                    for(var i = 0; i < floats.Length; i++)
                    {
                        floats[i] = MathF.Sin(2 * MathF.PI * 441f * (i / 44100f));
                    }
                }
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

            context.LineThickness = 1;

            var upBeat = 128 * (_averaged[1] + _averaged[2]);
            var upBeat2 = 128 * (_averaged[5] + _averaged[6] + _averaged[7]);

            context.Rectangle(
                ShapeMode.Fill,
                Window.Center - new Vector2(0, upBeat),
                new Size(32, 32),
                new Color(0, (byte)(upBeat % 255), 200)
            );

            context.Rectangle(
                ShapeMode.Fill,
                Window.Center - new Vector2(0, -32 + upBeat2),
                new Size(32, 32),
                new Color(0, 200, (byte)(upBeat % 255))
            );

            for (var i = 0; i < _averaged.Length / 2; i++)
            {
                context.Line(
                    new Vector2(
                        (2 + i) * (2 + context.LineThickness - 1),
                        Window.Size.Height
                    ),
                    new Vector2(
                        (2 + i) * (2 + context.LineThickness - 1),
                        Window.Size.Height - 1 - _averaged[i] * 1024
                    ),
                    new Color((byte)(255f * (_averaged.Length / (float)i)), 55, (255 / (_averaged[i] * 1024)) % 255)
                );
            }
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

            var spec = 0;
            for (var i = 0; i < chunk.Length; i += 2)
                _samples[spec++] = new Complex((chunk[i] + chunk[i + 1]) / 2f, 0);

            FFT.CalculateFFT(_samples, _result);

            _averaged = new float[512];
            for (var i = 0; i < _result.Length; i++)
            {
                _averaged[i / 2] += _result[i];

                if (i != 0 && i % 2 == 0)
                    _averaged[i / 2] /= 2;
            }
        }
    }
}