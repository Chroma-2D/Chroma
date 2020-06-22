using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;

            Window.GoWindowed(1024, 640);
            Graphics.AutoClearColor = Color.CornflowerBlue;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString("Yay!", new Vector2(16),
                (c, i, p, g) => { return new GlyphTransformData(p) {Color = Color.Aqua}; });
        }

        protected override void LoadContent()
        {
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
        }
    }
}