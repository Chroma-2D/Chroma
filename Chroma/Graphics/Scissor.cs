using System;

namespace Chroma.Graphics
{
    public struct Scissor : IEquatable<Scissor>
    {
        public short X { get; }
        public short Y { get; }

        public ushort Width { get; }
        public ushort Height { get; }

        public static readonly Scissor None = new Scissor(0, 0, 0, 0);

        public Scissor(short x, short y, ushort width, ushort height)
        {
            X = x;
            Y = y;

            Width = width;
            Height = height;
        }

        public bool Equals(Scissor other)
            => X == other.X &&
               Y == other.Y &&
               Width == other.Width &&
               Height == other.Height;

        public override bool Equals(object obj)
            => obj is Scissor other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(X, Y, Width, Height);

        public static bool operator ==(Scissor a, Scissor b)
            => a.X == b.X &&
               a.Y == b.Y &&
               a.Width == b.Width &&
               a.Height == b.Height;

        public static bool operator !=(Scissor a, Scissor b)
            => !(a == b);
    }
}