using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_Matrix
    {
        public IntPtr xx, xy;
        public IntPtr yx, yy;
    }
}
