using System.Collections.Generic;
using System.IO;

namespace Chroma.Natives.Boot.PlatformSpecific
{
    internal class LinuxPlatform : IPlatform
    {
        public NativeLibraryRegistry Registry { get; }

        public LinuxPlatform()
        {
            // our root has to be at the end.
            // on linux. why? fuck if i know,
            // but it works that way. probably one
            // of the natives fails to load, and
            // the cascade results in renderer failing
            // to initialize, but that's just my humble theory

            var paths = new List<string>
            {
                "/lib",
                "/usr/lib",
                "/usr/local/lib",
                NativeLibraryExtractor.LibraryRoot
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