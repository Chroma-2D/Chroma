namespace Pong;

using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

public class Paddle
{
    private Board _board;

    public RectangleF Rectangle;
    public Color Color = Color.White;
    public float Acceleration = 200f;

    public Vector2 Position
    {
        get => new(Rectangle.X, Rectangle.Y);
        set
        {
            Rectangle.X = value.X;
            Rectangle.Y = value.Y;
        }
    }

    public Vector2 Size => new(Rectangle.Width, Rectangle.Height);

    public KeyCode UpKey = KeyCode.Up;
    public KeyCode DownKey = KeyCode.Down;

    public Paddle(Board board)
    {
        Rectangle = new Rectangle(
            16, 16, 20, 120
        );

        _board = board;
    }

    public void Update(float dt)
    {
        if (Keyboard.IsKeyDown(UpKey))
        {
            if (Rectangle.Y - (Acceleration * dt) > 0)
            {
                Rectangle.Y -= Acceleration * dt;
            }
            else
            {
                Rectangle.Y = 0;
            }
        }
        else if (Keyboard.IsKeyDown(DownKey))
        {
            if (Rectangle.Y + (Acceleration * dt) < _board.Size.Height - Rectangle.Height)
            {
                Rectangle.Y += Acceleration * dt;
            }
            else
            {
                Rectangle.Y = _board.Size.Height - Rectangle.Height;
            }
        }
    }

    public void Draw(RenderContext context)
    {
        context.Rectangle(
            ShapeMode.Fill,
            Rectangle,
            Color
        );
    }
}