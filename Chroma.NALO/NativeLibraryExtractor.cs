using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Chroma.NALO.Compression;

namespace Chroma.NALO
{
    internal static class NativeLibraryExtractor
    {
        internal static string LibraryRoot { get; private set; }

        internal static List<string> ExtractAll(Assembly assembly, bool skipChecksumVerification)
        {
            LibraryRoot = CreateLibraryDirectory(true);

            var resourceNames = EmbeddedResources.GetResourceNames(assembly);
            var fileNames = resourceNames.Where(x => x.Contains(EmbeddedResources.PlatformString));

            return ExtractLibraries(assembly, fileNames, LibraryRoot, skipChecksumVerification);
        }

        private static string CreateLibraryDirectory(bool extractToApplicationDirectory)
        {
            string libraryRoot;

            if (extractToApplicationDirectory)
            {
                var appDirPath = AppContext.BaseDirectory;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    libraryRoot = appDirPath;
                }
                else
                {
                    libraryRoot = Path.Combine(appDirPath!, "Natives");
                }
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

        private static List<string> ExtractLibraries(
            Assembly assembly, 
            IEnumerable<string> resourceNames, 
            string targetDir,
            bool skipChecksumVerification)
        {
            var filePaths = new List<string>();

            foreach (var resourceName in resourceNames)
            {
                var fileName = Path.GetFileNameWithoutExtension(
                    EmbeddedResources.ResourceNameToFileName(assembly, resourceName)
                ) + EmbeddedResources.PlatformSpecificNativeExtension;

                var libraryPath = Path.Combine(targetDir, fileName);
                filePaths.Add(libraryPath);

                using var embeddedPackageStream = EmbeddedResources.GetResourceStream(assembly, resourceName);
                using var bzipStream = new BZip2InputStream(embeddedPackageStream);

                if (File.Exists(libraryPath))
                {
                    if (skipChecksumVerification)
                        continue;

                    using var memoryStream = new MemoryStream();
                    bzipStream.CopyTo(memoryStream, 1024);

                    var memoryBuffer = memoryStream.ToArray();
                    var embeddedLibraryBytes = memoryBuffer[0..(int)memoryStream.Position];

                    var existingBytes = File.ReadAllBytes(libraryPath);
                    if (NativeIntegrity.ChecksumsMatch(existingBytes, embeddedLibraryBytes))
                        continue;
                    
                    File.Delete(libraryPath);
                    File.WriteAllBytes(libraryPath, embeddedLibraryBytes);
                }
                else
                {
                    using (var fs = new FileStream(libraryPath, FileMode.CreateNew, FileAccess.Write))
                        bzipStream.CopyTo(fs);
                }

                EarlyLog.Info($"Extracting: {libraryPath}");
                bzipStream.Close();
            }

            return filePaths;
        }
    }
}
