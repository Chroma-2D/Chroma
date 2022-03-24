using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_MemoryRec
    {
        public IntPtr user;
        public FT_Alloc_Func alloc;
        public FT_Free_Func free;
        public FT_Realloc_Func realloc;
    }
}