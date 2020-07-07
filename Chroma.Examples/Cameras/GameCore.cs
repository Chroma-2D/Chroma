using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Cameras
{
    public class GameCore : Game
    {
        private Camera _cam;
        private Texture _tex;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }

        protected override void LoadContent()
        {
            _cam = new Camera();
            _cam.UseCenteredOrigin = true;
            _tex = Content.Load<Texture>("Textures/grid.png");
        }

        protected override void Update(float delta)
        {
            if (Keyboard.IsKeyDown(KeyCode.W))
            {
                _cam.Y += (int)(100 * delta);
            }
            else if (Keyboard.IsKeyDown(KeyCode.S))
            {
                _cam.Y -= (int)(100 * delta);
            }

            if (Keyboard.IsKeyDown(KeyCode.A))
            {
                _cam.X -= (int)(100 * delta);
            }
            else if (Keyboard.IsKeyDown(KeyCode.D))
            {
                _cam.X += (int)(100 * delta);
            }
        }

        protected override void Draw(RenderContext context)
        {
            context.WithCamera(_cam, () =>
            {
                context.DrawTexture(
                    _tex,
                    Vector2.Zero,
                    Vector2.One,
                    Vector2.Zero,
                    rotation: 0f
                );
            });

            context.DrawString(
                "Use <W> <A> <S> <D> to move camera around the screen.\n" +
                "Use mouse to rotate the camera around the center.\n" +
                "Use F1 to lock the mouse cursor into the window.\n" +
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
                Window.IsCursorGrabbed = !Window.IsCursorGrabbed;
                Mouse.IsRelativeModeEnabled = !Mouse.IsRelativeModeEnabled;
            }
        }
    }
}