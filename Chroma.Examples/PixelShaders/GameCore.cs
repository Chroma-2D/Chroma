using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace PixelShaders
{
    public class GameCore : Game
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        private RenderTarget _target;
        private Effect _tintEffect;
        private Texture _burger;

        private float _rotation;

        public GameCore() : base(new(false, false))
        {
            _log.Info(
                $"GLSL {Shader.MinimumSupportedGlslVersion}-{Shader.MaximumSupportedGlslVersion} supported.");
            
            Window.Mode.SetWindowed(new Size(1024, 600));
            Window.CenterOnScreen();
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            _target = new RenderTarget(Window.Size);
            _tintEffect = Content.Load<Effect>("Shaders/tint.frag");
            _burger = Content.Load<Texture>("Textures/burg.png");
        }

        protected override void Update(float delta)
        {
            _rotation += 50 * delta;
            _rotation %= 360;

            Window.Title = PerformanceCounter.FPS.ToString(CultureInfo.InvariantCulture);
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_target, () =>
            {
                context.Clear(Color.Black);

                _tintEffect.Activate();
                _tintEffect.SetUniform("mouseLoc", Mouse.GetPosition() / Window.Width);

                context.DrawTexture(
                    _burger,
                    Window.Center - (_burger.Center / 2),
                    Vector2.One * 2,
                    _burger.Center,
                    _rotation
                );
                Shader.Deactivate();

                context.DrawString(
                    "Move mouse horizontally to tweak the burger's green channel.\n" +
                    "Move mouse vertically to tweak the burger's red channel.",
                    new Vector2(8)
                );
            });

            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
        }
    }
}