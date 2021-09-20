using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Chroma.Natives.Syscalls;

namespace Chroma.Natives.Boot
{
    internal class NativeLibraryRegistry
    {
        private readonly List<string> _lookupPaths;
        private readonly Dictionary<string, NativeLibrary> _libRegistry;

        public NativeLibraryRegistry(IEnumerable<string> lookupPaths)
        {
            _lookupPaths = new List<string>(lookupPaths);
            _libRegistry = new Dictionary<string, NativeLibrary>();
        }

        public NativeLibrary Register(string fileName)
        {
            var lookupStack = new Stack<string>(_lookupPaths);

            while (lookupStack.Count != 0)
            {
                var lookupDirectory = lookupStack.Pop();
                var libPath = Path.Combine(lookupDirectory, fileName);

                if (!File.Exists(libPath))
                    continue;

                var handle = RegisterPlatformSpecific(libPath, out var symbolLookup);
                var nativeInfo = new NativeLibrary(libPath, handle, symbolLookup);

                _libRegistry.Add(fileName, nativeInfo);
                return nativeInfo;
            }

            throw new NativeLoaderException($"Failed to find '{fileName}' at the provided lookup paths!");
        }

        public NativeLibrary TryRegister(params string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                try
                {
                    return Register(fileName);
                }
                catch (NativeLoaderException)
                {
                    /* Skip to next... */
                }
            }

            throw new NativeLoaderException(
                "Failed to find any provided file name variant at the provided lookup paths!");
        }

        public NativeLibrary Retrieve(string fileName)
        {
            if (!_libRegistry.ContainsKey(fileName))
                throw new NativeLoaderException($"Library file '{fileName}' was never registered.");

            return _libRegistry[fileName];
        }

        public NativeLibrary TryRetrieve(bool tryRegisterIfNotFound = true, params string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                try
                {
                    return Retrieve(fileName);
                }
                catch (NativeLoaderException)
                {
                    if (tryRegisterIfNotFound)
                    {
                        try
                        {
                            return Register(fileName);
                        }
                        catch (NativeLoaderException)
                        {
                            /* Skip to next... */
                        }
                    }
                    /* Skip to next... */
                }
            }
            
            throw new NativeLoaderException(
                $"None of the provided file names were found. Tried: {string.Join(",", fileNames)}"
            );
        }

        private IntPtr RegisterPlatformSpecific(string absoluteFilePath,
            out NativeLibrary.SymbolLookupDelegate symbolLookup)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var handle = Posix.dlopen(absoluteFilePath, Posix.RTLD_LAZY | Posix.RTLD_GLOBAL);

                if (handle == IntPtr.Zero)
                    throw new NativeLoaderException(
                        $"Failed to load '{absoluteFilePath}'. dlerror: {Marshal.PtrToStringAnsi(Posix.dlerror())}");

                symbolLookup = Posix.dlsym;
                return handle;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var handle = Posix.dlopen(absoluteFilePath, Posix.RTLD_LAZY | Posix.RTLD_GLOBAL);

                if (handle == IntPtr.Zero)
                    throw new NativeLoaderException(
                        $"Failed to load '{absoluteFilePath}'. dlerror: {Marshal.PtrToStringAnsi(Posix.dlerror())}");

                symbolLookup = Posix.dlsym;
                return handle;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var dllDirectory = Path.GetDirectoryName(absoluteFilePath);
                var fileName = Path.GetFileName(absoluteFilePath);

                Windows.SetDllDirectory(dllDirectory);
                var handle = Windows.LoadLibrary(fileName);

                if (handle == IntPtr.Zero)
                {
                    var error = Windows.GetLastError();

                    if (error == 0x000000C1 || error == 0x0000007E)
                    {
                        throw new NativeLoaderException(
                            $"Failed to load '{absoluteFilePath}'. " +
                            $"Make sure your system has VC/VC++ runtime redistributables installed."
                        );
                    }
                    else
                    {
                        throw new NativeLoaderException(
                            $"Failed to load '{absoluteFilePath}' - " +
                            $"LoadLibrary returned HRESULT 0x{error:X8}. " +
                            $"If you feel adventurous you can look it up using your favorite search engine."
                        );
                    }
                }

                symbolLookup = Windows.GetProcAddress;
                return handle;
            }

            throw new NativeLoaderException($"Platform '{Environment.OSVersion.Platform}' is not supported.");
        }
    }
}