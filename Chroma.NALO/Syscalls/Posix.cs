using System;
using System.Runtime.InteropServices;

namespace Chroma.NALO.Syscalls
{
    internal static class Posix
    {
        private static bool _isLibc2_35plus;
        
        internal const int RTLD_LAZY = 0x00001;
        internal const int RTLD_NOW = 0x0002;
        internal const int RTLD_GLOBAL = 0x0100;

        internal static void DetectLibcEnvironment()
        {
            try
            {
                libdl_dlopen("not_important", RTLD_LAZY);
                _isLibc2_35plus = false;
            }
            catch (DllNotFoundException)
            {
                _isLibc2_35plus = true;
            }
        }

        internal static IntPtr dlopen(string fileName, int flags)
        {
            if (_isLibc2_35plus)
            {
                return libc_dlopen(fileName, flags);
            }
            else
            {
                return libdl_dlopen(fileName, flags);
            }
        }

        internal static IntPtr dlerror()
        {
            if (_isLibc2_35plus)
            {
                return libc_dlerror();
            }
            else
            {
                return libdl_dlerror();
            }
        }

        internal static IntPtr dlsym(IntPtr handle, string symbol)
        {
            if (_isLibc2_35plus)
            {
                return libc_dlsym(handle, symbol);
            }
            else
            {
                return libdl_dlsym(handle, symbol);
            }
        }
        
        [DllImport("libdl", EntryPoint = "dlopen")]
        private static extern IntPtr libdl_dlopen(string fileName, int flags);

        [DllImport("libdl", EntryPoint = "dlerror")]
        private static extern IntPtr libdl_dlerror();

        [DllImport("libdl", EntryPoint = "dlsym")]
        private static extern IntPtr libdl_dlsym(IntPtr handle, string symbol);
        
        [DllImport("libc", EntryPoint = "dlopen")]
        private static extern IntPtr libc_dlopen(string fileName, int flags);

        [DllImport("libc", EntryPoint = "dlerror")]
        private static extern IntPtr libc_dlerror();

        [DllImport("libc", EntryPoint = "dlsym")]
        private static extern IntPtr libc_dlsym(IntPtr handle, string symbol);
    }
}