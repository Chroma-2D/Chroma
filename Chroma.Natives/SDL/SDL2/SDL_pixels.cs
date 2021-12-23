using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL
{
    internal static partial class SDL2
    {
		public static uint SDL_DEFINE_PIXELFOURCC(byte A, byte B, byte C, byte D)
		{
			return SDL_FOURCC(A, B, C, D);
		}

		public static uint SDL_DEFINE_PIXELFORMAT(
			SDL_PixelType type,
			uint order,
			SDL_PackedLayout layout,
			byte bits,
			byte bytes
		) {
			return (uint) (
				(1 << 28) |
				(((byte) type) << 24) |
				(((byte) order) << 20) |
				(((byte) layout) << 16) |
				(bits << 8) |
				(bytes)
			);
		}

		public enum SDL_PixelType
		{
			SDL_PIXELTYPE_UNKNOWN,
			SDL_PIXELTYPE_INDEX1,
			SDL_PIXELTYPE_INDEX4,
			SDL_PIXELTYPE_INDEX8,
			SDL_PIXELTYPE_PACKED8,
			SDL_PIXELTYPE_PACKED16,
			SDL_PIXELTYPE_PACKED32,
			SDL_PIXELTYPE_ARRAYU8,
			SDL_PIXELTYPE_ARRAYU16,
			SDL_PIXELTYPE_ARRAYU32,
			SDL_PIXELTYPE_ARRAYF16,
			SDL_PIXELTYPE_ARRAYF32
		}

		public enum SDL_BitmapOrder
		{
			SDL_BITMAPORDER_NONE,
			SDL_BITMAPORDER_4321,
			SDL_BITMAPORDER_1234
		}

		public enum SDL_PackedOrder
		{
			SDL_PACKEDORDER_NONE,
			SDL_PACKEDORDER_XRGB,
			SDL_PACKEDORDER_RGBX,
			SDL_PACKEDORDER_ARGB,
			SDL_PACKEDORDER_RGBA,
			SDL_PACKEDORDER_XBGR,
			SDL_PACKEDORDER_BGRX,
			SDL_PACKEDORDER_ABGR,
			SDL_PACKEDORDER_BGRA
		}

		public enum SDL_ArrayOrder
		{
			SDL_ARRAYORDER_NONE,
			SDL_ARRAYORDER_RGB,
			SDL_ARRAYORDER_RGBA,
			SDL_ARRAYORDER_ARGB,
			SDL_ARRAYORDER_BGR,
			SDL_ARRAYORDER_BGRA,
			SDL_ARRAYORDER_ABGR
		}

		public enum SDL_PackedLayout
		{
			SDL_PACKEDLAYOUT_NONE,
			SDL_PACKEDLAYOUT_332,
			SDL_PACKEDLAYOUT_4444,
			SDL_PACKEDLAYOUT_1555,
			SDL_PACKEDLAYOUT_5551,
			SDL_PACKEDLAYOUT_565,
			SDL_PACKEDLAYOUT_8888,
			SDL_PACKEDLAYOUT_2101010,
			SDL_PACKEDLAYOUT_1010102
		}

		public static readonly uint SDL_PIXELFORMAT_UNKNOWN = 0;
		public static readonly uint SDL_PIXELFORMAT_INDEX1LSB =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_INDEX1,
				(uint) SDL_BitmapOrder.SDL_BITMAPORDER_4321,
				0,
				1, 0
			);
		public static readonly uint SDL_PIXELFORMAT_INDEX1MSB =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_INDEX1,
				(uint) SDL_BitmapOrder.SDL_BITMAPORDER_1234,
				0,
				1, 0
			);
		public static readonly uint SDL_PIXELFORMAT_INDEX4LSB =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_INDEX4,
				(uint) SDL_BitmapOrder.SDL_BITMAPORDER_4321,
				0,
				4, 0
			);
		public static readonly uint SDL_PIXELFORMAT_INDEX4MSB =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_INDEX4,
				(uint) SDL_BitmapOrder.SDL_BITMAPORDER_1234,
				0,
				4, 0
			);
		public static readonly uint SDL_PIXELFORMAT_INDEX8 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_INDEX8,
				0,
				0,
				8, 1
			);
		public static readonly uint SDL_PIXELFORMAT_RGB332 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED8,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_332,
				8, 1
			);
		public static readonly uint SDL_PIXELFORMAT_XRGB444 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
				12, 2
			);
		public static readonly uint SDL_PIXELFORMAT_RGB444 =
			SDL_PIXELFORMAT_XRGB444;
		public static readonly uint SDL_PIXELFORMAT_XBGR444 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XBGR,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
				12, 2
			);
		public static readonly uint SDL_PIXELFORMAT_BGR444 =
			SDL_PIXELFORMAT_XBGR444;
		public static readonly uint SDL_PIXELFORMAT_XRGB1555 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
				15, 2
			);
		public static readonly uint SDL_PIXELFORMAT_RGB555 =
			SDL_PIXELFORMAT_XRGB1555;
		public static readonly uint SDL_PIXELFORMAT_XBGR1555 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_INDEX1,
				(uint) SDL_BitmapOrder.SDL_BITMAPORDER_4321,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
				15, 2
			);
		public static readonly uint SDL_PIXELFORMAT_BGR555 =
			SDL_PIXELFORMAT_XBGR1555;
		public static readonly uint SDL_PIXELFORMAT_ARGB4444 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_RGBA4444 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_RGBA,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_ABGR4444 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_ABGR,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_BGRA4444 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_BGRA,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_4444,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_ARGB1555 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_RGBA5551 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_RGBA,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_5551,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_ABGR1555 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_ABGR,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_1555,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_BGRA5551 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_BGRA,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_5551,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_RGB565 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_565,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_BGR565 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED16,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XBGR,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_565,
				16, 2
			);
		public static readonly uint SDL_PIXELFORMAT_RGB24 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_ARRAYU8,
				(uint) SDL_ArrayOrder.SDL_ARRAYORDER_RGB,
				0,
				24, 3
			);
		public static readonly uint SDL_PIXELFORMAT_BGR24 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_ARRAYU8,
				(uint) SDL_ArrayOrder.SDL_ARRAYORDER_BGR,
				0,
				24, 3
			);
		public static readonly uint SDL_PIXELFORMAT_XRGB888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XRGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				24, 4
			);
		public static readonly uint SDL_PIXELFORMAT_RGB888 =
			SDL_PIXELFORMAT_XRGB888;
		public static readonly uint SDL_PIXELFORMAT_RGBX8888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_RGBX,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				24, 4
			);
		public static readonly uint SDL_PIXELFORMAT_XBGR888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_XBGR,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				24, 4
			);
		public static readonly uint SDL_PIXELFORMAT_BGR888 =
			SDL_PIXELFORMAT_XBGR888;
		public static readonly uint SDL_PIXELFORMAT_BGRX8888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_BGRX,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				24, 4
			);
		public static readonly uint SDL_PIXELFORMAT_ARGB8888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				32, 4
			);
		public static readonly uint SDL_PIXELFORMAT_RGBA8888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_RGBA,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				32, 4
			);
		public static readonly uint SDL_PIXELFORMAT_ABGR8888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_ABGR,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				32, 4
			);
		public static readonly uint SDL_PIXELFORMAT_BGRA8888 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_BGRA,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_8888,
				32, 4
			);
		public static readonly uint SDL_PIXELFORMAT_ARGB2101010 =
			SDL_DEFINE_PIXELFORMAT(
				SDL_PixelType.SDL_PIXELTYPE_PACKED32,
				(uint) SDL_PackedOrder.SDL_PACKEDORDER_ARGB,
				SDL_PackedLayout.SDL_PACKEDLAYOUT_2101010,
				32, 4
			);
		public static readonly uint SDL_PIXELFORMAT_YV12 =
			SDL_DEFINE_PIXELFOURCC(
				(byte) 'Y', (byte) 'V', (byte) '1', (byte) '2'
			);
		public static readonly uint SDL_PIXELFORMAT_IYUV =
			SDL_DEFINE_PIXELFOURCC(
				(byte) 'I', (byte) 'Y', (byte) 'U', (byte) 'V'
			);
		public static readonly uint SDL_PIXELFORMAT_YUY2 =
			SDL_DEFINE_PIXELFOURCC(
				(byte) 'Y', (byte) 'U', (byte) 'Y', (byte) '2'
			);
		public static readonly uint SDL_PIXELFORMAT_UYVY =
			SDL_DEFINE_PIXELFOURCC(
				(byte) 'U', (byte) 'Y', (byte) 'V', (byte) 'Y'
			);
		public static readonly uint SDL_PIXELFORMAT_YVYU =
			SDL_DEFINE_PIXELFOURCC(
				(byte) 'Y', (byte) 'V', (byte) 'Y', (byte) 'U'
			);

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Color
		{
			public byte r;
			public byte g;
			public byte b;
			public byte a;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_Palette
		{
			public int ncolors;
			public IntPtr colors;
			public int version;
			public int refcount;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_PixelFormat
		{
			public uint format;
			public IntPtr palette; // SDL_Palette*
			public byte BitsPerPixel;
			public byte BytesPerPixel;
			public uint Rmask;
			public uint Gmask;
			public uint Bmask;
			public uint Amask;
			public byte Rloss;
			public byte Gloss;
			public byte Bloss;
			public byte Aloss;
			public byte Rshift;
			public byte Gshift;
			public byte Bshift;
			public byte Ashift;
			public int refcount;
			public IntPtr next; // SDL_PixelFormat*
		}

		/* IntPtr refers to an SDL_PixelFormat* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_AllocFormat(uint pixel_format);

		/* format refers to an SDL_PixelFormat* */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_FreeFormat(IntPtr format);
    }
}