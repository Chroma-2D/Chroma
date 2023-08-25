using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;

namespace LerpingCameras
{
    public class GameCore : Game
    {
        private LerpCamera _lerpCamera;

        public GameCore() 
            : base(new(true, false))
        {
            Graphics.VerticalSyncMode = VerticalSyncMode.None;
        }

        protected override void LoadContent()
        {
            _lerpCamera = new LerpCamera(Vector2.Zero);
        }

        protected override void Draw(RenderContext context)
        {
            context.WithCamera(_lerpCamera, (ctx, tgt) =>
            {
                base.Draw(ctx);
                ctx.Pixel(Window.Center, Color.Red);
            });
            
            context.DrawString(
                (_lerpCamera.EndTime - _lerpCamera.StartTime).TotalMilliseconds.ToString("F0"),
                Window.Center, 
                Color.Red
            );
        }

        protected override void Update(float delta)
        {
            _lerpCamera.Update(delta);
            base.Update(delta);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _lerpCamera.StartMoving(
                    Window.Center, 
                    3.5f
                );
            }
        }
    }
}