using Accord.Audio;
using Accord.Audio.Filters;
using Accord.Math;
using Accord.Math.Metrics;
using Accord.Math.Transforms;
using Chroma.Audio;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private float _a = 127;
        private RenderTarget _tgt;
        private TrueTypeFont _ttf;
        private PixelShader _pixelShader;
        private Sound _shotgun;
        private Music _analo; // thanks beasuce

        private Vector2 _screenSize;
        private Color[] _palette = new[]
        {
            Color.Red,
            Color.Black,
            Color.Gray,
            Color.DarkRed,
            Color.IndianRed,
            Color.MediumVioletRed,
            Color.OrangeRed,
            Color.PaleVioletRed,
        };

        private ushort _tw;
        private ushort _th;
        private Color[] _pixels;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;
            Window.GoWindowed(1024, 640);

            _tw = 1024 / 2;
            _th = 640 / 2;
            _pixels = new Color[_tw * _th];

            _tgt = new RenderTarget(_tw, _th)
            {
                VirtualResolution = new Vector2(1024, 640),
                FilteringMode = TextureFilteringMode.NearestNeighbor
            };

            _screenSize = new Vector2(Window.Properties.Width, Window.Properties.Height);
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/c64style.ttf", 16);
            _pixelShader = Content.Load<PixelShader>("Shaders/sh.frag");
            _shotgun = Content.Load<Sound>("Sounds/dsshotgn.wav");
            _analo = Content.Load<Music>("Music/sinetest.mp3");
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"FPS: {Window.FPS}";

            if (_fftBuffer == null)
                return;

            for (var i = 0; i < _fftBuffer.Length; i += 6)
            {
                var c = _fftBuffer[i];
                var y = GetYPosLog(c);

                _vertices.Add(new Vertex(0 + i, 320 + y));
            }
        }

        protected override void FixedUpdate(float fixedDelta)
        {

        }

        private List<Vertex> _vertices = new List<Vertex>();
        protected override void Draw(RenderContext context)
        {
            if (_fftBuffer == null)
                return;

            context.PolyLine(_vertices, Color.HotPink, false);
            _vertices.Clear();
        }

        private float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        private Vector2 Lerp(Vector2 firstVector, Vector2 secondVector, float by)
        {
            float retX = Lerp(firstVector.X, secondVector.X, by);
            float retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Vector2(retX, retY);
        }

        private float GetYPosLog(Complex c)
        {
            float intensityDB = 10 * MathF.Log10(
                MathF.Sqrt(
                    (float)(c.Real * c.Real) + 
                    (float)(c.Imaginary * c.Imaginary)
                )
            );

            float minDB = -60;
            if (intensityDB < minDB) intensityDB = minDB;
            float percent = intensityDB / minDB;

            return percent * 100f;
        }

        private Complex[] _fftBuffer;
        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _shotgun.Play();
            }
            else if (e.KeyCode == KeyCode.Q)
            {
                _analo.Play();
                Console.WriteLine(_analo.Status);
            }
            else if (e.KeyCode == KeyCode.W)
            {
                _analo.Pause();
                Console.WriteLine(_analo.Status);

            }
            else if (e.KeyCode == KeyCode.E)
            {
                _analo.Stop();
                Console.WriteLine(_analo.Status);
            }
            else if (e.KeyCode == KeyCode.R)
            {
                _analo.Dispose();
            }
            else if (e.KeyCode == KeyCode.T)
            {
                var r = new Random();
                Audio.HookPostMixProcessor<float>((chunk, bytes) =>
                {
                    var signal = new Signal(
                        bytes.ToArray(),
                        1,
                        chunk.Length,
                        Audio.SamplingRate,
                        Accord.Audio.SampleFormat.Format32BitIeeeFloat
                    );
                    var complex = signal.ToComplex();

                    _fftBuffer = complex.ToArray(1);
                    FourierTransform2.FFT(_fftBuffer, FourierTransform.Direction.Forward);
                });
            }
            else if (e.KeyCode == KeyCode.Y)
            {
                Audio.UnhookPostMixProcessor();
            }
            else if (e.KeyCode == KeyCode.Left)
            {

            }
        }
    }
}