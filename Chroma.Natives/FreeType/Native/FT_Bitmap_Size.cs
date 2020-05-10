using System;
using System.Runtime.InteropServices;
using FT_Pos = System.IntPtr;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_Bitmap_Size
    {
        public short height;
        public short width;

        public FT_Pos size;

        public FT_Pos x_ppem;
        public FT_Pos y_ppem;
    }
}
