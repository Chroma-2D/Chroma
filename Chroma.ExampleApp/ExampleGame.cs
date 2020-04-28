using System;
using System.Collections.Generic;
using System.Numerics;
using Chroma.Audio;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private float _a = 127;
        private RenderTarget _tgt;
        private TrueTypeFont _ttf;
        private PixelShader _pixelShader;
        private VGA _vga;
        private AudioClip _shotgun;

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
            _shotgun = Content.Load<AudioClip>("Sounds/dsshotgn.wav");
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"FPS: {Window.FPS}";
        }

        protected override void FixedUpdate(float fixedDelta)
        {
            _a += 0.0025f;

            float a2 = _a * 2f;
            for (int x = 0; x < _tw - 2; x += 2)
            {
                float x2 = x / 3184f;
                for (int y = 0; y < _th - 2; y += 2)
                {
                    float y2 = y / 2048f;
                    float v1 = 256f + 192f * MathF.Sin(y2 + a2);
                    float v2 = MathF.Sin(_a - x2 + y2 * 2);

                    float r = 6 * MathF.Cos(a2 + x / v1 + v2);
                    float g = 6 * MathF.Sin((x + y) / v1 * v2);
                    float b = 6 * MathF.Cos((x * v2 - y) / v1);

                    var c1 = y * _tw + x;
                    var c2 = y * _tw + (x + 1);
                    var c3 = (y + 1) * _tw + x;
                    var c4 = (y + 1) * _tw + (x + 1);

                    ref var p1 = ref _pixels[c1];
                    ref var p2 = ref _pixels[c2];
                    ref var p3 = ref _pixels[c3];
                    ref var p4 = ref _pixels[c4];

                    p1.R = (byte)(_palette[(int)MathF.Abs(MathF.Floor(r))].R * b);
                    p1.G = (byte)(_palette[(int)MathF.Abs(MathF.Floor(r))].B - g);
                    p1.B = (byte)(_palette[(int)MathF.Abs(MathF.Floor(r))].G + r);
                    p1.A = 255;

                    p2.R = (byte)(_palette[(int)MathF.Abs(MathF.Floor(g))].G - p1.G);
                    p2.G = (byte)(_palette[(int)MathF.Abs(MathF.Floor(g))].B + p1.R);
                    p2.B = _palette[(int)MathF.Abs(MathF.Floor(g))].R;
                    p2.A = 255;

                    p3.R = _palette[(int)MathF.Abs(MathF.Floor(b))].B;
                    p3.G = _palette[(int)MathF.Abs(MathF.Floor(b))].G;
                    p3.B = _palette[(int)MathF.Abs(MathF.Floor(b))].R;
                    p3.A = 255;
                }
            }
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);

                for(var x = 0; x < _tw; x++)
                {
                    for(var y = 0; y < _th; y++)
                    {
                        _tgt.SetPixel(x, y, _pixels[y * _tw + x]);
                    }
                }
                _tgt.Flush();
            });

            //_pixelShader.Activate();
            //_pixelShader.SetUniform("screenSize", _screenSize);
            //_pixelShader.SetUniform("scanlineDensity", 2f);
            //_pixelShader.SetUniform("blurDistance", .88f);

            context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            context.DeactivateShader();
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _shotgun.Play(5);
            }
        }
    }
}