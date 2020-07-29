using System.Globalization;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace PixelShaders
{
    public class GameCore : Game
    {
        private RenderTarget _target;
        private PixelShader _crtShader;
        private PixelShader _tintShader;
        private Texture _burger;
        
        private float _rotation;
        private bool _crtShaderEnabled;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));

            GraphicsManager.LimitFramerate = false;
            Graphics.VerticalSyncMode = VerticalSyncMode.None;
        }
        
        protected override void LoadContent()
        {
            _target = new RenderTarget(Window.Size);
            _crtShader = Content.Load<PixelShader>("Shaders/VerticalGauss.glsl");
            _tintShader = Content.Load<PixelShader>("Shaders/tint.frag");
            _burger = Content.Load<Texture>("Textures/burg.png");
        }
        
        protected override void Update(float delta)
        {
            _rotation += 50 * delta;
            _rotation %= 360;
            
            Window.Title = Window.FPS.ToString(CultureInfo.InvariantCulture);
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_target, () =>
            {
                context.Clear(Color.Transparent);
                
                _tintShader.Activate();
                _tintShader.SetUniform("mouseLoc", Mouse.GetPosition() / Window.Size.Width);
                    context.DrawTexture(
                        _burger, 
                        Window.Center - (_burger.Center / 2),
                        Vector2.One,
                        _burger.Center,
                        _rotation
                    );
                Shader.Deactivate();
                
                context.DrawString(
                    "Use <F1> to toggle the CRT shader on and off.\n" +
                    "Move mouse horizontally to tweak the burger's green channel.\n" +
                    "Move mouse vertically to tweak the burger's red channel.", 
                    new Vector2(8)
                );
                
                if (_crtShaderEnabled)
                {
                    _crtShader.Activate();
                    _crtShader.SetUniform("rt_dims", new Vector2(Window.Size.Width, Window.Size.Height));
                    _crtShader.SetUniform("vx_offset", 5f);
                }
            });



            context.Clear(Color.Transparent);
            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);

            if (_crtShaderEnabled)
            {
                Shader.Deactivate();
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                _crtShaderEnabled = !_crtShaderEnabled;
            }
        }
    }
}