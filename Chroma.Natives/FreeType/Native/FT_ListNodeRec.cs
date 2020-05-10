using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_ListNodeRec
    {
        public IntPtr prev;
        public IntPtr next;
        public IntPtr data;
    }
}
