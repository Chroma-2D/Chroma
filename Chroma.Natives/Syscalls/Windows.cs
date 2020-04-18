using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Syscalls
{
    internal static class Windows
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern bool SetDllDirectory(string path);

        [DllImport("kernel32")]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

        [DllImport("kernel32")]
        internal static extern IntPtr GetLastError();
    }
}
