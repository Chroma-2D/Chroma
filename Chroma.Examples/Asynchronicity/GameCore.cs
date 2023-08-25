using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Chroma;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Threading;
using Color = System.Drawing.Color;

namespace Asynchronicity
{
    public class GameCore : Game
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        private RenderTarget _target;

        public GameCore() : base(new(false, false))
        {
        }

        protected override void LoadContent()
        {
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "Press <F1> to asynchronously dispose and recreate the render target\n" +
                "causing an InvalidOperationException.\n\n" +
                "Press <F2> to asynchronously queue the render target creation for execution on the main thread.\n" +
                "Press <F3> to asynchronously queue render target destruction for execution on the main thread.",
                new Vector2(8)
            );

            if (_target != null && !_target.Disposed)
            {
                context.RenderTo(_target, (ctx, tgt) =>
                {
                    ctx.Rectangle(
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
                    try
                    {
                        _target?.Dispose();
                        _target = new RenderTarget(Window.Size);
                    }
                    catch (InvalidOperationException ioe)
                    {
                        _log.Error($"Caught exception: {ioe.Message}");
                    }
                });
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                Task.Run(async () =>
                {
                    await Dispatcher.RunOnMainThread(() =>
                    {
                        if (_target != null && !_target.Disposed)
                        {
                            _target.Dispose();
                        }

                        _target = new RenderTarget(Window.Size);
                    });
                });
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                Task.Run(async () =>
                {
                    await Dispatcher.RunOnMainThread(() =>
                    {
                        if (_target != null && !_target.Disposed)
                        {
                            _target.Dispose();
                        }
                    });
                });
            }
        }
    }
}