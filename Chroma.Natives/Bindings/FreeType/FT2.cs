using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType
{
    internal static class FT2
    {
        public const string LibraryName = "freetype";

        [Flags]
        public enum FT_Load_Flags
        {
            FT_LOAD_DEFAULT = 0,
            FT_LOAD_NO_SCALE = 1 << 0,
            FT_LOAD_NO_HINTING = 1 << 1,
            FT_LOAD_RENDER = 1 << 2,
            FT_LOAD_NO_BITMAP = 1 << 3,
            FT_LOAD_VERTICAL_LAYOUT = 1 << 4,
            FT_LOAD_FORCE_AUTOHINT = 1 << 5,
            FT_LOAD_CROP_BITMAP = 1 << 6,
            FT_LOAD_PEDANTIC = 1 << 7,
            FT_LOAD_IGNORE_GLOBAL_ADVANCE_WIDTH = 1 << 9,
            FT_LOAD_NO_RECURSE = 1 << 10,
            FT_LOAD_IGNORE_TRANSFORM = 1 << 11,
            FT_LOAD_MONOCHROME = 1 << 12,
            FT_LOAD_LINEAR_DESIGN = 1 << 13,
            FT_LOAD_NO_AUTOHINT = 1 << 15,
            FT_LOAD_TARGET_NORMAL = (0 & 15) << 16,
            FT_LOAD_TARGET_LIGHT = (1 & 15) << 16,
            FT_LOAD_TARGET_MONO = (2 & 15) << 16,
            FT_LOAD_TARGET_LCD = (3 & 15) << 16,
            FT_LOAD_TARGET_LCD_V = (4 & 15) << 16,
            FT_LOAD_COLOR = 1 << 20,
            FT_LOAD_COMPUTE_METRICS = 1 << 21,
            FT_LOAD_BITMAP_METRICS_ONLY = 1 << 22
        }

        public enum FT_Kerning_Mode
        {
            FT_KERNING_DEFAULT = 0,
            FT_KERNING_UNFITTED = 1,
            FT_KERNING_UNSCALED = 2
        }

        public enum FT_Glyph_Format : uint
        {
            FT_GLYPH_FORMAT_NONE = 0,

            FT_GLYPH_FORMAT_COMPOSITE = ((uint)'c' << 24) | ((uint)'o' << 16) | ((uint)'m' << 8) | 'p',
            FT_GLYPH_FORMAT_BITMAP = ((uint)'b' << 24) | ((uint)'i' << 16) | ((uint)'t' << 8) | 's',
            FT_GLYPH_FORMAT_OUTLINE = ((uint)'o' << 24) | ((uint)'u' << 16) | ((uint)'t' << 8) | 'l',
            FT_GLYPH_FORMAT_PLOTTER = ((uint)'p' << 24) | ((uint)'l' << 16) | ((uint)'o' << 8) | 't',
        }

        public enum FT_Encoding : uint
        {
            FT_ENCODING_NONE = 0,

            FT_ENCODING_MS_SYMBOL = ((uint)'s' << 24) | ((uint)'y' << 16) | ((uint)'m' << 8) | 'b',
            FT_ENCODING_UNICODE = ((uint)'u' << 24) | ((uint)'n' << 16) | ((uint)'i' << 8) | 'c',

            FT_ENCODING_SJIS = ((uint)'s' << 24) | ((uint)'j' << 16) | ((uint)'i' << 8) | 's',
            FT_ENCODING_PRC = ((uint)'g' << 24) | ((uint)'b' << 16) | ((uint)' ' << 8) | ' ',
            FT_ENCODING_BIG5 = ((uint)'b' << 24) | ((uint)'i' << 16) | ((uint)'g' << 8) | '5',
            FT_ENCODING_WANSUNG = ((uint)'w' << 24) | ((uint)'a' << 16) | ((uint)'n' << 8) | 's',
            FT_ENCODING_JOHAB = ((uint)'j' << 24) | ((uint)'o' << 16) | ((uint)'h' << 8) | 'a',

            FT_ENCODING_GB2312 = FT_ENCODING_PRC,
            FT_ENCODING_MS_SJIS = FT_ENCODING_SJIS,
            FT_ENCODING_MS_GB2312 = FT_ENCODING_PRC,
            FT_ENCODING_MS_BIG5 = FT_ENCODING_BIG5,
            FT_ENCODING_MS_WANSUNG = FT_ENCODING_WANSUNG,
            FT_ENCODING_MS_JOHAB = FT_ENCODING_JOHAB,

            FT_ENCODING_ADOBE_STANDARD = ((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'O' << 8) | 'B',
            FT_ENCODING_ADOBE_EXPERT = ((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'B' << 8) | 'E',
            FT_ENCODING_ADOBE_CUSTOM = ((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'B' << 8) | 'C',
            FT_ENCODING_ADOBE_LATIN_1 = ((uint)'l' << 24) | ((uint)'a' << 16) | ((uint)'t' << 8) | '1',

            FT_ENCODING_OLD_LATIN_2 = ((uint)'l' << 24) | ((uint)'a' << 16) | ((uint)'t' << 8) | '2',

            FT_ENCODING_APPLE_ROMAN = ((uint)'a' << 24) | ((uint)'r' << 16) | ((uint)'m' << 8) | 'n',
        }

        public enum FT_Error
        {
            FT_Err_Ok = 0x00,
            FT_Err_Cannot_Open_Resource = 0x01,
            FT_Err_Unknown_File_Format = 0x02,
            FT_Err_Invalid_File_Format = 0x03,
            FT_Err_Invalid_Version = 0x04,
            FT_Err_Lower_Module_Version = 0x05,
            FT_Err_Invalid_Argument = 0x06,
            FT_Err_Unimplemented_Feature = 0x07,
            FT_Err_Invalid_Table = 0x08,
            FT_Err_Invalid_Offset = 0x09,
            FT_Err_Array_Too_Large = 0x0A,
            FT_Err_Missing_Module = 0x0B,
            FT_Err_Missing_Property = 0x0C,
            FT_Err_Invalid_Glyph_Index = 0x10,
            FT_Err_Invalid_Character_Code = 0x11,
            FT_Err_Invalid_Glyph_Format = 0x12,
            FT_Err_Cannot_Render_Glyph = 0x13,
            FT_Err_Invalid_Outline = 0x14,
            FT_Err_Invalid_Composite = 0x15,
            FT_Err_Too_Many_Hints = 0x16,
            FT_Err_Invalid_Pixel_Size = 0x17,
            FT_Err_Invalid_Handle = 0x20,
            FT_Err_Invalid_Library_Handle = 0x21,
            FT_Err_Invalid_Driver_Handle = 0x22,
            FT_Err_Invalid_Face_Handle = 0x23,
            FT_Err_Invalid_Size_Handle = 0x24,
            FT_Err_Invalid_Slot_Handle = 0x25,
            FT_Err_Invalid_CharMap_Handle = 0x26,
            FT_Err_Invalid_Cache_Handle = 0x27,
            FT_Err_Invalid_Stream_Handle = 0x28,
            FT_Err_Too_Many_Drivers = 0x30,
            FT_Err_Too_Many_Extensions = 0x31,
            FT_Err_Out_Of_Memory = 0x40,
            FT_Err_Unlisted_Object = 0x41,
            FT_Err_Cannot_Open_Stream = 0x51,
            FT_Err_Invalid_Stream_Seek = 0x52,
            FT_Err_Invalid_Stream_Skip = 0x53,
            FT_Err_Invalid_Stream_Read = 0x54,
            FT_Err_Invalid_Stream_Operation = 0x55,
            FT_Err_Invalid_Frame_Operation = 0x56,
            FT_Err_Nested_Frame_Access = 0x57,
            FT_Err_Invalid_Frame_Read = 0x58,
            FT_Err_Raster_Uninitialized = 0x60,
            FT_Err_Raster_Corrupted = 0x61,
            FT_Err_Raster_Overflow = 0x62,
            FT_Err_Raster_Negative_Height = 0x63,
            FT_Err_Too_Many_Caches = 0x70,
            FT_Err_Invalid_Opcode = 0x80,
            FT_Err_Too_Few_Arguments = 0x81,
            FT_Err_Stack_Overflow = 0x82,
            FT_Err_Code_Overflow = 0x83,
            FT_Err_Bad_Argument = 0x84,
            FT_Err_Divide_By_Zero = 0x85,
            FT_Err_Invalid_Reference = 0x86,
            FT_Err_Debug_OpCode = 0x87,
            FT_Err_ENDF_In_Exec_Stream = 0x88,
            FT_Err_Nested_DEFS = 0x89,
            FT_Err_Invalid_CodeRange = 0x8A,
            FT_Err_Execution_Too_Long = 0x8B,
            FT_Err_Too_Many_Function_Defs = 0x8C,
            FT_Err_Too_Many_Instruction_Defs = 0x8D,
            FT_Err_Table_Missing = 0x8E,
            FT_Err_Horiz_Header_Missing = 0x8F,
            FT_Err_Locations_Missing = 0x90,
            FT_Err_Name_Table_Missing = 0x91,
            FT_Err_CMap_Table_Missing = 0x92,
            FT_Err_Hmtx_Table_Missing = 0x93,
            FT_Err_Post_Table_Missing = 0x94,
            FT_Err_Invalid_Horiz_Metrics = 0x95,
            FT_Err_Invalid_CharMap_Format = 0x96,
            FT_Err_Invalid_PPem = 0x97,
            FT_Err_Invalid_Vert_Metrics = 0x98,
            FT_Err_Could_Not_Find_Context = 0x99,
            FT_Err_Invalid_Post_Table_Format = 0x9A,
            FT_Err_Invalid_Post_Table = 0x9B,
            FT_Err_DEF_In_Glyf_Bytecode = 0x9C,
            FT_Err_Missing_Bitmap = 0x9D,
            FT_Err_Syntax_Error = 0xA0,
            FT_Err_Stack_Underflow = 0xA1,
            FT_Err_Ignore = 0xA2,
            FT_Err_No_Unicode_Glyph_Name = 0xA3,
            FT_Err_Glyph_Too_Big = 0xA4,
            FT_Err_Missing_Startfont_Field = 0xB0,
            FT_Err_Missing_Font_Field = 0xB1,
            FT_Err_Missing_Size_Field = 0xB2,
            FT_Err_Missing_Fontboundingbox_Field = 0xB3,
            FT_Err_Missing_Chars_Field = 0xB4,
            FT_Err_Missing_Startchar_Field = 0xB5,
            FT_Err_Missing_Encoding_Field = 0xB6,
            FT_Err_Missing_Bbx_Field = 0xB7,
            FT_Err_Bbx_Too_Big = 0xB8,
            FT_Err_Corrupted_Font_Header = 0xB9,
            FT_Err_Corrupted_Font_Glyphs = 0xBA,
        }
        
        public enum FT_Render_Mode
        {
            FT_RENDER_MODE_NORMAL,
            FT_RENDER_MODE_LIGHT,
            FT_RENDER_MODE_MONO,
            FT_RENDER_MODE_LCD,
            FT_RENDER_MODE_LCD_V,

            FT_RENDER_MODE_MAX
        }
        
        public enum FT_Glyph_BBox_Mode
        {
            FT_GLYPH_BBOX_UNSCALED = 0,
            FT_GLYPH_BBOX_SUBPIXELS = 0,
            FT_GLYPH_BBOX_GRIDFIT = 1,
            FT_GLYPH_BBOX_TRUNCATE = 2,
            FT_GLYPH_BBOX_PIXELS = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Generic
        {
            public nint data;
            public nint finalizer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Glyph_Metrics
        {
            public long width;
            public long height;

            public IntPtr horiBearingX;
            public IntPtr horiBearingY;
            public long horiAdvance;

            public long vertBearingX;
            public long vertBearingY;
            public long vertAdvance;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Vector
        {
            public IntPtr x;
            public IntPtr y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Bitmap
        {
            public uint rows;
            public uint width;
            public int pitch;
            public unsafe byte* buffer;
            public ushort num_grays;
            public byte pixel_mode;
            public byte palette_mode;
            public unsafe void* palette;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Outline
        {
            public short n_contours;
            public short n_points;

            public unsafe FT_Vector* points;
            public unsafe byte* tags;
            public unsafe short* contours;

            public int flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_GlyphSlotRec
        {
            public IntPtr library;
            public unsafe FT_FaceRec* face;
            public unsafe FT_GlyphSlotRec* next;
            public uint glyph_index;
            public FT_Generic generic;

            public FT_Glyph_Metrics metrics;
            public IntPtr linearHoriAdvance;
            public IntPtr linearVertAdvance;
            public FT_Vector advance;

            public FT_Glyph_Format format;

            public FT_Bitmap bitmap;
            public int bitmap_left;
            public int bitmap_top;

            public FT_Outline outline;

            public uint num_subglyphs;
            public unsafe IntPtr* subglyphs;

            public unsafe void* control_data;
            public IntPtr control_len;

            public IntPtr lsb_delta;
            public IntPtr rsb_delta;

            public IntPtr other;
            
            private IntPtr @internal;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Bitmap_Size
        {
            public short height;
            public short width;

            public IntPtr size;

            public IntPtr x_ppem;
            public IntPtr y_ppem;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_CharMapRec
        {
            public unsafe FT_FaceRec* face;
            public FT_Encoding encoding;
            public ushort platform_id;
            public ushort encoding_id;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_BBox
        {
            public IntPtr xMin;
            public IntPtr yMin;
            public IntPtr xMax;
            public IntPtr yMax;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Size_Metrics
        {
            public ushort x_ppem;
            public ushort y_ppem;

            public IntPtr x_scale;
            public IntPtr y_scale;

            public IntPtr ascender;
            public IntPtr descender;
            public IntPtr height;
            public IntPtr max_advance;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_SizeRec
        {
            public unsafe FT_FaceRec* face;
            public FT_Generic generic;
            public FT_Size_Metrics metrics;
            
            private IntPtr @internal;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FT_ListRec
        {
            public IntPtr head;
            public IntPtr tail;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_FaceRec
        {
            public long num_faces;
            public long face_index;

            public long face_flags;
            public long style_flags;

            public long num_glyphs;

            public IntPtr family_name;
            public IntPtr style_name;

            public int num_fixed_sizes;
            public unsafe FT_Bitmap_Size* available_sizes;

            public int num_charmaps;
            public unsafe FT_CharMapRec** charmaps;

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

            public unsafe FT_GlyphSlotRec* glyph;
            public unsafe FT_SizeRec* size;
            public unsafe FT_CharMapRec* charmap;

            private IntPtr driver;
            private IntPtr memory;
            private IntPtr stream;
            
            private FT_ListRec sizes_list;
            
            private FT_Generic autohint;
            private IntPtr extensions;
            
            private IntPtr @internal;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_GlyphRec
        {
            public IntPtr library;
            private unsafe void* clazz;
            public FT_Glyph_Format format;
            public FT_Vector advance;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Library_Version(IntPtr library, out int major, out int minor, out int patch);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Init_FreeType(out IntPtr library);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe FT_Error FT_New_Memory_Face(
            IntPtr library,
            byte* file_base,
            int file_size,
            int face_index,
            out FT_FaceRec* face
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe FT_Error FT_Done_Face(FT_FaceRec* face);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe FT_Error FT_Set_Pixel_Sizes(FT_FaceRec* face, uint pixel_width, uint pixel_height);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe uint FT_Get_Char_Index(FT_FaceRec* face, uint charcode);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe FT_Error FT_Get_Kerning(
            FT_FaceRec* face,
            uint left_glyph,
            uint right_glyph,
            uint kern_mode,
            out FT_Vector akerning
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe FT_Error FT_Load_Glyph(
            FT_FaceRec* face,
            uint glyph_index,
            int load_flags
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe FT_Error FT_Render_Glyph(FT_GlyphSlotRec* slot, FT_Render_Mode render_mode);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe FT_Error FT_Get_Glyph(FT_GlyphSlotRec* slot, out FT_GlyphRec aglyph);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void FT_Done_Glyph(ref FT_GlyphRec glyph);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern FT_Error FT_Glyph_Get_CBox(ref FT_GlyphRec glyph, uint bbox_mode, out FT_BBox acbox);
        
    }
}