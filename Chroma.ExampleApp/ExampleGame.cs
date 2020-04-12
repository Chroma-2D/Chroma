using System;
using System.Collections.Generic;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Texture _tex;
        private RenderTarget _tgt;
        private TrueTypeFont _ttf;

        private List<Color> _colors = new List<Color>
        {
            Color.Red,
            Color.Orange,
            Color.Lime,
            Color.CornflowerBlue,
            Color.Indigo,
            Color.Violet
        };

        private PixelShader _pixelShader;

        private Vector2 _screenSize;
        private float _rot = 0.0f;
        private float _x = 0f;
        private bool _goUp = true;

        private bool _doot = true;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;
            Window.GoWindowed(1024, 600);

            _tgt = new RenderTarget(1024, 600);
            _screenSize = new Vector2(Window.Properties.Width, Window.Properties.Height);
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/c64style.ttf", 16);
            _tex = Content.Load<Texture>("Textures/burg.png");
            _pixelShader = Content.Load<PixelShader>("Shaders/sh.frag");
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";
        }

        protected override void FixedUpdate(float fixedDelta)
        {
            if (_goUp)
            {
                _x += 60f * fixedDelta;
                if (_x >= 100)
                    _goUp = false;
            }
            else
            {
                _x += -60f * fixedDelta;

                if (_x < 0)
                    _goUp = true;
            }

            _rot += 10f * fixedDelta;
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);

                var measure = _ttf.Measure(".,,,,,-----");
                context.Rectangle(ShapeMode.Stroke, new Vector2(64, 64), measure.X, measure.Y, Color.Red);

                context.DrawString(_ttf, ".,,,,,-----", new Vector2(64, 64), (c, i, p, g) =>
                {
                    var color = _colors[i % _colors.Count];
                    var nudgeVert = 3.5f * MathF.Sin(i + _rot);

                    return new GlyphTransformData(p)
                    {
                        Color = color,
                        // Position = new Vector2(p.X, p.Y + nudgeVert)
                    };
                });

                context.DrawTexture(_tex, new Vector2(50, 150), Vector2.One, Vector2.Zero, _rot);
                context.Rectangle(ShapeMode.Fill, new Vector2(150, 150), 100, 100, Color.Red);
            });

            if (_doot)
            {
                _pixelShader.Activate();
                _pixelShader.SetUniform("screenSize", _screenSize);
                _pixelShader.SetUniform("scanlineDensity", 2f);
                _pixelShader.SetUniform("blurDistance", .88f);
            }

            context.DrawTexture(_tgt.Texture, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);

            if (_doot)
            {
                context.DeactivateShader();
            }
            //context.Batch(() => context.DrawTexture(_tex, new Vector2(128, 128), Vector2.One, Vector2.Zero, .0f), 1);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
                _tex.VirtualResolution = new Vector2(256, 256);
            else if (e.KeyCode == KeyCode.F2)
                _tex.VirtualResolution = null;
            else if (e.KeyCode == KeyCode.F3)
                _tex.FilteringMode = TextureFilteringMode.LinearMipmapped;
            else if (e.KeyCode == KeyCode.F4)
                _doot = !_doot;
        }
    }
}