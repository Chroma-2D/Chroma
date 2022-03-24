using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_SubGlyphRec
    {
        public int index;
        public ushort flags;
        public int arg1;
        public int arg2;
        public FT_Matrix transform;
    }
}