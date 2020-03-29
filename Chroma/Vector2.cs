using System;

namespace Chroma
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public readonly float X;
        public readonly float Y;

        public static readonly Vector2 Up = new Vector2(0, -1);
        public static readonly Vector2 Down = new Vector2(0, 1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Zero = new Vector2(0, 0);

        public Vector2 Normalized
        {
            get
            {
                var mag = Magnitude;
                return new Vector2(X / mag, Y / mag);
            }
        }

        public float Magnitude
            => (float)Math.Sqrt(
                Math.Pow(X, 2) +
                Math.Pow(Y, 2)
            );

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(float uniform)
        {
            X = Y = uniform;
        }

        public Vector2(Vector2 other)
        {
            X = other.X;
            Y = other.Y;
        }

        public float DistanceTo(Vector2 target)
            => Distance(this, target);

        public bool Equals(Vector2 other)
            => X.Equals(other.X) &&
               Y.Equals(other.Y);

        public override bool Equals(object obj)
            => obj is Vector2 other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(X, Y);

        public override string ToString()
            => $"[{X}, {Y}]";

        public static float Distance(Vector2 a, Vector2 b)
            => (a - b).Magnitude;

        public static Vector2 operator *(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.X * right.X,
                left.Y * right.Y
            );
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.X + right.X,
                left.Y + right.Y
            );
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(
                left.X - right.X,
                left.Y - right.Y
            );
        }

        public static Vector2 operator *(Vector2 left, float right)
            => new Vector2(left.X * right, left.Y * right);

        public static Vector2 operator /(Vector2 left, float right)
            => new Vector2(left.X / right, left.Y / right);

        public static bool operator ==(Vector2 left, Vector2 right)
            => left.X.Equals(right.X) && left.Y.Equals(right.Y);

        public static bool operator !=(Vector2 left, Vector2 right)
            => !(left == right);
    }
}