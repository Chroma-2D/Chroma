using System.Collections.Generic;
using System.IO;

namespace Chroma.Natives.Boot.PlatformSpecific
{
    internal class LinuxPlatform : IPlatform
    {
        public NativeLibraryRegistry Registry { get; }

        public LinuxPlatform()
        {
            var paths = new List<string>
            {
                NativeLibraryExtractor.LibraryRoot,
                "/usr/lib",
                "/usr/local/lib"
            };
            
            Registry = new NativeLibraryRegistry(paths);
        }

        public void Register(string libFilePath)
        {
            var fileName = Path.GetFileName(libFilePath);
            var namesToTry = new[]
            {
                fileName,
                $"{fileName}.so",
                $"lib{fileName}",
                $"lib{fileName}.so"
            };

            Registry.TryRegister(namesToTry);
        }
    }
}
