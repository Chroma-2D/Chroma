using System.Drawing;
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
        public Color Color = Color.White;

        public PlayerView(float x, float y, int width, int height)
        {
            Rectangle = new RectangleF(x, y, width, height);
        }
    }
}