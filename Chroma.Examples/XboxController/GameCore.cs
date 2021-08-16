using System;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.GameControllers;
using Chroma.Input.GameControllers.Drivers;

namespace XboxController
{
    public class GameCore : Game
    {
        private RenderTarget _tgt;

        private float _scale = 1.0f;
        private float _rotation;
        private Vector2 _position = new(32);
        private Color _color = Color.White;
        private Color _bgColor = Color.Black;
        private ShapeMode _mode;
        
        public GameCore() : base(new(false, false))
        {
        }

        protected override void LoadContent()
        {
            _tgt = new RenderTarget(32, 32);
            _tgt.FilteringMode = TextureFilteringMode.NearestNeighbor;
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(_bgColor);
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Transparent);
                context.Rectangle(
                    _mode,
                    Vector2.Zero,
                    32, 32,
                    _color
                );
            });

            context.DrawTexture(_tgt, _position + _tgt.Center, Vector2.One * _scale, _tgt.Center, _rotation);
            var inputs = "";
            
            for (var i = 0; i < Controller.DeviceCount; i++)
            {
                var buttonSet = Controller.GetActiveButtons(i);
                if (buttonSet == null)
                    continue;
                
                inputs += $"Controller {i}: {string.Join(", ", buttonSet)}\n";
            }
            
            context.DrawString(
                "Use left stick to control the rectangle.\n" +
                "Use right stick to control rumble.\n" +
                "Use left trigger to control rotation.\n" +
                "Use right trigger to control scale.\n" +
                "Use A/B/X/Y to control colors.\n" +
                "Use A/B/X/Y on player 2 to control background colors.\n" +
                "Use left/right stick buttons to toggle between stroke and fill.\n" +
                "Use left/right bumper to control line thickness.\n\n" +
                inputs, 
                new Vector2(8)
            );
        }

        protected override void Update(float delta)
        {
            _scale = 1.0f + 16f * Controller.GetAxisValueNormalized(0, ControllerAxis.RightTrigger);
            _rotation = 360 * Controller.GetAxisValueNormalized(0, ControllerAxis.LeftTrigger);

            _position.X += 120 * delta * Controller.GetAxisValueNormalized(0, ControllerAxis.LeftStickX);
            _position.Y += 120 * delta * Controller.GetAxisValueNormalized(0, ControllerAxis.LeftStickY);

            if (Controller.IsButtonDown(1, ControllerButton.A))
                _bgColor = Color.SlateGray;
            else if (Controller.IsButtonDown(1, ControllerButton.B))
                _bgColor = Color.DarkCyan;
            else if (Controller.IsButtonDown(1, ControllerButton.X))
                _bgColor = Color.DarkGreen;
            else if (Controller.IsButtonDown(1, ControllerButton.Y))
                _bgColor = Color.DarkRed;
            else
                _bgColor = Color.Black;

            var rumbleSide = Controller.GetAxisValueNormalized(0, ControllerAxis.RightStickX);

            var leftIntensity = 0f;
            var rightIntensity = 0f;

            if (rumbleSide < 0)
            {
                leftIntensity = MathF.Abs(rumbleSide);
            }
            else
            {
                rightIntensity = MathF.Abs(rumbleSide);
            }

            Controller.Rumble(0, (ushort)(65535 * leftIntensity), (ushort)(65535 * rightIntensity), 1000);
        }

        protected override void ControllerConnected(ControllerEventArgs e)
        {
            e.Controller.SetDeadZone(ControllerAxis.LeftStickX, 5500);
            e.Controller.SetDeadZone(ControllerAxis.LeftStickY, 5500);
            e.Controller.SetDeadZone(ControllerAxis.RightStickX, 5500);
            e.Controller.SetDeadZone(ControllerAxis.RightStickY, 5500);

            Console.WriteLine(e.Controller.Info);
        }

        protected override void ControllerButtonPressed(ControllerButtonEventArgs e)
        {
            if (e.Button == ControllerButton.X)
            {
                _color = Color.DodgerBlue;
            }
            else if (e.Button == ControllerButton.B)
            {
                _color = Color.OrangeRed;
            }
            else if (e.Button == ControllerButton.Y)
            {
                _color = Color.Yellow;
            }
            else if (e.Button == ControllerButton.A)
            {
                _color = Color.LimeGreen;
            }

            if (e.Button == ControllerButton.LeftStick)
            {
                _mode = ShapeMode.Fill;
            }
            else if (e.Button == ControllerButton.RightStick)
            {
                _mode = ShapeMode.Stroke;
            }

            if (e.Button == ControllerButton.LeftBumper)
            {
                if (RenderSettings.LineThickness - 1 >= 0)
                    RenderSettings.LineThickness -= 1;
            }
            else if (e.Button == ControllerButton.RightBumper)
            {
                RenderSettings.LineThickness += 1;
            }

            if (e.Button == ControllerButton.Logo)
            {
                if (e.Controller.Is<DualShockControllerDriver>())
                {
                    e.Controller.As<DualShockControllerDriver>().SetLED(Color.Orange);
                }
            }
        }
    }
}