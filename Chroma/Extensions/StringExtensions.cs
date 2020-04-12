namespace Chroma.Extensions
{
    public static class StringExtensions
    {
        public static string AnsiColorEncodeRGB(this string s, byte r, byte g, byte b)
            => $"\x1b[38;2;{r};{g};{b}m{s}\x1b[0m";
    }
}
