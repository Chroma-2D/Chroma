using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input.GameControllers;
using Chroma.Input.GameControllers.Drivers;
using Chroma.Windowing;

namespace GameController.Views
{
    public class DualShockControllerView : GenericControllerView
    {
        public override string ViewName => "DualShock 4 controllers";

        public override Vector2 PositionOnScreen => Vector2.Zero;

        public override List<ControllerType> AcceptedControllers { get; } = new()
        {
            ControllerType.PlayStation4
        };

        public override Dictionary<ControllerButton, Color> ButtonColors { get; } = new()
        {
            { ControllerButton.A, Color.Blue },
            { ControllerButton.B, Color.Red },
            { ControllerButton.X, Color.HotPink },
            { ControllerButton.Y, Color.Lime }
        };

        public override Color RightStickHatColor { get; } = Color.Orange;

        public DualShockControllerView(Window window) : base(window)
        {
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            foreach (var kvp in _controllers)
            {
                if (kvp.Key is DualShockControllerDriver ds4)
                {
                    if (ds4.AccelerometerEnabled)
                        kvp.Value.Accelerometer = Vector3.Normalize(ds4.ReadAccelerometerSensor());
                    else kvp.Value.Accelerometer = Vector3.Zero;

                    if (ds4.GyroscopeEnabled)
                        kvp.Value.Gyroscope = Vector3.Normalize(ds4.ReadGyroscopeSensor());
                    else kvp.Value.Gyroscope = Vector3.Zero;

                    kvp.Value.TouchPoints = ds4.TouchPoints.ToArray();
                }
            }
        }

        protected override void DrawViewSpecific(PlayerView player, RenderContext context)
        {
            if (Math.Abs(player.Accelerometer.X) > 0.03f || Math.Abs(player.Accelerometer.Z) > 0.03f)
            {
                context.Line(
                    player.Rectangle.X + player.Rectangle.Width / 2,
                    player.Rectangle.Y + player.Rectangle.Height / 2,
                    player.Rectangle.X + player.Rectangle.Width / 2 - 72 * player.Accelerometer.X,
                    player.Rectangle.Y + player.Rectangle.Width / 2 - 72 * player.Accelerometer.Z,
                    Color.Cyan
                );
            }

            if (Math.Abs(player.Gyroscope.X) > 0.03f || Math.Abs(player.Gyroscope.Z) > 0.03f)
            {
                context.Line(
                    player.Rectangle.X + player.Rectangle.Width / 2,
                    player.Rectangle.Y + player.Rectangle.Height / 2,
                    player.Rectangle.X + player.Rectangle.Width / 2 - 72 * player.Gyroscope.Y,
                    player.Rectangle.Y + player.Rectangle.Width / 2 - 72 * player.Gyroscope.X,
                    Color.Gold
                );
            }

            foreach (var touchPoint in player.TouchPoints)
            {
                if (touchPoint.Touching)
                {
                    context.Circle(
                        ShapeMode.Stroke,
                        player.Rectangle.X + player.Rectangle.Width / 2 + 128 * touchPoint.Position.X - 64,
                        player.Rectangle.Y + player.Rectangle.Height / 2 + 64 * touchPoint.Position.Y - 32,
                        8,
                        Color.Magenta
                    );
                }
            }
        }

        public override void OnButtonPressed(ControllerButtonEventArgs e)
        {
            if (e.Button == ControllerButton.Touchpad)
            {
                var ds4 = e.Controller.As<DualShockControllerDriver>();

                foreach (var tp in ds4.TouchPoints)
                {
                    if (tp.Touching)
                    {
                        if (tp.Position.X > 0.5)
                        {
                            ds4.AccelerometerEnabled = !ds4.AccelerometerEnabled;
                        }
                        else if (tp.Position.X < 0.5)
                        {
                            ds4.GyroscopeEnabled = !ds4.GyroscopeEnabled;
                        }
                        break;
                    }
                }
            }
        }
    }
}