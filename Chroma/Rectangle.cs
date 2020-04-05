using System;

namespace Chroma
{
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle();

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Vector2 Location
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Size
        {
            get => new Vector2(Width, Height);
            set
            {
                Width = value.X;
                Height = value.Y;
            }
        }
        
        public Rectangle(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rectangle rect))
                return false;

            return (rect.X == X) &&
                   (rect.Y == Y) &&
                   (rect.Width == Width) &&
                   (rect.Height == Height);
        }

        public bool Contains(float x, float y)
            => X <= x &&
               Y <= y &&
               x < X + Width &&
               y < Y + Height;

        public bool Contains(Vector2 v)
            => Contains(v.X, v.Y);

        public bool Contains(Rectangle other)
            => X <= other.X &&
               Y <= other.Y &&
               (other.X + other.Width) <= (X + Width) &&
               (other.Y + other.Height) <= (Y + Height);

        public bool IntersectsWith(Rectangle other)
            => (other.X < X + Width) &&
               (other.Y < Y + Height) &&
               (X < (other.X + other.Width)) &&
               (Y < other.Y + other.Height);

        public static Rectangle Intersection(Rectangle a, Rectangle b)
        {
            var x1 = Math.Max(a.X, b.X);
            var x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            var y1 = Math.Max(a.Y, b.Y);
            var y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);

            return Empty;
        }

        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            var x1 = Math.Min(a.X, b.X);
            var x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            var y1 = Math.Min(a.Y, b.Y);
            var y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        // taken directly from system.drawing.rectangle
        public override int GetHashCode()
            => unchecked(
                (int)(
                    (uint)X ^ 
                    (((uint)Y << 13) | ((uint)Y >> 19)) ^
                    (((uint)Width << 26) | ((uint)Width >> 6)) ^
                    (((uint)Height << 7) | ((uint)Height >> 25))
                )
            );

        public static bool operator ==(Rectangle left, Rectangle right)
            => left.X == right.X && 
               left.Y == right.Y && 
               left.Width == right.Width && 
               left.Height == right.Height;

        public static bool operator !=(Rectangle left, Rectangle right)
            => !(left == right);
    }
}
