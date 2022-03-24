using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    internal delegate void FT_Generic_Finalizer(IntPtr @object);

    [StructLayout(LayoutKind.Sequential)]
    internal struct FT_Generic
    {
        public IntPtr data;
        public IntPtr finalizer;
    }
}