using System;
using System.Runtime.InteropServices;

namespace Chroma.SDL2
{
    public struct CString
    {
        public IntPtr Pointer;

        public string Value => Marshal.PtrToStringAnsi(Pointer);

        public override string ToString()
            => Value;
    }
}
