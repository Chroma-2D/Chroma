using System;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Color = Chroma.Graphics.Color;

namespace Pong
{
    public class Ball
    {
        private readonly Board _board;
        private readonly Random _random;

        public bool CanMove = false;
        public RectangleF Rectangle;
        public Vector2 Direction = new(1, -1);
        public Vector2 Speed = new(200);
        public Color Color = Color.Lime;

        public Ball(Board board)
        {
            _board = board;
            _random = new Random();

            Rectangle = new(
                _board.Size.Width / 2f - 12,
                _board.Size.Height / 2f - 12,
                24, 24
            );
        }

        public void Update(float dt)
        {
            if (CanMove)
            {
                Rectangle.X += Speed.X * Direction.X * dt;
                Rectangle.Y += Speed.Y * Direction.Y * dt;
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

        public bool CollidesWithPaddle(Paddle p)
        {
            return Rectangle.IntersectsWith(p.Rectangle);
        }

        public bool CollidesWithBoardSide()
        {
            return Rectangle.Y + Rectangle.Height >= _board.Size.Height
                   || Rectangle.Y <= 0;
        }

        public bool CollidesWithEndOfPlayfield(out bool left)
        {
            left = Rectangle.X <= 0;

            return Rectangle.X + Rectangle.Width >= _board.Size.Width
                   || left;
        }

        private RectangleF Intersection(RectangleF r1, RectangleF r2)
        {
            var intersect = new RectangleF(
                r1.X, r1.Y, r1.Width, r1.Height
            );

            intersect.Intersect(r2);

            return intersect;
        }

        public void BounceFromPaddle(Paddle p)
        {
            var intersection = Intersection(Rectangle, p.Rectangle);

            if (Rectangle.X > _board.Size.Width / 2)
            {
                Rectangle.X -= intersection.Width;
            }
            else
            {
                Rectangle.X += intersection.Width;
            }

            Direction.X = -Direction.X;

            Speed.X = _random.Next(150, 300);
            Speed.Y = _random.Next(150, 300);
        }

        public void BounceFromSide()
        {
            Direction.Y = -Direction.Y;
        }

        public void Center()
        {
            Rectangle.X = _board.Size.Width / 2f;
            Rectangle.Y = _board.Size.Height / 2f;
        }
    }
}