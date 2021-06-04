using System;
using System.Collections.Generic;

namespace Chroma
{
    public static partial class Extensions
    {
        public static string AnsiColorEncodeRGB(this string s, byte r, byte g, byte b)
            => $"\x1b[38;2;{r};{g};{b}m{s}\x1b[0m";

        public static List<Range> FindWordRanges(this string str, params string[] highlights)
        {
            var ranges = new List<Range>();

            foreach (var highlight in highlights)
            {
                var index = str.IndexOf(highlight, StringComparison.Ordinal);

                if (index >= 0)
                    ranges.Add(index..(index + highlight.Length));
            }

            return ranges;
        }
    }
}