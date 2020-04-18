using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Chroma.Natives.Boot.PlatformSpecific;
using Chroma.Natives.SDL;

namespace Chroma.Natives.Boot
{
    internal static class ModuleInitializer
    {
        internal static IPlatform Platform { get; private set; }

        public static void Initialize()
        {
            if (!Environment.Is64BitOperatingSystem)
                throw new PlatformNotSupportedException("Chroma supports 64-bit systems only.");

            var libraryFileNames = NativeLibraryExtractor.ExtractAll().Select(
                x => Path.GetFileName(x)
            );

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Platform = new WindowsPlatform();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Platform = new LinuxPlatform();
            }
            else //if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new PlatformNotSupportedException("Your current platform is not supported by Chroma Natives just yet.");
            }

            foreach (var libraryFileName in libraryFileNames)
            {
                Console.WriteLine($"Now loading: {libraryFileName}");
                Platform.Register(libraryFileName);
            }

            Console.WriteLine("---");
            Console.WriteLine("Initializing SDL2 core...");
            SDL2.SDL_Init(SDL2.SDL_INIT_EVERYTHING);
        }
    }
}
