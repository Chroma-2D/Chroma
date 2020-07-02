using System;

namespace Chroma.Extensions
{
    public static class RangeExtensions
    {
        public static bool Includes(this Range range, int number)
            => range.Start.Value <= number && range.End.Value > number;
    }
}