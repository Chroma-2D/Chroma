using Chroma.Input.GameControllers;
using System.Drawing;
using System.Numerics;
using Color = Chroma.Graphics.Color;

namespace GameController
{
    public class PlayerView
    {
        public RectangleF Rectangle;
        public float TriggerLeft;
        public float TriggerRight;
        public float RightStickX;
        public float RightStickY;
        public Vector3 Accelerometer;
        public Vector3 Gyroscope;
        public ControllerTouchPoint[] TouchPoints;
        public Color Color = Color.White;

        public PlayerView(float x, float y, int width, int height)
        {
            Rectangle = new RectangleF(x, y, width, height);
        }
    }
}