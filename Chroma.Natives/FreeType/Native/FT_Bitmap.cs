using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
#pragma warning disable 1591
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Bitmap
    {
        public uint rows;
        public uint width;
        public int pitch;
        public IntPtr buffer;
        public ushort num_grays;
        public byte pixel_mode;
        public byte palette_mode;
        public IntPtr palette;
    }
#pragma warning restore 1591
}
