using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_SizeRec
    {
        public FT_FaceRec* face;
        public FT_Generic generic;
        public FT_Size_Metrics metrics;
        public IntPtr @internal;
    }
}
