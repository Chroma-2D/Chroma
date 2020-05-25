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
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = delta.ToString();
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt,
                () =>
                {
                    context.Clear(Color.Black);

                    context.Scissor = _sc;
                    context.DrawTexture(_bigpic, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
                    context.Scissor = Scissor.None;

                    context.DrawString(_ttf, $"{Window.FPS} FPS", Vector2.Zero);
                });

            _ps.Activate();
            _ps.SetUniform("CRT_CURVE_AMNTy", .15f);
            _ps.SetUniform("CRT_CURVE_AMNTx", .25f);
            context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            Shader.Deactivate();
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _sc = new Scissor((short)(e.Position.X), (short)(e.Position.Y), 100, 100);
        }
    }
}