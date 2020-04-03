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

        internal static readonly string ArchitectureString = Environment.Is64BitOperatingSystem ? "Win64" : "Win32";
        internal static string DllDirectoryPath { get; private set; }

        internal void InitializeNativeDlls()
        {
            DllDirectoryPath = CreateDllDirectory();
            SetDllDirectory(DllDirectoryPath);

            var dependencies = EmbeddedResources.GetResourceNames()
                                                .Where(x => x.Contains(ArchitectureString) && x.EndsWith(".dll"));
            foreach (var dep in dependencies)
            {
                ExtractAndLoadEmbeddedDependency(DllDirectoryPath, dep);
            }
        }

        private static string ResourceNameToFileName(string resourceName)
            => resourceName.Replace($"Chroma.Natives.Binaries.{ArchitectureString}.", "");

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

            Console.WriteLine($"    {actualFileName}");
            LoadLibrary(actualFileName);
        }
    }
}
