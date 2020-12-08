using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma;
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

        // private Sound _doomShotgun;
        // private Music _groovyMusic;
        private Music _elysiumMod;

        private float[] _averaged = new float[512];
        private float[] _result = new float[2048];
        private Complex[] _samples = new Complex[4096];

        public GameCore()
        {
            Content = new FileSystemContentProvider(Path.Combine(LocationOnDisk, "../../../../_common"));

            Window.GoWindowed(new Size(800, 600));
            Audio.DeviceConnected += (sender, e) =>
            {
                _log.Info($"Connected {(e.Device.IsCapture ? "input" : "output")} device {e.Device.IsCapture}: '{e.Device.Name}'.");
            };
            //
            // Audio.HookPostMixProcessor<float>(FftPostMixProcessor);
        }

        protected override void LoadContent()
        {
            // _doomShotgun = Content.Load<Sound>("Sounds/doomsg.wav");
            // _groovyMusic = Content.Load<Music>("Music/groovy.mp3");
            _elysiumMod = Content.Load<Music>("Music/elysium.mod");
        }

        protected override void Draw(RenderContext context)
        {
            // context.DrawString(
            //     $"Use <F1> to start/stop the groovy music ({_groovyMusic.Status}).\n" +
            //     "Use <F2> to pause/unpause the groovy music.\n" +
            //     $"Use <space> to play the shotgun sound. ({_doomShotgun.Status})\n" +
            //     $"Use <F3>/<F4> to tweak the shotgun sound volume -/+ ({_doomShotgun.Volume}).\n" +
            //     $"Use <F5>/<F6> to tweak the groovy music volume -/+ ({Audio.MusicVolume}).",
            //     new Vector2(8)
            // );

            context.LineThickness = 4;

            var upBeat = 128 * ((_averaged[1] + _averaged[2]));
            
            context.Rectangle(
                ShapeMode.Fill,
                Window.Center - new Vector2(0, upBeat),
                new Size(32, 32),
                new Color(0, (byte)(upBeat % 255), 125)
            );
            
            for (var i = 0; i < _averaged.Length / 8; i++)
            {
                context.Line(
                    new Vector2(
                        (2 + i) * (2 + context.LineThickness - 1) + Window.Center.X / 2, 
                        Window.Size.Height
                    ),
                    new Vector2(
                        (2 + i) * (2 + context.LineThickness - 1) + Window.Center.X / 2, 
                        Window.Size.Height - 1 - _averaged[i] * 768
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
                    if (_elysiumMod.IsPlaying)
                        _elysiumMod.Pause();
                    else
                        _elysiumMod.Play();
                    break;
                //
                // case KeyCode.F2:
                //     if (_elysiumMod.Status == PlaybackStatus.Playing)
                //         _elysiumMod.Pause();
                //     else if (_elysiumMod.Status == PlaybackStatus.Paused)
                //         _elysiumMod.Play();
                //     break;
                //
                // case KeyCode.Space:
                //     _doomShotgun.PlayOneShot();
                //     break;
                //
                // case KeyCode.F3:
                //     _doomShotgun.Volume--;
                //     break;
                //
                // case KeyCode.F4:
                //     _doomShotgun.Volume++;
                //     break;
                //
                // case KeyCode.F5:
                //     Audio.MusicVolume--;
                //     break;
                //
                // case KeyCode.F6:
                //     Audio.MusicVolume++;
                //     break;
            }
        }

        private void FftPostMixProcessor(Span<float> chunk, Span<byte> bytes)
        {
            var spec = 0;
            for (var i = 0; i < chunk.Length; i += 2)
            {
                _samples[spec++] = new Complex((chunk[i] + chunk[i + 1]) / 2f, 0);
            }

            FFT.CalculateFFT(_samples, _result);

            _averaged = new float[512];
            for (var i = 0; i < _result.Length; i++)
            {
                _averaged[i / 4] += _result[i];

                if (i != 0 && i % 4 == 0)
                {
                    _averaged[i / 4] /= 4;
                }
            }
        }
    }
}