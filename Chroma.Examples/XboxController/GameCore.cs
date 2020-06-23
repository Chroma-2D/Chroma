using System;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace XboxController
{
    public class GameCore : Game
    {
        private RenderTarget _tgt;

        private float _scale = 1.0f;
        private float _rotation = 0.0f;
        private Vector2 _position = new Vector2(32);
        private Color _color = Color.White;
        private ShapeMode _mode;
        private float _lineThickness = 1;

        protected override void LoadContent()
        {
            _tgt = new RenderTarget(32, 32);
            _tgt.FilteringMode = TextureFilteringMode.NearestNeighbor;
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.LineThickness = _lineThickness;
                context.Clear(Color.Transparent);
                context.Rectangle(
                    _mode,
                    Vector2.Zero,
                    32, 32,
                    _color
                );
            });

            context.DrawTexture(_tgt, _position + _tgt.Center, Vector2.One * _scale, _tgt.Center, _rotation);
            context.DrawString("Use left stick to control the rectangle.\n" +
                               "Use right stick to control rumble.\n" +
                               "Use left trigger to control rotation.\n" +
                               "Use right trigger to control scale.\n" +
                               "Use A/B/X/Y to control colors.\n" +
                               "Use left/right stick buttons to toggle between stroke and fill.\n" +
                               "Use left/right bumper to control line thickness.", new Vector2(8));
        }

        protected override void Update(float delta)
        {
            _scale = 1.0f + 16f * Controller.GetAxisValueNormalized(0, ControllerAxis.RightTrigger);
            _rotation = 360 * Controller.GetAxisValueNormalized(0, ControllerAxis.LeftTrigger);

            _position.X += 120 * delta * Controller.GetAxisValueNormalized(0, ControllerAxis.LeftStickX);
            _position.Y += 120 * delta * Controller.GetAxisValueNormalized(0, ControllerAxis.LeftStickY);

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

            Controller.Vibrate(0, (ushort)(65535 * leftIntensity), (ushort)(65535 * rightIntensity), 1000);
        }

        protected override void ControllerConnected(ControllerEventArgs e)
        {
            Controller.SetDeadZone(e.Controller.PlayerIndex, ControllerAxis.LeftStickX, 5500);
            Controller.SetDeadZone(e.Controller.PlayerIndex, ControllerAxis.LeftStickY, 5500);
            Controller.SetDeadZone(e.Controller.PlayerIndex, ControllerAxis.RightStickX, 5500);
            Controller.SetDeadZone(e.Controller.PlayerIndex, ControllerAxis.RightStickY, 5500);
        }

        protected override void ControllerButtonPressed(ControllerButtonEventArgs e)
        {
            if (e.Button == ControllerButton.X)
            {
                _color = Color.DodgerBlue;
            }
            else if (e.Button == ControllerButton.B)
            {
                _color = Color.LimeGreen;
            }
            else if (e.Button == ControllerButton.Y)
            {
                _color = Color.Yellow;
            }
            else if (e.Button == ControllerButton.A)
            {
                _color = Color.OrangeRed;
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
                _lineThickness -= 1;

                if (_lineThickness <= 0)
                    _lineThickness = 1;
            }
            else if (e.Button == ControllerButton.RightBumper)
            {
                _lineThickness += 1;
            }
        }
    }
}