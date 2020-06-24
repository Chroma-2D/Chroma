using System.Globalization;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Shaders
{
    public class GameCore : Game
    {
        private RenderTarget _target;
        private PixelShader _screenShader;
        private Texture _burger;
        
        private float _rotation;
        private bool _shaderEnabled;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }
        
        protected override void LoadContent()
        {
            _target = new RenderTarget(Window.Properties.Width, Window.Properties.Height);
            _screenShader = Content.Load<PixelShader>("Shaders/scanline.frag");
            _burger = Content.Load<Texture>("Textures/burg.png");
        }
        
        protected override void Update(float delta)
        {
            _rotation += 50 * delta;
            _rotation %= 360;
            
            Window.Properties.Title = Window.FPS.ToString(CultureInfo.InvariantCulture);
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_target, () =>
            {
                context.Clear(Color.DodgerBlue);
                context.DrawTexture(_burger, new Vector2(64) + _burger.Center, Vector2.One, _burger.Center, _rotation);
                context.DrawString("Use F1 to toggle the pixel shader on and off.", new Vector2(8));
            });

            if (_shaderEnabled)
            {
                _screenShader.Activate();
                _screenShader.SetUniform("CRT_CURVE_AMNTx", 0.3f);
                _screenShader.SetUniform("CRT_CURVE_AMNTy", 0.2f);
            }

            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);

            if (_shaderEnabled)
            {
                Shader.Deactivate();
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                _shaderEnabled = !_shaderEnabled;
            }
        }
    }
}