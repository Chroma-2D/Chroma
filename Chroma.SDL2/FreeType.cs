using System;
using System.Runtime.InteropServices;

namespace Chroma.SDL2
{
    public static class FreeType
    {
        #region freetype.h
        [StructLayout(LayoutKind.Sequential)]
        public struct FT_GlyphMetrics
        {
            public int width;
            public int height;

            public int horiBearingX;
            public int horiBearingY;
            public int horiAdvance;

            public int vertBearingX;
            public int vertBearingY;
            public int vertAdvance;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_BitmapSize
        {
            public short height;
            public short width;

            public int size;

            public int x_ppem;
            public int y_ppem;
        }

        public enum FT_Encoding : uint
        {
            FT_ENCODING_NONE = 0,
            FT_ENCODING_MS_SYMBOL = ('s' << 24 | 'y' << 16 | 'm' << 8 | 'b'),
            FT_ENCODING_UNICODE = ('u' << 24 | 'n' << 16 | 'i' << 8 | 'c'),
            FT_ENCODING_SJIS = ('s' << 24 | 'j' << 16 | 'i' << 8 | 's'),
            FT_ENCODING_PRC = ('g' << 24 | 'b' << 16 | ' ' << 8 | ' '),
            FT_ENCODING_BIG5 = ('b' << 24 | 'i' << 16 | 'g' << 8 | '5'),
            FT_ENCODING_WANSUNG = ('w' << 24 | 'a' << 16 | 'n' << 8 | 's'),
            FT_ENCODING_JOHAB = ('j' << 24 | 'o' << 16 | 'h' << 8 | 'a'),

            FT_ENCODING_ADOBE_STANDARD = ('A' << 24 | 'D' << 16 | 'O' << 8 | 'B'),
            FT_ENCODING_ADOBE_EXPERT = ('A' << 24 | 'D' << 16 | 'B' << 8 | 'E'),
            FT_ENCODING_ADOBE_CUSTOM = ('A' << 24 | 'D' << 16 | 'B' << 8 | 'C'),

            FT_ENCODING_ADOBE_LATIN_1 = ('l' << 24 | 'a' << 16 | 't' << 8 | '1'),
            FT_ENCODING_OLD_LATIN_2 = ('l' << 24 | 'a' << 16 | 't' << 8 | '2'),
            FT_ENCODING_APPLE_ROMAN = ('a' << 24 | 'r' << 16 | 'm' << 8 | 'n'),
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_CharMapRec
        {
            IntPtr face;
            FT_Encoding encoding;
            ushort platform_id;
            ushort encoding_id;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_FaceRec
        {
            int num_faces;
            int face_index;

            int face_flags;
            int style_flags;

            int num_glyphs;

            string family_name;
            string style_name;

            int num_fixed_sizes;
            IntPtr available_sizes;

            int num_charmaps;
            IntPtr charmaps;

            IntPtr generic;
        }

        public static uint FT_ENC_TAG(uint a, uint b, uint c, uint d)
        {
            return ((a << 24) | (b << 16) | (c << 8) | (d));
        }
        #endregion

        #region ftimage.h
        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Vector
        {
            int x;
            int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_BBox
        {
            int xMin, yMin;
            int xMax, yMax;
        }

        public enum FT_PixelMode
        {
            FT_PIXEL_MODE_NONE = 0,
            FT_PIXEL_MODE_MONO,
            FT_PIXEL_MODE_GRAY,
            FT_PIXEL_MODE_GRAY2,
            FT_PIXEL_MODE_GRAY4,
            FT_PIXEL_MODE_LCD,
            FT_PIXEL_MODE_LCD_V,
            FT_PIXEL_MODE_BGRA,
            FT_PIXEL_MODE_MAX
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Bitmap
        {
            uint rows;
            uint width;
            int pitch;
            byte[] buffer;
            ushort num_grays;
            byte pixel_mode;
            byte palette_mode;
            IntPtr palette;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Outline
        {
            short n_contours;
            short n_points;

            FT_Vector[] points;
            byte[] tags;
            short[] contours;

            int flags;

        }

        public const int FT_OUTLINE_NONE = 0x0;
        public const int FT_OUTLINE_OWNER = 0x1;
        public const int FT_OUTLINE_EVEN_ODD_FILL = 0x2;
        public const int FT_OUTLINE_REVERSE_FILL = 0x4;
        public const int FT_OUTLINE_IGNORE_DROPOUTS = 0x8;
        public const int FT_OUTLINE_SMART_DROPOUTS = 0x10;
        public const int FT_OUTLINE_INCLUDE_STUBS = 0x20;
        public const int FT_OUTLINE_HIGH_PRECISION = 0x100;
        public const int FT_OUTLINE_SINGLE_PASS = 0x200;

        public const int FT_CURVE_TAG_ON = 0x01;
        public const int FT_CURVE_TAG_CONIC = 0x00;
        public const int FT_CURVE_TAG_CUBIC = 0x02;
        public const int FT_CURVE_TAG_HAS_SCANMODE = 0x04;
        public const int FT_CURVE_TAG_TOUCH_X = 0x08;
        public const int FT_CURVE_TAG_TOUCH_Y = 0x10;

        public const int FT_CURVE_TAG_TOUCH_BOTH = (FT_CURVE_TAG_TOUCH_X |
                                                    FT_CURVE_TAG_TOUCH_Y);

        public delegate int FT_Outline_MoveToFunc(ref FT_Vector to, IntPtr user);
        public delegate int FT_Outline_LineToFunc(ref FT_Vector to, IntPtr user);
        public delegate int FT_Outline_ConicToFunc(ref FT_Vector control, ref FT_Vector to, IntPtr user);
        public delegate int FT_Outline_CubicToFunc(ref FT_Vector control1, ref FT_Vector control2, ref FT_Vector to, IntPtr user);

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Outline_Funcs
        {
            public IntPtr move_to;
            public IntPtr line_to;
            public IntPtr conic_to;
            public IntPtr cubic_to;

            int shift;
            int delta;
        }

        public enum FT_Glyph_Format : uint
        {
            FT_GLYPH_FORMAT_NONE = 0,
            FT_GLYPH_FORMAT_COMPOSITE = ('c' << 24 | 'o' << 16 | 'm' << 8 | 'p'),
            FT_GLYPH_FORMAT_BITMAP = ('b' << 24 | 'i' << 16 | 't' << 8 | 's'),
            FT_GLYPH_FORMAT_OUTLINE = ('o' << 24 | 'u' << 16 | 't' << 8 | 'l'),
            FT_GLYPH_FORMAT_PLOTTER = ('p' << 24 | 'l' << 16 | 'o' << 8 | 't'),
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Span
        {
            short x;
            ushort len;
            byte coverage;
        }

        public delegate void FT_SpanFunc(int y, int count, FT_Span[] spans, IntPtr user);

        public const int FT_RASTER_FLAG_DEFAULT = 0x0;
        public const int FT_RASTER_FLAG_AA = 0x1;
        public const int FT_RASTER_FLAG_DIRECT = 0x2;
        public const int FT_RASTER_FLAG_CLIP = 0x4;

        [StructLayout(LayoutKind.Sequential)]
        public struct FT_Raster_Params
        {
            IntPtr target;
            IntPtr source;
            int flags;
            IntPtr gray_spans;
            IntPtr black_spans; // black_spans
            IntPtr bit_test; // bit_test
            IntPtr bit_set; // bit_set
            IntPtr user;
            FT_BBox clip_box;
        }
        #endregion
    }
}
