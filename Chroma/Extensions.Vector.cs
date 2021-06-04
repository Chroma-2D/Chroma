using System.Drawing;
using System.Numerics;

namespace Chroma
{
    public static partial class Extensions
    {
        public static Point ToPoint(this Vector2 v)
            => new((int)v.X, (int)v.Y);
    }
}