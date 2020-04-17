using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Boot
{
    internal class EmbeddedDllLoader
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool SetDllDirectory(string path);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool LoadLibrary(string path);

        internal static string PlatformString = Environment.OSVersion.Platform switch
        {
            PlatformID.MacOSX => "osx_64",
            PlatformID.Unix => "linux_64",
            PlatformID.Win32NT => "windows_64",
            PlatformID.Win32S => throw new PlatformNotSupportedException("Stop. Get help. Find someone else who uses a sane operating system."),
            PlatformID.Win32Windows => throw new PlatformNotSupportedException("Seriously, stop using Win9x. Chroma doesn't run on this thing."),
            PlatformID.WinCE => throw new PlatformNotSupportedException("Chroma doesn't run on Windows CE, although I do appreciate your attempt."),
            PlatformID.Xbox => throw new PlatformNotSupportedException("Xbox platform is not supported by Chroma (yet?)."),
            _ => throw new PlatformNotSupportedException("The heck are you using? Whatever it is, it's not supported by Chroma.")
        };

        internal static string DllDirectoryPath { get; private set; }

        internal void InitializeNativeDlls()
        {
            DllDirectoryPath = CreateDllDirectory();
            SetDllDirectory(DllDirectoryPath);

            var resourceNames = EmbeddedResources.GetResourceNames();
            var dependencies = resourceNames.Where(x => x.Contains(PlatformString) && x.EndsWith(".dll"));
            foreach (var dep in dependencies)
                ExtractAndLoadEmbeddedDependency(DllDirectoryPath, dep);
        }

        private static string ResourceNameToFileName(string resourceName)
            => resourceName.Replace($"Chroma.Natives.Binaries.{PlatformString}.", "");

        private static string CreateDllDirectory()
        {
            var path = Path.GetTempPath();
            var completePath = Path.Combine(path, "Chroma");

            if (Directory.Exists(completePath))
                Directory.Delete(completePath, true);

            Directory.CreateDirectory(completePath);
            return completePath;
        }

        private static void ExtractAndLoadEmbeddedDependency(string targetDir, string fqn)
        {
            var actualFileName = ResourceNameToFileName(fqn);

            using var fs = new FileStream(Path.Combine(targetDir, actualFileName), FileMode.Create);
            using var ms = EmbeddedResources.GetResourceStream(fqn);
            ms.CopyTo(fs);

            if (actualFileName == "freetype.dll")
                return;

            Console.WriteLine($"    {actualFileName}");
            LoadLibrary(actualFileName);
        }
    }
}
