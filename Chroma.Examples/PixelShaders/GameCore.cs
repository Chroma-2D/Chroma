using System.Globalization;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace PixelShaders
{
    public class GameCore : Game
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        private RenderTarget _target;
        private PixelShader _gaussShader;
        private PixelShader _tintShader;
        private Effect _testEffect;
        private Texture _burger;

        private float _rotation;
        private bool _gaussShaderEnabled;
        private bool _testEffectEnabled = true;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));

            GraphicsManager.LimitFramerate = false;
            Graphics.VerticalSyncMode = VerticalSyncMode.None;

            _log.Info(
                $"GLSL {Shader.MinimumSupportedGlslVersion}-{Shader.MaximumSupportedGlslVersion} version range supported.");

            _gaussShader = Content.Load<PixelShader>("Shaders/VerticalGauss_150.glsl");
            _testEffect = Content.Load<Effect>("Shaders/effect.frag");
        }

        protected override void LoadContent()
        {
            _target = new RenderTarget(Window.Size);
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

                if (_testEffectEnabled)
                {
                    _testEffect.Activate();

                    context.DrawTexture(
                        _burger,
                        Window.Center + new Vector2(120, 0),
                        Vector2.One,
                        Vector2.Zero,
                        0
                    );
                    
                    Shader.Deactivate();
                }

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
                    "Use <F1> to toggle the gauss blur shader on and off.\n" +
                    "Move mouse horizontally to tweak the burger's green channel.\n" +
                    "Move mouse vertically to tweak the burger's red channel.",
                    new Vector2(8)
                );

                if (_gaussShaderEnabled)
                {
                    _gaussShader.Activate();
                    _gaussShader.SetUniform("rt_dims", new Vector2(Window.Size.Width, Window.Size.Height));
                    _gaussShader.SetUniform("vx_offset", 5f);
                }
            });


            context.Clear(Color.Transparent);
            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);

            if (_gaussShaderEnabled)
            {
                Shader.Deactivate();
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                _gaussShaderEnabled = !_gaussShaderEnabled;
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                _testEffectEnabled = !_testEffectEnabled;
            }
        }
    }
}