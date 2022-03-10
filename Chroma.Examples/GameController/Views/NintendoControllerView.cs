using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input.GameControllers;
using Chroma.Input.GameControllers.Drivers;
using Chroma.Input.GameControllers.Drivers.Capabilities;
using Chroma.Input.GameControllers.Drivers.Nintendo;
using Chroma.Windowing;

namespace GameController.Views
{
    public class NintendoControllerView : GenericControllerView
    {
        public override string ViewName { get; } = "Nintendo controllers";
        public override Vector2 PositionOnScreen => new(_window.Width / 2f, 0);

        public override List<ControllerType> AcceptedControllers { get; } = new()
        {
            ControllerType.NintendoSwitch
        };

        public NintendoControllerView(Window window)
            : base(window)
        {
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            foreach (var kvp in _controllers)
            {
                if (kvp.Key is IAccelerometerEnabled accel)
                {
                    if (accel.AccelerometerEnabled)
                        kvp.Value.Accelerometer = Vector3.Normalize(accel.ReadAccelerometerSensor());
                    else kvp.Value.Accelerometer = Vector3.Zero;
                }
                if (kvp.Key is IGyroscopeEnabled gyro)
                {
                    if (gyro.GyroscopeEnabled)
                        kvp.Value.Gyroscope = Vector3.Normalize(gyro.ReadGyroscopeSensor());
                    else kvp.Value.Gyroscope = Vector3.Zero;
                }
            }
        }

        protected override void DrawViewSpecific(ControllerDriver controller, PlayerView player, RenderContext context)
        {
            if (controller is SwitchJoyConControllerDriver joycon)
            {
                var label = (joycon.IsLeftSide ? "L" : "R") + (joycon.UseInputRemapping ? "" : "*");
                var labelMeasure = TrueTypeFont.Default.Measure(label);
                context.DrawString(label, new Vector2(
                    player.Rectangle.X +
                    (joycon.IsLeftSide ? -16 : player.Rectangle.Width + 16) - (labelMeasure.Width / 2),
                    player.Rectangle.Y + (player.Rectangle.Height / 2f - labelMeasure.Height / 2f)
                ));
            }

            if (controller is NintendoControllerDriver { UseXboxButtonLayout: true })
            {
                context.DrawString("X", new Vector2(player.Rectangle.X, player.Rectangle.Y - 5), Color.Black);
            }

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
        }

        public override void OnButtonPressed(ControllerButtonEventArgs e)
        {
            if (e.Button is ControllerButton.Logo or ControllerButton.Special
                && e.Controller is SwitchJoyConControllerDriver joycon)
            {
                joycon.UseInputRemapping = !joycon.UseInputRemapping;
            }

            if (e.Button is ControllerButton.Menu && e.Controller is NintendoControllerDriver nintendoController)
            {
                nintendoController.UseXboxButtonLayout = !nintendoController.UseXboxButtonLayout;
            }

            if (e.Button is ControllerButton.LeftStick)
            {
                if (e.Controller is SwitchProControllerDriver pro)
                {
                    pro.GyroscopeEnabled = !pro.GyroscopeEnabled;
                    pro.AccelerometerEnabled = !pro.AccelerometerEnabled;
                }
                else if (e.Controller is SwitchJoyConControllerDriver con)
                {
                    con.GyroscopeEnabled = !con.GyroscopeEnabled;
                    con.AccelerometerEnabled = !con.AccelerometerEnabled;
                }
            }
        }
    }
}