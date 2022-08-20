using System.Drawing;
using System.Numerics;

namespace Chroma
{
    public static partial class Extensions
    {
        public static Vector2 ToVector(this Point p)
            => new(p.X, p.Y);
    }
}