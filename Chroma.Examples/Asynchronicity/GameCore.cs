using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Threading;
using Color = System.Drawing.Color;

namespace Asynchronicity
{
    public class GameCore : Game
    {
        private RenderTarget _target;

        protected override void LoadContent()
        {
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "Press F1 to asynchronously dispose and recreate the render target, causing an AccessViolationException.\n" +
                "Press F2 to queue the render target creation for execution on the main thread.",
                new Vector2(8)
            );

            if (_target != null && !_target.Disposed)
            {
                context.RenderTo(_target, () =>
                {
                    context.Rectangle(
                        ShapeMode.Fill,
                        new Vector2(16),
                        new Size(32, 32),
                        Color.Aqua
                    );
                });

                context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0);
            }
        }

        protected override void Update(float delta)
        {
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                Task.Run(() =>
                {
                    _target?.Dispose();
                    _target = new RenderTarget(Window.Size);
                });
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                Task.Run(async () =>
                {
                    await Dispatcher.RunOnMainThread(() =>
                    {
                        _target?.Dispose();
                        _target = new RenderTarget(Window.Size);
                    });
                });
            }
        }
    }
}