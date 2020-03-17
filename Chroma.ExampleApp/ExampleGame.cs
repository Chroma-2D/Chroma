using System;
using Chroma.Graphics;
using Chroma.Input;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private byte Red { get; set; }
        private Color Color { get; set; }

        public ExampleGame()
        {
            Window.GoWindowed(1024, 600);
            GraphicsManager.Instance.VSyncEnabled = false;

            Mouse.Moved += (sender, e) =>
            {
                Window.Properties.Title = $"{e.Position.X}, {e.Position.Y}";
            };

            Mouse.WheelMoved += (sender, e) =>
            {
                Red += (byte)(e.Motion.Y % byte.MaxValue);
                Color = new Color(Red, 0, 0);
            };

            Keyboard.KeyDown += (sender, e) => Console.WriteLine(e.Scancode);
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color);
        }

        protected override void Update(float delta)
        {
            // Window.Properties.Title = Window.FPS.ToString();
        }
    }
}
