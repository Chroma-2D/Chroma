using System.Runtime.InteropServices;
using FT_Pos = System.IntPtr;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_Glyph_Metrics
    {
        public FT_Pos width;
        public FT_Pos height;

        public FT_Pos horiBearingX;
        public FT_Pos horiBearingY;
        public FT_Pos horiAdvance;

        public FT_Pos vertBearingX;
        public FT_Pos vertBearingY;
        public FT_Pos vertAdvance;
    }
}