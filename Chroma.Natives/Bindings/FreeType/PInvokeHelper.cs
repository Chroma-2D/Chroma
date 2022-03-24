using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.FreeType
{
    internal static class PInvokeHelper
    {
        public static unsafe void Copy(IntPtr source, int sourceOffset, IntPtr destination, int destinationOffset,
            int count)
        {
            byte* src = (byte*)source + sourceOffset;
            byte* dst = (byte*)destination + destinationOffset;
            byte* end = dst + count;

            while (dst != end)
                *dst++ = *src++;
        }

        public static IntPtr AbsoluteOffsetOf<T>(IntPtr start, string fieldName)
            => new(start.ToInt64() + Marshal.OffsetOf(typeof(T), fieldName).ToInt64());

        public static T PtrToStructure<T>(IntPtr reference)
            => (T)Marshal.PtrToStructure(reference, typeof(T));
    }
}