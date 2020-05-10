using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeReference<T> where T : NativeObject
    {
        private readonly IntPtr _memPtr;

        public NativeReference(IntPtr memPtr)
            => _memPtr = memPtr;

        public static implicit operator NativeReference<T>(T memory)
            => new NativeReference<T>(memory.Reference);
    }
}