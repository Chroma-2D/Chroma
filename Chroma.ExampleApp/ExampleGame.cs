using System.Diagnostics;
using System.IO;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Graphics;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Texture _tex;
        private RenderTarget _tgt;

        private Stopwatch _sw;

        private float _rotation;
        private byte _alpha;

        public ExampleGame()
        {
            _sw = new Stopwatch();
            Graphics.VSyncEnabled = false;
            Log.Verbosity |= Verbosity.Debug;

            Window.GoWindowed(1024, 600);

            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _tgt = new RenderTarget((ushort)Window.Properties.Width, (ushort)Window.Properties.Height);

            _tex = new Texture(Path.Combine(loc, "dvd.png"))
            {
                ColorMask = Color.White,
                Origin = new Vector2(0.5f, 0.5f)
            };
        }
        
        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";
            _rotation += 45f * delta;

            _alpha = 255;

            _tex.ColorMask = new Color(255, 255, 255, _alpha);
        }
        
        protected override void Draw(RenderContext context)
        {
            context.SetRenderTarget(_tgt);
            context.Clear(Color.Yellow);
            for (var x = 0; x < 24; x++)
            {
                for (var y = 0; y < 24; y++)
                {
                    context.DrawTexture(
                        _tex,
                        new Vector2(
                            x * _tex.Width,
                            y * _tex.Height
                        ),
                        new Vector2(1.0f),
                        _rotation
                    );
                }
            }
            context.SetRenderTarget(null);

            context.Clear(Color.CornflowerBlue);
            context.DrawTexture(
                _tgt.Texture, Vector2.Zero, Vector2.One, 0f
            );
        }
    }
}