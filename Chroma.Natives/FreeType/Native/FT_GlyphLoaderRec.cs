using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_GlyphLoaderRec
    {
        public IntPtr memory;
        public uint max_points;
        public uint max_contours;
        public uint max_subglyphs;
        public bool use_extra;

        public FT_GlyphLoadRec @base;
        public FT_GlyphLoadRec current;

        public IntPtr other;
    }
}