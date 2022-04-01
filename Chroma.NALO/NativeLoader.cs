using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Chroma.NALO.PlatformSpecific;
using Chroma.NALO.Syscalls;

namespace Chroma.NALO
{
    public static class NativeLoader
    {
        internal static EarlyLog EarlyLog = new("crboot.nalo.log");
        internal static IPlatform Platform { get; private set; }

        public static void LoadNatives(bool skipChecksumVerification)
        {
            try
            {
                var assembly = Assembly.GetCallingAssembly();

                var libraryFileNames = NativeLibraryExtractor.ExtractAll(assembly, skipChecksumVerification)
                    .Select(Path.GetFileName);

                Posix.DetectLibcEnvironment();

                if (Platform == null)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        Platform = new WindowsPlatform();
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        Platform = new LinuxPlatform();
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        Platform = new MacPlatform();
                    }
                    else throw new PlatformNotSupportedException("This platform is not supported.");
                }

                foreach (var libraryFileName in libraryFileNames)
                {
                    EarlyLog.Info($"Now loading: {libraryFileName}");
                    Platform.Register(libraryFileName);
                }
            }
            catch (Exception e)
            {
                throw new NativeLoaderException("NALO has failed. This is an engine error. Contact the developer.\n", e);
            }
        }
    }
}