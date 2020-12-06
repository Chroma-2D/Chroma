using System;
using System.Drawing;
using System.IO;
using System.Numerics;
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

        private float[] averaged = new float[512];
        private float[] result = new float[2048];
        private Complex[] spec2 = new Complex[4096];

        public GameCore()
        {
            Content = new FileSystemContentProvider(Path.Combine(LocationOnDisk, "../../../../_common"));

            Window.GoWindowed(new Size(800, 600));
            Audio.AudioDeviceConnected += (sender, e) =>
            {
                _log.Info($"Connected {(e.IsCapture ? "input" : "output")} device {e.Index}: '{e.Name}'.");
            };

            Audio.HookPostMixProcessor<float>(FftPostMixProcessor);
        }

        protected override void LoadContent()
        {
            _doomShotgun = Content.Load<Sound>("Sounds/doomsg.wav");
            _groovyMusic = Content.Load<Music>("Music/groovy.mp3");
            _elysiumMod = Content.Load<Music>("Music/elysium.mod");
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                $"Use <F1> to start/stop the groovy music ({_groovyMusic.Status}).\n" +
                "Use <F2> to pause/unpause the groovy music.\n" +
                $"Use <space> to play the shotgun sound. ({_doomShotgun.Status})\n" +
                $"Use <F3>/<F4> to tweak the shotgun sound volume -/+ ({_doomShotgun.Volume}).\n" +
                $"Use <F5>/<F6> to tweak the groovy music volume -/+ ({Audio.MusicVolume}).",
                new Vector2(8)
            );

            context.LineThickness = 2;

            for (var i = 0; i < averaged.Length; i++)
            {
                context.Line(
                    new Vector2(2 + i * (2 + context.LineThickness - 1), Window.Size.Height),
                    new Vector2(2 + i * (2 + context.LineThickness - 1), Window.Size.Height - 1 - averaged[i] * 1024),
                    new Color((byte)(255f * (averaged.Length / (float)i)), 55, (255 / (averaged[i] * 1024)) % 255)
                );
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    if (_elysiumMod.Status == PlaybackStatus.Playing || _elysiumMod.Status == PlaybackStatus.Paused)
                        _elysiumMod.Stop();
                    else
                        _elysiumMod.Play();
                    break;

                case KeyCode.F2:
                    if (_elysiumMod.Status == PlaybackStatus.Playing)
                        _elysiumMod.Pause();
                    else if (_elysiumMod.Status == PlaybackStatus.Paused)
                        _elysiumMod.Play();
                    break;

                case KeyCode.Space:
                    _doomShotgun.PlayOneShot();
                    break;

                case KeyCode.F3:
                    _doomShotgun.Volume--;
                    break;

                case KeyCode.F4:
                    _doomShotgun.Volume++;
                    break;

                case KeyCode.F5:
                    Audio.MusicVolume--;
                    break;

                case KeyCode.F6:
                    Audio.MusicVolume++;
                    break;
            }
        }

        private void FftPostMixProcessor(Span<float> chunk, Span<byte> bytes)
        {
            var spec = 0;
            for (var i = 0; i < chunk.Length; i += 2)
            {
                spec2[spec++] = new Complex((chunk[i] + chunk[i + 1]) / 2f, 0);
            }

            FFT.CalculateFFT(spec2, result, false);

            averaged = new float[256];
            for (var i = 0; i < result.Length; i++)
            {
                averaged[i / 8] += result[i];

                if (i != 0 && i % 8 == 0)
                {
                    averaged[i / 8] /= 8;
                }
            }
        }
    }
}