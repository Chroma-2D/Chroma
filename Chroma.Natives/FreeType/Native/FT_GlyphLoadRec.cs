using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_GlyphLoadRec
    {
        public FT_Outline outline;
        public FT_Vector* extra_points;
        public FT_Vector* extra_points2;
        public uint num_subglyphs;
        public FT_SubGlyphRec* subglyphs;
    }
}