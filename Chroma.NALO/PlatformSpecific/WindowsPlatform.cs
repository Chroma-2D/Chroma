using System.Collections.Generic;
using System.IO;

namespace Chroma.NALO.PlatformSpecific
{
    internal class WindowsPlatform : IPlatform
    {
        public NativeLibraryRegistry Registry { get; }

        public WindowsPlatform()
        {
            var paths = new List<string> { NativeLibraryExtractor.LibraryRoot };
            Registry = new NativeLibraryRegistry(paths);
        }

        public void Register(string libFilePath)
            => Registry.Register(Path.GetFileName(libFilePath));
    }
}
