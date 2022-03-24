using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_Vector
    {
        public IntPtr x;
        public IntPtr y;
    }
}