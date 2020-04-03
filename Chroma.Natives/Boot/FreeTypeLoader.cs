using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

namespace Chroma.Natives.Boot
{
    public static class FreeTypeLoader
    {
        const int RTLD_NOW = 2;

        public static string NativeLibraryPath { get; private set; }
        public delegate IntPtr SymbolLookupDelegate(IntPtr addr, string name);

        private static IntPtr _freetypeAddr;
        private static SymbolLookupDelegate _symbolLookup;

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

        [DllImport("libdl")]
        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl")]
        private static extern IntPtr dlerror();

        [DllImport("libdl")]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        static FreeTypeLoader()
        {

            // Figure out which OS we're on. Windows or "other".
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                        Environment.OSVersion.Platform == PlatformID.MacOSX ||
                        // Legacy mono value. See https://www.mono-project.com/docs/faq/technical/
                        (int)Environment.OSVersion.Platform == 128)
            {
                _freetypeAddr = LoadPosixLibrary(out _symbolLookup);
            }
            else
            {
                _freetypeAddr = LoadWindowsLibrary(out _symbolLookup);
            }
        }

        public static T LoadFunction<T>(string function, bool throwIfNotFound = false)
        {
            var ret = _symbolLookup(_freetypeAddr, function);

            if (ret == IntPtr.Zero)
            {
                if (throwIfNotFound)
                    throw new EntryPointNotFoundException(function);

                return default;
            }

            return Marshal.GetDelegateForFunctionPointer<T>(ret);
        }

        private static IntPtr LoadWindowsLibrary(out SymbolLookupDelegate symbolLookup)
        {
            string libFile = "freetype.dll";
            string arch = EmbeddedDllLoader.ArchitectureString;

            var paths = new[]
            {
                // This is where native libraries in our nupkg should end up
                Path.Combine(EmbeddedDllLoader.DllDirectoryPath, libFile),
            };

            foreach (var path in paths)
            {
                if (path == null) continue;
                if (!File.Exists(path)) continue;

                var addr = LoadLibrary(path);
                if (addr == IntPtr.Zero)
                    throw new Exception("LoadLibrary failed: " + path);

                symbolLookup = GetProcAddress;
                NativeLibraryPath = path;
                return addr;
            }

            throw new Exception("LoadLibrary failed: unable to locate library " + libFile + ". Searched: " + paths.Aggregate((a, b) => a + "; " + b));
        }

        private static IntPtr LoadPosixLibrary(out SymbolLookupDelegate symbolLookup)
        {
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Environment.OSVersion.Platform returns "Unix" for Unix or OSX, so use RuntimeInformation here
            var isOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            string libFile = isOsx ? "libfreetype.dylib" : "libfreetype.so";
            string arch = isOsx ? "osx" : "linux-" + (Environment.Is64BitProcess ? "x64" : "x86");

            // Search a few different locations for our native assembly
            var paths = new[]
            {
                // This is where native libraries in our nupkg should end up
                Path.Combine(rootDirectory, "runtimes", arch, "native", libFile),
                // The build output folder
                Path.Combine(rootDirectory, libFile),
                Path.Combine("/usr/local/lib", libFile),
                Path.Combine("/usr/lib", libFile)
            };

            foreach (var path in paths)
            {
                if (path == null) continue;
                if (!File.Exists(path)) continue;

                var addr = dlopen(path, RTLD_NOW);
                if (addr == IntPtr.Zero)
                {
                    // Not using NanosmgException because it depends on nn_errno.
                    var error = Marshal.PtrToStringAnsi(dlerror());
                    throw new Exception("dlopen failed: " + path + " : " + error);
                }

                symbolLookup = dlsym;
                NativeLibraryPath = path;
                return addr;
            }

            throw new Exception("dlopen failed: unable to locate library " + libFile + ". Searched: " + paths.Aggregate((a, b) => a + "; " + b));
        }

    }
}
