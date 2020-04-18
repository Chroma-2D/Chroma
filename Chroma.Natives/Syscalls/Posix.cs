using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Syscalls
{
    internal static class Posix
    {
        internal const int RTLD_NOW = 2;

        [DllImport("libdl")]
        internal static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl")]
        internal static extern IntPtr dlerror();

        [DllImport("libdl")]
        internal static extern IntPtr dlsym(IntPtr handle, string symbol);
    }
}
