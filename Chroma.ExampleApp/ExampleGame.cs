using System.IO;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Texture _tex;
        private RenderTarget _tgt;

        private PixelShader _pixelShader;
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
            _tex = new Texture(Path.Combine(loc, "whiterect.png"));
            _tgt = new RenderTarget(1024, 600);

            _pixelShader = new PixelShader(Path.Combine(loc, "sh.frag"));
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";

            if (_goUp)
            {
                _x++;
                if (_x > 900)
                    _goUp = false;
            }
            else
            {
                _x--;
                if (_x < -300)
                    _goUp = true;
            }

            _rot += 25f * delta;

        }

        protected override void Draw(RenderContext context)
        {
            if (_doot)
            {
                context.ActivateShader(_pixelShader);
                _pixelShader.SetUniform("rotation", _rot % 360);
            }

            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.CornflowerBlue);
                context.DrawTexture(_tex, new Vector2(480, 300), Vector2.One, new Vector2(_tex.Width / 2, _tex.Height / 2), 0);
            });

            if (_doot)
            {
                context.DeactivateShader();
            }

            context.DrawTexture(_tgt.Texture, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
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