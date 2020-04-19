using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL.Interop
{
    public struct AnsiString
    {
        public IntPtr Pointer;

        public string Value => Marshal.PtrToStringAnsi(Pointer);

        public override string ToString()
            => Value;
    }
}
