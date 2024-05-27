using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Chroma;
using Chroma.Audio;
using Chroma.Audio.Sources;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace MusicAndSounds
{
    public class GameCore : Game
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        private Sound _doomShotgun;
        private Music _elysiumMod;
        private Waveform _waveform;

        private double[] _frequencies;

        public GameCore() : base(new(false, false))
        {
            Window.Mode.SetWindowed(new Size(800, 600));
            Audio.DeviceConnected += (_, e) =>
            {
                _log.Info(
                    $"Connected {(e.Device.IsCapture ? "input" : "output")} device {e.Device.Index}: '{e.Device.Name}'.");
            };

            Audio.DeviceDisconnected += (_, e) =>
            {
                _log.Info($"Disconnected: {e.Device.Index}");
            };

            foreach (var e in Audio.Output.Decoders)
            {
                _log.Info($"Decoder: {string.Join(',', e.SupportedFormats)}");
            }

            FixedTimeStepTarget = 75;
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void Initialize(IContentProvider content)
        {
            _doomShotgun = content.Load<Sound>("Sounds/doomsg.wav");
            _elysiumMod = content.Load<Music>("Music/elysium.mod");

            var time = 0;
            _waveform = new Waveform(
                new AudioFormat(SampleFormat.F32),
                (s, e) =>
                {
                    var floats = MemoryMarshal.Cast<byte, float>(s);
                       
                    var freq = 554.365f;
                    var amp = 0.6f;
                    var angle = MathF.PI * 2 * freq / _waveform.Frequency;
                    
                    for (var i = 0; i < floats.Length; i++)
                    {
                        floats[i] = amp * MathF.Sin(angle * time++);
                    }
                }, ChannelMode.Mono
            );
            
            RenderSettings.LineThickness = 1;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to resume/pause the groovy music ({_elysiumMod.Status}) [{_elysiumMod.Position:F3}s / {_elysiumMod.Duration:F3}s].\n" +
                "Use <F2> to stop the groovy music.\n" +
                "Use Ctrl+F1 to skip 10 seconds of groovy music.\n" + 
                $"Use <space> to play the shotgun sound. ({_doomShotgun.Status}) [{_doomShotgun.Position:F3}s / {_doomShotgun.Duration:F3}s]\n" +
                $"Use <F3>/<F4> to tweak the shotgun sound volume -/+ ({_doomShotgun.Volume}).\n" +
                $"Use <F5>/<F6> to tweak master volume -/+ ({Audio.Output.MasterVolume}).\n" +
                $"Use <F7> to play/pause the sine waveform.",
                new Vector2(8)
            );

            if (_frequencies == null)
                return;

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

            for (var i = 0; i < _frequencies.Length / 4; i ++)
            {
                context.Line(
                    new Vector2(
                        (2 + i) * (2 + RenderSettings.LineThickness - 1),
                        Window.Height
                    ),
                    new Vector2(
                        (2 + i) * (2 + RenderSettings.LineThickness - 1),
                        Window.Height - 1 - (float)_frequencies[i] * 768
                    ),
                    new Color(
                        i / ((float)_frequencies.Length / 4), 
                        1f - i / ((float)_frequencies.Length / 4), 
                        1f, 
                        1f
                    )
                );
            }
        }

        protected override void FixedUpdate(float delta)
        {
            DoFFT(_elysiumMod.InBuffer, _elysiumMod.Format);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                {
                    var skip = 10;
                    
                    if (e.Modifiers.HasFlag(KeyModifiers.LeftControl))
                    {
                        if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                            skip *= -1;
                        
                        _elysiumMod.Seek(skip, SeekOrigin.Current);
                    }
                    else
                    {
                        if (_elysiumMod.IsPlaying)
                            _elysiumMod.Pause();
                        else
                            _elysiumMod.Play();
                    }

                    break;
                }

                case KeyCode.F2:
                    _elysiumMod.Stop();
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
                    Audio.Output.MasterVolume -= 0.1f;
                    break;

                case KeyCode.F6:
                    Audio.Output.MasterVolume += 0.1f;
                    break;

                case KeyCode.F7:
                    if (_waveform.IsPlaying)
                        _waveform.Pause();
                    else
                        _waveform.Play();
                    break;
                
                case KeyCode.F8:
                    _waveform.Dispose();
                    _doomShotgun.Dispose();
                    _elysiumMod.Dispose();
                    
                    Audio.Output.Close();
                    Audio.Output.Open();
                    
                    Initialize(Content);
                    
                    break;
                
                case KeyCode.F9:
                    _waveform.Dispose();
                    break;
            }
        }

        private void DoFFT(Span<byte> audioBufferData, AudioFormat format)
        {
            float[] chunk;

            if (format.SampleFormat != SampleFormat.F32)
            {
                var shortSamples = MemoryMarshal.Cast<byte, short>(audioBufferData);

                chunk = new float[2048];

                for (var i = 0; i < shortSamples.Length; i++)
                {
                    if (i >= chunk.Length)
                        break;

                    chunk[i] = shortSamples[i] / 32767f;
                }
            }
            else
            {
                chunk = MemoryMarshal.Cast<byte, float>(audioBufferData).ToArray();
            }

            _frequencies = FftSharp.Transform.FFTmagnitude(chunk.Select(x => (double)x).ToArray());
        }
    }
}