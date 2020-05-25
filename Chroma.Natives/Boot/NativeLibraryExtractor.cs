﻿using System;
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
                var fileName = Path.GetFileNameWithoutExtension(
                    EmbeddedResources.ResourceNameToFileName(resourceName)
                ) + EmbeddedResources.GetNativeExtensionForCurrentPlatform();

                var libraryPath = Path.Combine(targetDir, fileName);
                filePaths.Add(libraryPath);

                var embeddedPackageStream = EmbeddedResources.GetResourceStream(resourceName);
                var bzipStream = new BZip2InputStream(embeddedPackageStream);

                if (File.Exists(libraryPath))
                {
                    if (ModuleInitializer.BootConfig.SkipChecksumVerification)
                        continue;

                    var memoryBuffer = new byte[1024 * 1024 * 4];
                    var memoryStream = new MemoryStream(memoryBuffer);
                    bzipStream.CopyTo(memoryStream, 1024);

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

                Console.WriteLine($"Extracting: {libraryPath}");
                bzipStream.Close();
            }

            return filePaths;
        }
    }
}
