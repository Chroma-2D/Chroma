using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Chroma.Natives.SDL
{
    internal static partial class SDL2
    {
        private const string LibraryName = "SDL2";

        internal static int Utf8Size(string str)
        {
            if (str == null)
            {
                return 0;
            }
            return (str.Length * 4) + 1;
        }

        internal static unsafe byte* Utf8Encode(string str, byte* buffer, int bufferSize)
        {
            if (str == null)
            {
                return (byte*)0;
            }
            fixed (char* strPtr = str)
            {
                Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
            }
            return buffer;
        }

        internal static unsafe byte* Utf8EncodeHeap(string str)
        {
            if (str == null)
            {
                return (byte*)0;
            }

            int bufferSize = Utf8Size(str);
            byte* buffer = (byte*)Marshal.AllocHGlobal(bufferSize);
            fixed (char* strPtr = str)
            {
                Encoding.UTF8.GetBytes(strPtr, str.Length + 1, buffer, bufferSize);
            }
            return buffer;
        }

        public static unsafe string UTF8_ToManaged(IntPtr s, bool freePtr = false)
        {
            if (s == IntPtr.Zero)
            {
                return null;
            }

            byte* ptr = (byte*)s;
            while (*ptr != 0)
            {
                ptr++;
            }

            var result = Encoding.UTF8.GetString(
                (byte*)s,
                (int)(ptr - (byte*)s)
            );

            if (freePtr)
            {
                SDL_free(s);
            }
            return result;
        }
    }
}