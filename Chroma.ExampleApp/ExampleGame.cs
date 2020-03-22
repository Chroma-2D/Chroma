using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.Windowing;
using System;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Vector2 _position;
        private float _speed = 400f;
        private string _str = string.Empty;
        private Color _color = Color.White;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;
            Log.Verbosity |= Verbosity.Debug;

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

        protected override void TextInput(TextInputEventArgs e)
        {
            _str += e.Text;
            Window.Properties.Title = _str;
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.Black);
            context.Rectangle(ShapeMode.Fill, _position, new Size(32, 32), _color);
        }

        protected override void Update(float delta)
        {
            var xAxis = Controller.GetAxisValueNormalized(0, ControllerAxis.LeftStickX);
            var yAxis = Controller.GetAxisValueNormalized(0, ControllerAxis.LeftStickY);

            if (Controller.IsButtonPressed(0, ControllerButton.Xbox))
            {
                _color = Color.Green;
            }

            var dx = _speed * delta * xAxis;
            var dy = _speed * delta * yAxis;
            var dims = Window.Properties.Size;

            if (_position.X + dx < 0 || _position.X + dx + 32 >= dims.Width)
            {
                Controller.Vibrate(0, 16384, 0, 16);
            }
            else
            {
                _position = new Vector2(_position.X + dx, _position.Y);
            }

            if (_position.Y + dy < 0 || _position.Y + dy + 32 >= dims.Height)
            {
                Controller.Vibrate(0, 0, 32768, 16);
            }
            else
            {
                _position = new Vector2(_position.X, _position.Y + dy);
            }

            Window.Properties.Title = Window.FPS.ToString();
        }

        protected override void ControllerConnected(ControllerEventArgs e)
        {
            Controller.SetDeadZoneUniform(e.Controller.PlayerIndex, 3500);
        }

        protected override void ControllerAxisMoved(ControllerAxisEventArgs e)
        {
            if (Controller.CanIgnoreAxisMotion(0, e.Axis, e.Value))
                return;

            Console.WriteLine($"{e.Axis} on controller {e.Controller.PlayerIndex}");
        }

        protected override void ControllerButtonPressed(ControllerButtonEventArgs e)
        {
            if (e.Button == ControllerButton.A)
            {
                Console.WriteLine(Controller.GetBatteryLevel(0));
            }
            Console.WriteLine($"{e.Button} on controller {e.Controller.PlayerIndex}");
        }
    }
}
