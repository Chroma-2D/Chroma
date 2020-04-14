using System.Collections.Generic;
using System.Geometry;
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
        private RenderTarget _tgt;
        private TrueTypeFont _ttf;
        private PixelShader _pixelShader;
        private VGA _vga;

        private Vector2 _screenSize;

        private List<Color> _colors = new List<Color>
        {
            Color.Red,
            Color.Orange,
            Color.Green,
            Color.CornflowerBlue,
            Color.Indigo,
            Color.Violet
        };

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;
            Window.GoWindowed(1024, 640);

            _tgt = new RenderTarget(1024, 640);
            _screenSize = new Vector2(Window.Properties.Width, Window.Properties.Height);

            _vga = new VGA(Window, _ttf);

            for (var y = 0; y < _vga.MaxRows; y++)
            {
                for (var x = 0; x < _vga.MaxCols; x++)
                {
                    _vga.SetColorAt(x, y, _colors[x % _colors.Count]);
                    _vga.SetCharAt(x, y, 'C');
                }
            }
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/c64style.ttf", 16);
            _pixelShader = Content.Load<PixelShader>("Shaders/sh.frag");
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);
                _vga.Render(context);
            });

            _pixelShader.Activate();
            _pixelShader.SetUniform("screenSize", _screenSize);
            _pixelShader.SetUniform("scanlineDensity", 2f);
            _pixelShader.SetUniform("blurDistance", .88f);

            context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            context.DeactivateShader();
        }
    }
}