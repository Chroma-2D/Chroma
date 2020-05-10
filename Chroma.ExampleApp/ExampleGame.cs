using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private TrueTypeFont _ttf;
        private string _text;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = true;

            Window.GoWindowed(1024, 640);
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/Anoxic-Light.ttf", 24);
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
            context.DrawString(_ttf, _text, Vector2.One * 10);
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
        }
    }
}