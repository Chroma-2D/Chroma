﻿using System;
using System.IO;
using System.Reflection;
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
        private Color _color = Color.White;

        private Button _button;
        private Texture _tex;

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

            _button = new Button(
                new Vector2(100, 100),
                new Size(128, 24),
                () => { Graphics.Gamma = 1.0f; }
            );

            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _tex = new Texture(Path.Combine(loc, "dvd.png"));
            _tex.ColorMask = Color.White;
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            _str += e.Text;
            Window.Properties.Title = _str;
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.CornflowerBlue);
            context.Rectangle(ShapeMode.Fill, _position, new Size(32, 32), _color);

            _button.Draw(context);

            for (var x = 0; x < 32; x++)
            {
                for (var y = 0; y < 32; y++)
                {
                    context.DrawTexture(
                        _tex,
                        new Vector2(
                            x * _tex.Size.Width,
                            y * _tex.Size.Height
                        )
                    );
                }
            }
        }

        protected override void MousePressed(MouseButtonEventArgs e)
        {
            _button.OnMousePressed(e);
        }

        protected override void MouseReleased(MouseButtonEventArgs e)
        {
            _button.OnMouseReleased(e);
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _button.OnMouseMoved(e);
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

            Window.Properties.Title = $"{Window.FPS} | {Window.DrawCallsThisFrame} draw calls";
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