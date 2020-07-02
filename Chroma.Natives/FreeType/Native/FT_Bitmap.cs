using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_Bitmap
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
}