using System;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
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

        public ExampleGame()
        {
            Graphics.VSyncEnabled = true;

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
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = delta.ToString();

            var r = (ushort)MathF.Abs(Controller.GetAxisValue(0, ControllerAxis.RightTrigger));
            var l = (ushort)MathF.Abs(Controller.GetAxisValue(0, ControllerAxis.LeftTrigger));

            if (r > 0)
                Controller.Vibrate(0, 0, r, 100);

            if (l > 0)
                Controller.Vibrate(0, l, 0, 100);
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);
                context.WithCamera(_cam, () =>
                {
                    context.Scissor = _sc;
                    context.DrawTexture(_bigpic, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
                    context.Scissor = Scissor.None;
                });
                context.DrawString(_ttf, $"{Window.FPS} FPS", Vector2.Zero);
            });

            context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _sc = new Scissor((short)(e.Position.X - _sc.Width / 2), (short)(e.Position.Y - _sc.Height / 2), _sc.Height, _sc.Width);
        }

        protected override void WheelMoved(MouseWheelEventArgs e)
        {
            _sc = new Scissor(_sc.X, _sc.Y, (ushort)(_sc.Width + 10 * e.Motion.Y), (ushort)(_sc.Height + 10 * e.Motion.Y));
        }
    }
}