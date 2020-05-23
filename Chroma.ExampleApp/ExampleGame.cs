using System.IO;
using System.Numerics;
using System.Reflection;
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

        public ExampleGame()
        {
            Graphics.VSyncEnabled = true;

            Window.GoWindowed(1024, 640);
            _tgt = new RenderTarget((ushort)Window.Properties.Width, (ushort)Window.Properties.Height);
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/TAHOMA.TTF", 16);
            _bigpic = Content.Load<Texture>("Textures/bigpic.jpg");
            _ps = Content.Load<PixelShader>("Shaders/sh.frag");
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = delta.ToString();
        }

        protected override void Draw(RenderContext context)
        {

            context.RenderTo(_tgt,
                () => { context.DrawTexture(_bigpic, Vector2.Zero, Vector2.One, Vector2.Zero, 0f); });

            _ps.Activate();
            _ps.SetUniform("CRT_CURVE_AMNTy", .25f);
            _ps.SetUniform("CRT_CURVE_AMNTx", .25f);
                context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            Shader.Deactivate();
            context.DrawString(_ttf, $"{Window.FPS} FPS", Vector2.Zero);
        }
    }
}