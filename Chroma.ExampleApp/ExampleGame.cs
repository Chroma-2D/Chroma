using System;
using Chroma.Graphics;
using Chroma.Input;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Vector2 _position;
        private float _speed = 500f;

        public ExampleGame()
        {
            Window.GoWindowed(1024, 600);
            GraphicsManager.Instance.VSyncEnabled = false;

            Mouse.Moved += (sender, e) =>
            {
                Window.Properties.Title = $"{e.Position.X}, {e.Position.Y}";
            };
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.Black);
            context.Rectangle(ShapeMode.Fill, _position, new Size(32, 32), Color.White);
        }

        protected override void Update(float delta)
        {
            var dx = 0f;
            var dy = 0f;

            if (Keyboard.IsKeyDown(KeyCode.Up))
            {
                dy = -_speed * delta;
            }
            else if (Keyboard.IsKeyDown(KeyCode.Down))
            {
                dy = _speed * delta;
            }

            if (Keyboard.IsKeyDown(KeyCode.Left))
            {
                dx = -_speed * delta;
            }
            else if (Keyboard.IsKeyDown(KeyCode.Right))
            {
                dx = _speed * delta;
            }

            _position = new Vector2(_position.X + dx, _position.Y + dy);
        }
    }
}
