using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Input.EventArgs;
using Color = Chroma.Graphics.Color;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private TrueTypeFont _ttf;
        private Texture _bigpic;
        private RenderTarget _tgt;
        private Rectangle _rect;

        private float _kx;
        private float _ky;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;
            Graphics.LimitFramerate = true;

            Window.GoWindowed(1024, 640);
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/TAHOMA.TTF", 16);
            _bigpic = Content.Load<Texture>("Textures/bigpic.jpg");
            _rect = new Rectangle(256, 256, 256, 128);
            _tgt = new RenderTarget((ushort)Window.Properties.Width, (ushort)Window.Properties.Height);
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = delta.ToString();

            _rect.X = (int)_kx;
            _rect.Y = (int)_ky;
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.DrawTexture(_bigpic, Vector2.Zero, Vector2.One, Vector2.Zero, 0);
            });

            context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0);
            context.DrawString(_ttf, $"{Window.FPS} FPS", new Vector2(24));
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _kx = e.Position.X;
            _ky = e.Position.Y;
        }
    }
}