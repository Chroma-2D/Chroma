namespace Chroma.Natives.FreeType
{
    internal static class FreeTypeCalc
    {
        public static int Int32ToF26Dot6(int x) => x * 64;
        public static int F26Dot6ToInt32(int x) => x / 64;
        public static int F26Dot6ToInt32(long x) => (int)(x / 64L);
    }
}
