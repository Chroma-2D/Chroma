using System.Collections.Generic;
using System.Drawing;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Color = Chroma.Graphics.Color;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private TrueTypeFont _ttf;
        private string _text;
        private List<Point> _p1;
        private List<Point> _p2;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = true;

            Window.GoWindowed(1024, 640);

            _p1 = new List<Point>
            {

            };
            
            _p2 = new List<Point>
            {
                new Point(30, 33),
                new Point(38, 43),
                new Point(49, 69),
                new Point(60, 80),
                new Point(70, 88),
                new Point(64, 33),
                new Point(100, 200),
            };
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/vv1989.ttf", 48);
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"FPS: {Window.FPS}";

            _text = $"HINTING: {(_ttf.HintingEnabled ? "ENABLED" : "DISABLED")}\n" +
                    $"FORCED AUTO-HINTING: {(_ttf.ForceAutoHinting ? "YES" : "NO")}\n" +
                    $"HINTING MODE: {(_ttf.HintingMode)}\n" +
                    $"the quick brown fox jumps over the lazy dog 1234567890";
        }

        protected override void Draw(RenderContext context)
        {
            //context.DrawString(_ttf, _text, Vector2.One * 10);
            context.Polyline(_p2, Color.Cyan, false);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _ttf.HintingEnabled = !_ttf.HintingEnabled;
            }
            else if (e.KeyCode == KeyCode.Return)
            {
                _ttf.ForceAutoHinting = !_ttf.ForceAutoHinting;
            }
            else if (e.KeyCode == KeyCode.Up)
            {
                _ttf.HintingMode = (HintingMode)(((int)_ttf.HintingMode + 1) % 3);
            }
            else if (e.KeyCode == KeyCode.F)
            {
                Audio.HookPostMixProcessor<float>((chunk, bytes) =>
                {
                    for (var i = 0; i < chunk.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (i % 32 == 0)
                            {
                                chunk[i] = 1.0f;
                            }
                        }
                        else
                        {
                            if (i % 31 == 0)
                            {
                                chunk[i] = 1.0f;
                            }
                        }
                    }
                });
            }
        }
    }
}