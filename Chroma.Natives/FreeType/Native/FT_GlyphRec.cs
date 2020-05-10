using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_GlyphRec
    {
        public FT_LibraryRec* library;
        public IntPtr clazz;
        public FT_Glyph_Format format;
        public FT_Vector advance;
    }
}
