using System.Runtime.InteropServices;
using FT_Pos = System.IntPtr;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_BBox
    {
        public FT_Pos xMin, yMin;
        public FT_Pos xMax, yMax;
    }
}