namespace Chroma;

using System;

public static partial class Extensions
{
    public static bool Includes(this Range range, int number)
        => range.Start.Value <= number && range.End.Value > number;
}