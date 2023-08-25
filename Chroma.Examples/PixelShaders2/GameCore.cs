using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Chroma;
using Chroma.Audio;
using Chroma.Audio.Sources;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input;
using FftSharp;
using FftSharp.Windows;

namespace PixelShaders2
{
    public class GameCore : Game
    {
        private Effect _effect = null!;
        private RenderTarget _target = null!;
        private Music _music = null!;

        private double[] _frequencies = new double[2048];
        private double[] _lowFrequencies = new double[0];
        private double[] _midFrequencies = new double[0];
        private double[] _highFrequencies = new double[0];

        public GameCore()
            : base(new(false, false))
        {
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            _effect = Content.Load<Effect>("Shaders/glowyrings.frag");
            _music = Content.Load<Music>("Music/groovy.mp3");
            _music.Filters.Add(FftTunnel);
            _music.IsLooping = true;
            _music.Play();

            _target = new RenderTarget(Window.Size);
        }

        protected override void Update(float delta)
        {
            Window.Title = $"{_music.Position}/{_music.Duration}";
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Left)
            {
                _music.Seek(-10, SeekOrigin.Current);
            }
            else if (e.KeyCode == KeyCode.Right)
            {
                _music.Seek(10, SeekOrigin.Current);
            }
            else if (e.KeyCode == KeyCode.Space)
            {
                if (_music.Status == PlaybackStatus.Playing)
                {
                    _music.Pause();   
                }
                else
                {
                    _music.Play();
                }
            }
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_target, (ctx, tgt) => { ctx.Clear(Color.Black); });

            var mp = Mouse.WindowSpacePosition;
            var dx = Window.Center.X - mp.X;
            var dy = Window.Center.Y - mp.Y;

            _effect.Activate();
            _effect.SetUniform(
                "mouse_pos",
                new Vector2(dx / Window.Width, dy / Window.Height)
            );

            if (_lowFrequencies.Any())
            {
                _effect.SetUniform(
                    "low_fft",
                    (float)_lowFrequencies.Select(Math.Abs).Average() * 1000f
                );
            }

            if (_midFrequencies.Any())
            {
                _effect.SetUniform(
                    "mid_fft",
                    (float)_midFrequencies.Select(Math.Abs).Average() * 1000f
                );
            }
            
            if (_highFrequencies.Any())
            {
                _effect.SetUniform(
                    "high_fft",
                    (float)_highFrequencies.Select(Math.Abs).Average() * 1000f
                );
            }

            context.DrawTexture(_target, Vector2.Zero);
            Shader.Deactivate();
        }

        private void FftTunnel(Span<byte> audioBufferData, AudioFormat format)
        {
            float[] chunk;

            if (format.SampleFormat != SampleFormat.F32)
            {
                var shortSamples = MemoryMarshal.Cast<byte, short>(audioBufferData);

                chunk = new float[shortSamples.Length];

                for (var i = 0; i < shortSamples.Length; i++)
                {
                    chunk[i] = shortSamples[i] / 32767f;
                }
            }
            else
            {
                chunk = MemoryMarshal.Cast<byte, float>(audioBufferData).ToArray();
            }

            var values = FFT.Forward(chunk.Select(x => (double)x).ToArray());

            lock (_frequencies)
            {
                if (!_frequencies.Any())
                {
                    _frequencies = FFT.Magnitude(values);
                }
                else
                {
                    var freqs = FFT.Magnitude(values);

                    for (var i = 0; i < _frequencies.Length; i++)
                    {
                        _frequencies[i] += freqs[i / 2];
                        _frequencies[i] /= 2;
                    }
                }
            }

            lock (_lowFrequencies)
            {
                if (!_lowFrequencies.Any())
                {
                    _lowFrequencies = Filter.BandPass(_frequencies, 44100, 40, 110);
                }
                else
                {
                    var freqs = Filter.BandPass(_frequencies, 44100, 40, 110);

                    for (var i = 0; i < _lowFrequencies.Length; i++)
                    {
                        _lowFrequencies[i] += freqs[i];
                        _lowFrequencies[i] /= 2;
                    }
                }
            }
            
            lock (_midFrequencies)
            {
                if (!_midFrequencies.Any())
                {
                    _midFrequencies = Filter.BandPass(_frequencies, 44100, 800, 1000);
                }
                else
                {
                    var freqs = Filter.BandPass(_frequencies, 44100, 800, 1000);

                    for (var i = 0; i < _midFrequencies.Length; i++)
                    {
                        _midFrequencies[i] += freqs[i];
                        _midFrequencies[i] /= 2;
                    }
                }
            }

            lock (_highFrequencies)
            {
                if (!_highFrequencies.Any())
                {
                    _highFrequencies = Filter.BandPass(_frequencies, 44100, 1400, 3000);
                }
                else
                {
                    var freqs = Filter.BandPass(_frequencies, 44100, 1400, 3000);

                    for (var i = 0; i < _highFrequencies.Length; i++)
                    {
                        _highFrequencies[i] += freqs[i];
                        _highFrequencies[i] /= 2;
                    }
                }
            }
        }
    }
}