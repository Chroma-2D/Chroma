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
        private Texture _ttfMap;

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

            _tex = new Texture(Path.Combine(loc, "skeltal.png"))
            {
                ColorMask = Color.White,
            };

            _origin = new Vector2(
                _tex.Width * .3f / 2,
                _tex.Height * .3f / 2
            );

            _ttf = new TrueTypeFont(Path.Combine(loc, "c64style.ttf"), 16);
            _ttfMap = _ttf.RenderGlyph('$');
            _ttfMap.ColorMask = Color.HotPink;

            //Console.WriteLine(_font.Measure("DOOT ME UP\nINSIDE!11"));
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
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);
                for (var x = 0; x < 2; x++)
                {
                    for (var y = 0; y < 2; y++)
                    {
                        context.DrawTexture(
                            _tex,
                            new Vector2(
                                x * _tex.Width * .3f,
                                y * _tex.Height * .3f
                            ) + _origin,
                            new Vector2(.3f),
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
            );

            context.DrawTexture(
                _ttfMap,
                new Vector2(100, 150), Vector2.One,
                Vector2.Zero, .0f
            );
            //context.DrawString(_font, "DOOT ME UP\nINSIDE!11", new Vector2(0, 0));

        }
    }
}