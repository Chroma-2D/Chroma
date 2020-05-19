using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private TrueTypeFont _ttf;
        private Texture _bigpic;
        private string _text;
        private RenderTarget _tgt;
        private Camera _cam;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = true;

            Window.GoWindowed(1024, 640);
            _tgt = new RenderTarget((ushort)Window.Properties.Width, (ushort)Window.Properties.Height);
            _cam = new Camera();
        }

        protected override void LoadContent()
        {
            _ttf = Content.Load<TrueTypeFont>("Fonts/TAHOMA.TTF", 16);
            _bigpic = Content.Load<Texture>("Textures/bigpic.jpg");
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"FPS: {Window.FPS}";

            _text = $"x: {_cam.X}\n" +
                    $"y: {_cam.Y}\n" +
                    $"z: {_cam.Z}\n" +
                    $"far_z: {_cam.FarZ}\n" +
                    $"near_z: {_cam.NearZ}\n" +
                    $"zoom_x: {_cam.ZoomX}\n" +
                    $"zoom_y: {_cam.ZoomY}\n" +
                    $"rotation: {_cam.Rotation}\n" +
                    $"center_origin: {_cam.UseCenteredOrigin}";
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.WithCamera(_cam, () =>
                {
                    context.DrawTexture(_bigpic, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
                });
            });
            
            context.DrawTexture(_tgt, Vector2.Zero, Vector2.One, Vector2.Zero, 0f);
            context.DrawString(_ttf, _text, Vector2.Zero);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            var modifier = 1;

            if (e.Modifiers.HasFlag(KeyModifiers.LeftShift))
                modifier = 5;
            
            if (e.Modifiers.HasFlag(KeyModifiers.LeftControl))
                modifier *= 2;
            
            if (e.KeyCode == KeyCode.Backspace)
            {
                _cam.Z += modifier;
            }
            else if (e.KeyCode == KeyCode.Return)
            {
                _cam.Z -= modifier;
            }
            else if (e.KeyCode == KeyCode.Up)
            {
                _cam.Y -= modifier;
            }
            else if (e.KeyCode == KeyCode.Down)
            {
                _cam.Y += modifier;
            }
            else if (e.KeyCode == KeyCode.Left)
            {
                _cam.X -= modifier;
            }
            else if (e.KeyCode == KeyCode.Right)
            {
                _cam.X += modifier;
            }
            else if (e.KeyCode == KeyCode.F5)
            {
                _cam.ZoomX += modifier;
            }
            else if (e.KeyCode == KeyCode.F6)
            {
                _cam.ZoomX -= modifier;
            }
            else if (e.KeyCode == KeyCode.F7)
            {
                _cam.ZoomY += modifier;
            }
            else if (e.KeyCode == KeyCode.F8)
            {
                _cam.ZoomY -= modifier;
            }
            else if (e.KeyCode == KeyCode.F1)
            {
                _cam.FarZ += modifier;
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                _cam.FarZ -= modifier;
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                _cam.NearZ += modifier;
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                _cam.NearZ -= modifier;
            }
            else if (e.KeyCode == KeyCode.Q)
            {
                _cam.Rotation -= modifier;
            }
            else if (e.KeyCode == KeyCode.E)
            {
                _cam.Rotation += modifier;
            }
            else if (e.KeyCode == KeyCode.R)
            {
                _cam = new Camera();
            }
            else if (e.KeyCode == KeyCode.S)
            {
                _tgt.SetCamera(_cam);
            }
        }
    }
}