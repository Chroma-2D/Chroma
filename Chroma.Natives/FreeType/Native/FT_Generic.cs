using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
#pragma warning disable 1591
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal unsafe delegate void FT_Generic_Finalizer(IntPtr @object);
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_Generic
    {
        public IntPtr data;
        public IntPtr finalizer;
    }
#pragma warning restore 1591
}
