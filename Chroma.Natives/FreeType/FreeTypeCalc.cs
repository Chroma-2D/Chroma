namespace Chroma.Natives.FreeType
{
    /// <summary>
    /// Contains utility methods for converting FreeType2 data types to CLR data types, and vice versa.
    /// </summary>
    public static class FreeTypeCalc
    {
        /// <summary>
        /// Converts a <see cref="int"/> value to FreeType2 26.6 fixed point value.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static int Int32ToF26Dot6(int x) { return x * 64; }

        /// <summary>
        /// Converts a FreeType 26.6 fixed point value to a <see cref="int"/> value.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static int F26Dot6ToInt32(int x) { return x / 64; }

        /// <summary>
        /// Converts a FreeType 26.6 fixed point value to a <see cref="int"/> value.
        /// </summary>
        /// <param name="x">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public static int F26Dot6ToInt32(long x) { return (int)(x / 64L); }
    }
}
