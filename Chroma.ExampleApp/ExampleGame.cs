using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading;
using Chroma.Diagnostics;
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
            Log.Verbosity |= Verbosity.Debug;

            Window.GoWindowed(1024, 600);

            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _ttf = new TrueTypeFont(Path.Combine(loc, "c64style.ttf"), 16);
            _tex = new Texture(Path.Combine(loc, "whiterect.png"));
            _tgt = new RenderTarget(1024, 600);

            _pixelShader = new PixelShader(Path.Combine(loc, "sh.frag"));
            _screenSize = new Vector2(Window.Properties.Width, Window.Properties.Height);
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
                context.DrawString(_ttf, "WE ARE 100 PERCENT BLACK\n -> ME TOO.", new Vector2(_x, 64), (c, i, p, g) =>
                {
                    var color = _colors[i % _colors.Count];
                    var nudgeVert = 3.5f * MathF.Sin(i + _rot);

                    return new GlyphTransformData(p)
                    {
                        Color = color,
                        Position = new Vector2(p.X, p.Y + nudgeVert)
                    };
                });
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