using System.Collections.Generic;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input.GameControllers;
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

        public override void OnTouchpadMoved(ControllerTouchpadEventArgs e)
        {
        }

        public override void OnTouchpadTouched(ControllerTouchpadEventArgs e)
        {
        }

        public override void OnTouchpadReleased(ControllerTouchpadEventArgs e)
        {
        }

        public override void OnSensorStateChanged(ControllerSensorEventArgs e)
        {
        }
    }
}