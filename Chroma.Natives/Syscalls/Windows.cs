using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Syscalls
{
    internal static class Windows
    {
        internal const int STD_OUTPUT_HANDLE = -11;
        
        internal const int ENABLE_PROCESSED_OUTPUT = 0x0001;
        internal const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        
        [DllImport("kernel32")]
        internal static extern int GetLastError();
        
        [DllImport("kernel32")]
        internal static extern IntPtr GetStdHandle(int nStdHandle);
        
        [DllImport("kernel32")]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);
        
        [DllImport("kernel32")]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);
    }
}