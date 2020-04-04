using System.Diagnostics;
using System.IO;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Texture _tex;
        private RenderTarget _tgt;

        private Stopwatch _sw;
        private TrueTypeFont _ttf;

        private Vector2 _origin;
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

            _tex = new Texture(Path.Combine(loc, "delet.png"))
            {
                ColorMask = Color.White,
            };

            _origin = new Vector2(
                _tex.Width * .3f / 2,
                _tex.Height * .3f / 2
            );

            _ttf = new TrueTypeFont(Path.Combine(loc, "TAHOMA.TTF"), 24);
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
            context.Clear(Color.Black);

           /* context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);
                for (var x = 0; x < 100; x++)
                {
                    for (var y = 0; y < 100; y++)
                    {
                        context.DrawTexture(
                            _tex,
                            new Vector2(
                                100, 100
                            ) + _origin,
                            Vector2.One,
                            _origin,
                            _rotation
                        );
                    }
                }
            });

            context.DrawTexture(
                _tgt.Texture,
                Vector2.Zero,
                Vector2.One,
                Vector2.Zero,
                0f
            );*/

           /* context.DrawTexture(
                _ttf.Atlas,
                Vector2.Zero,//new Vector2(300, 300), 
                Vector2.One,
                Vector2.Zero, .0f
            );*/

            context.DrawString(_ttf, "the quick brown fox jumps over the lazy dog 1234567890\nTHE QUICK BROWN FOX JUMPS OVER THE LAZY DOG 1234567890\nLoReM iPsUm dOlOr sIt AmEt\nX", Vector2.Zero);

        }
    }
}