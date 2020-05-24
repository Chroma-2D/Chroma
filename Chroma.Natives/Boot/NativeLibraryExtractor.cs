using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Chroma.Natives.Boot.Config;
using Chroma.Natives.Compression;

namespace Chroma.Natives.Boot
{
    internal static class NativeLibraryExtractor
    {
        internal static string LibraryRoot { get; private set; }

        internal static List<string> ExtractAll()
        {
            LibraryRoot = CreateLibraryDirectory();

            var resourceNames = EmbeddedResources.GetResourceNames();
            var fileNames = resourceNames.Where(x => x.Contains(EmbeddedResources.PlatformString));

            return ExtractLibraries(fileNames, LibraryRoot);
        }

        private static string CreateLibraryDirectory()
        {
            string libraryRoot;

            if (ModuleInitializer.BootConfig.NativesInApplicationDirectory)
            {
                var appDirPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                libraryRoot = Path.Combine(appDirPath, "Natives");
            }
            else
            {
                var configDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                libraryRoot = Path.Combine(configDirPath, "ChromaNatives");
            }

            try
            {
                if (!Directory.Exists(libraryRoot))
                    Directory.CreateDirectory(libraryRoot);
            }
            catch (Exception e)
            {
                throw new NativeExtractorException("Failed to extract natives to the requested directory.", e);
            }

            return libraryRoot;
        }

        private static List<string> ExtractLibraries(IEnumerable<string> resourceNames, string targetDir)
        {
            var filePaths = new List<string>();

            foreach (var resourceName in resourceNames)
            {
                var embeddedPackageStream = EmbeddedResources.GetResourceStream(resourceName);
                var bzipStream = new BZip2InputStream(embeddedPackageStream);
                
                var embeddedLibraryBytes = new byte[1024 * 1024 * 8];
                var memoryStream = new MemoryStream(embeddedLibraryBytes);

                var fileName = Path.GetFileNameWithoutExtension(
                    EmbeddedResources.ResourceNameToFileName(resourceName)
                ) + EmbeddedResources.GetNativeExtensionForCurrentPlatform();

                var libraryPath = Path.Combine(targetDir, fileName);
                filePaths.Add(libraryPath);

                if (File.Exists(libraryPath))
                {
                    if (ModuleInitializer.BootConfig.SkipChecksumVerification)
                    {
                        continue;
                    }
                    else
                    {
                        var existingBytes = File.ReadAllBytes(libraryPath);
                        bzipStream.CopyTo(memoryStream, 1024);

                        if (NativeIntegrity.ChecksumsMatch(existingBytes, embeddedLibraryBytes))
                            continue;
                        
                        File.Delete(libraryPath);
                    }
                }
                else
                {
                    bzipStream.CopyTo(memoryStream, 1024);
                }
                
                File.WriteAllBytes(libraryPath, embeddedLibraryBytes);
                Console.WriteLine($"Extracting: {libraryPath}");
                
                bzipStream.Close();
                memoryStream.Close();
            }

            return filePaths;
        }
    }
}
