namespace Chroma.NALO.PlatformSpecific;

using System.Collections.Generic;
using System.IO;

internal class MacPlatform : IPlatform
{
    public NativeLibraryRegistry Registry { get; }

    public MacPlatform()
    {
        Directory.SetCurrentDirectory(NativeLibraryExtractor.LibraryRoot);
        var paths = new List<string>
        {
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
            $"{fileName}.dylib",
            $"lib{fileName}",
            $"lib{fileName}.dylib"
        };

        Registry.TryRegister(namesToTry);
    }
}