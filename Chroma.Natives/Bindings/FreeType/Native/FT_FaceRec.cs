using System.Runtime.InteropServices;
using FT_Long = System.IntPtr;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_FaceRec
    {
        public FT_Long num_faces;
        public FT_Long face_index;

        public FT_Long face_flags;
        public FT_Long style_flags;

        public FT_Long num_glyphs;

        public FT_Long family_name;
        public FT_Long style_name;

        public int num_fixed_sizes;
        public FT_Bitmap_Size* available_sizes;

        public int num_charmaps;
        public FT_CharMapRec** charmaps;

        public FT_Generic generic;

        public FT_BBox bbox;

        public ushort units_per_EM;
        public short ascender;
        public short descender;
        public short height;

        public short max_advance_width;
        public short max_advance_height;

        public short underline_position;
        public short underline_thickness;

        public FT_GlyphSlotRec* glyph;
        public FT_SizeRec* size;
        public FT_CharMapRec* charmap;

        public FT_Long driver;
        public FT_Long memory;
        public FT_Long stream;

        public FT_ListRec sizes_list;

        public FT_Generic autohint;
        public FT_Long extensions;

        public FT_Long @internal;
    }
}