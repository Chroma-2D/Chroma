using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.Particles;
using Chroma.Graphics.Particles.StateInitializers;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Color = Chroma.Graphics.Color;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private BitmapFont _bmf;
        private string _str;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;

            Window.GoWindowed(1024, 640);
            Graphics.AutoClearColor = Color.CornflowerBlue;
        }

        protected override void LoadContent()
        {
            _bmf = Content.Load<BitmapFont>("BitmapFonts/vv.fnt");
        }

        protected override void Update(float delta)
        {
            _str = $"{Window.FPS}";

            Window.Properties.Title = delta.ToString();
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(_bmf, "Lorem ipsum dolor\nsit amet.", new Vector2(22), (c, i, p, g) =>
            {
                return new GlyphTransformData(p) {Color = Color.Red};
            });
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
        }
    }
}