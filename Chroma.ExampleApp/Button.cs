using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using System;

namespace Chroma.ExampleApp
{
    public class Button
    {
        public Vector2 Position { get; set; }
        public Size Size { get; set; }
        public Action Action { get; }

        public Color HoverColor { get; } = Color.LightGreen;
        public Color ClickColor { get; } = Color.Yellow;
        public Color IdleColor { get; } = Color.White;

        public Color CurrentColor { get; private set; }

        public Button(Vector2 position, Size size, Action action)
        {
            Position = position;
            Size = size;
            Action = action;

            CurrentColor = IdleColor;
        }

        public void Draw(RenderContext context)
        {
            context.Rectangle(ShapeMode.Fill, Position, Size, CurrentColor);
        }

        public void OnMouseMoved(MouseMoveEventArgs e)
        {
            if (IsMouseOver(e.Position))
            {
                if (e.ButtonState.Left)
                {
                    CurrentColor = ClickColor;
                }
                else
                {
                    CurrentColor = HoverColor;
                }
            }
            else
            {
                CurrentColor = IdleColor;
            }
        }

        public void OnMousePressed(MouseButtonEventArgs e)
        {
            if (IsMouseOver(e.Position) && e.Button == MouseButton.Left)
            {
                CurrentColor = ClickColor;
            }
        }

        public void OnMouseReleased(MouseButtonEventArgs e)
        {
            if (IsMouseOver(e.Position) && e.Button == MouseButton.Left)
            {
                Action?.Invoke();
                CurrentColor = HoverColor;
            }
        }

        private bool IsMouseOver(Vector2 position)
        {
            return position.X >= Position.X && position.X <= Position.X + Size.Width
                && position.Y >= Position.Y && position.Y <= Position.Y + Size.Height;
        }
    }
}
