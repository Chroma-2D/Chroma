using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private TrueTypeFont _ttf;
        private Texture _bigpic;
        private PixelShader _ps;
        private RenderTarget _tgt;
        private Scissor _sc;
        private Camera _cam;

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
            _ps = Content.Load<PixelShader>("Shaders/sh.frag");
            _tgt = new RenderTarget((ushort)Window.Properties.Width, (ushort)Window.Properties.Height);
            _sc = new Scissor(10, 10, 100, 100);
            _cam = new Camera();
            _cam.FarZ = 2000;
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = delta.ToString();
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);

                context.WithCamera(_cam, () =>
                {
                    context.Transform.Push();
                    context.Transform.Translate(new Vector2(_kx, _ky));
                    {
                        context.LineThickness = 3;
                        context.Rectangle(
                            ShapeMode.Fill,
                            new Vector2(0), 
                            128, 
                            128, 
                            Color.Red
                        );
                    }
                
                    context.Transform.Pop();
                });
                context.DrawString(_ttf, $"{Window.FPS} FPS", new Vector2(50, 300));
            });

            context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0);
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _kx = e.Position.X;
            _ky = e.Position.Y;
        }

        protected override void WheelMoved(MouseWheelEventArgs e)
        {
            _sc = new Scissor(_sc.X, _sc.Y, (ushort)(_sc.Width + 10 * e.Motion.Y),
                (ushort)(_sc.Height + 10 * e.Motion.Y));
        }
    }
}