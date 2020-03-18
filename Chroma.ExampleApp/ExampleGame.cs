using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.Windowing;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Vector2 _position;
        private float _speed = 400f;
        private string _str = string.Empty;

        public ExampleGame()
        {
            GraphicsManager.Instance.VSyncEnabled = false;

            Window.GoWindowed(1024, 600); 

            Window.MouseEntered += (sender, e) => Log.Info(":: Mouse entered window area.");
            Window.MouseLeft += (sender, e) => Log.Info(":: Mouse left window area.");
            Window.Moved += (sender, e) => Log.Info($":: Window was moved to {e.Position}");
            Window.SizeChanged += (sender, e) => Log.Info($":: Window size has changed to {e.Size}");
            Window.Resized += (sender, e) => Log.Info($":: Window was resized by user to {e.Size}");
            Window.Focused += (sender, e) => Log.Info($":: Window has gained keyboard focus.");
            Window.StateChanged += (sender, e) => Log.Info($":: Window state changed to {e.State}");
            Window.Unfocused += (sender, e) => Log.Info($":: Window has lost keyboard focus.");
            Window.Hidden += (sender, e) => Log.Info(":: Window hidden.");
            Window.Invalidated += (sender, e) => Log.Info(":: Window invalidated.");

            Window.Properties.State = WindowState.Maximized;
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {

        }

        protected override void TextInput(TextInputEventArgs e)
        {
            _str += e.Text;
            Window.Properties.Title = _str;
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.Black);
            context.Rectangle(ShapeMode.Fill, _position, new Size(32, 32), Color.White);
        }

        protected override void Update(float delta)
        {
            var dx = 0f;
            var dy = 0f;

            if (Keyboard.IsKeyDown(KeyCode.Up))
            {
                dy = -_speed * delta;
            }
            else if (Keyboard.IsKeyDown(KeyCode.Down))
            {
                dy = _speed * delta;
            }

            if (Keyboard.IsKeyDown(KeyCode.Left))
            {
                dx = -_speed * delta;
            }
            else if (Keyboard.IsKeyDown(KeyCode.Right))
            {
                dx = _speed * delta;
            }

            _position = new Vector2(_position.X + dx, _position.Y + dy);
            Window.Properties.Title = Window.FPS.ToString();
        }
    }
}
