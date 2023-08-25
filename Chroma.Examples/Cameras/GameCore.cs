using System;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;

namespace Cameras
{
    public class GameCore : Game
    {
        private Camera _cam;
        private Vector2 _burgpos;
        private Texture _grid;
        private Texture _burg;

        public GameCore() : base(new(false, false))
        {
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            _cam = new Camera
            {
                UseCenteredOrigin = true
            };
            
            _grid = Content.Load<Texture>("Textures/grid.png");
            _burg = Content.Load<Texture>("Textures/burg.png");
        }

        protected override void Update(float delta)
        {
            Window.Title = $"{PerformanceCounter.FPS} | {PerformanceCounter.Delta}";
            
            if (Keyboard.IsKeyDown(KeyCode.W))
            {
                _cam.Y += (int)(500 * delta);
            }
            else if (Keyboard.IsKeyDown(KeyCode.S))
            {
                _cam.Y -= (int)(500 * delta);
            }

            if (Keyboard.IsKeyDown(KeyCode.A))
            {
                _cam.X -= (int)(500 * delta);
            }
            else if (Keyboard.IsKeyDown(KeyCode.D))
            {
                _cam.X += (int)(500 * delta);
            }

            if (Keyboard.IsKeyDown(KeyCode.Up))
            {
                _burgpos.Y -= 200 * delta;
            }
            else if(Keyboard.IsKeyDown(KeyCode.Down))
            {
                _burgpos.Y += 200 * delta;
            }

            if (Keyboard.IsKeyDown(KeyCode.Left))
            {
                _burgpos.X -= 200 * delta;
            }
            else if (Keyboard.IsKeyDown(KeyCode.Right))
            {
                _burgpos.X += 200 * delta;
            }
        }

        protected override void Draw(RenderContext context)
        {
            context.WithCamera(_cam, (ctx, cam) =>
            {
                ctx.DrawTexture(
                    _grid,
                    Vector2.Zero,
                    Vector2.One,
                    Vector2.Zero,
                    rotation: 0f
                );
                
                ctx.DrawTexture(
                    _burg,
                    _burgpos,
                    Vector2.One,
                    Vector2.Zero, 
                    0
                );
            });
            
            context.DrawString(
                "Use <W> <A> <S> <D> to move camera around the screen.\n" +
                "Use mouse to rotate the camera around the center.\n" +
                "Use <F1> to lock the mouse cursor into the window.\n" +
                "Use mouse wheel to zoom the camera in/out.", 
                new Vector2(8),
                Color.Red
            );
        }

        protected override void WheelMoved(MouseWheelEventArgs e)
        {
            if (_cam.ZoomX <= 0.3f || _cam.ZoomY <= 0.3f)
            {
                _cam.ZoomX = _cam.ZoomY = 0.3f;
            }
            
            if (e.Motion.Y > 0)
            {
                _cam.ZoomX *= 2;
                _cam.ZoomY *= 2;
            }
            else
            {
                _cam.ZoomX *= 0.5f;
                _cam.ZoomY *= 0.5f;
            }
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _cam.Rotation += e.Delta.X / 8;
            _cam.Rotation %= 360;
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.F1)
            {
                Mouse.IsCaptured = !Mouse.IsCaptured;
                Mouse.IsRelativeModeEnabled = !Mouse.IsRelativeModeEnabled;
            }
        }
    }
}