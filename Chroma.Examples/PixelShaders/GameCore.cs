using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;
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
        private Texture _overlay;

        private bool _showOverlay = true;
        private bool _showEdge = true;
        private bool _showTweak = true;

        private float _rotation;

        private StringBuilder _messageBuilder = new();

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
            _burger.FilteringMode = TextureFilteringMode.NearestNeighbor;

            _overlay = Content.Load<Texture>("Textures/checkerboard.jpg");
            _overlay.HorizontalWrappingMode = TextureWrappingMode.Mirror;
            _overlay.VerticalWrappingMode = TextureWrappingMode.Mirror;
        }

        protected override void Update(float delta)
        {
            _rotation += 50 * delta;
            _rotation %= 360;

            Window.Title = $"Chroma Framework - pixel shader example. {PerformanceCounter.FPS} FPS";

            _messageBuilder.Clear();
            _messageBuilder.Append("Press <F1> to toggle sprite edge detection.\n" +
                                   "Press <F2> to toggle texture overlay.\n" +
                                   "Press <F3> to toggle cursor position-based color modulation for the sprite.\n\n");

            if (_showEdge)
                _messageBuilder.AppendLine("Edge detection active.");

            if (_showOverlay)
                _messageBuilder.AppendLine("Texture overlay active.");

            if (_showTweak)
                _messageBuilder.Append("Cursor position-based color modulation active.");
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_target, (ctx, _) =>
            {
                ctx.Clear(Color.Black);

                _tintEffect.Activate();
                _tintEffect.SetUniform("mouse_loc", Mouse.GetPosition() / Window.Width);
                _tintEffect.SetUniform("overlay", _overlay, 1);
                _tintEffect.SetUniform("border_color", Color.Yellow);

                _tintEffect.SetUniform("show_edge", _showEdge);
                _tintEffect.SetUniform("show_overlay", _showOverlay);
                _tintEffect.SetUniform("show_tweak", _showTweak);

                ctx.DrawTexture(
                    _burger,
                    Window.Center - (_burger.Center / 2),
                    Vector2.One * 2,
                    _burger.Center,
                    _rotation
                );
                Shader.Deactivate();

                ctx.DrawString(
                    _messageBuilder.ToString(),
                    new Vector2(8)
                );
            });

            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    _showEdge = !_showEdge;
                    break;

                case KeyCode.F2:
                    _showOverlay = !_showOverlay;
                    break;

                case KeyCode.F3:
                    _showTweak = !_showTweak;
                    break;
            }
        }
    }
}