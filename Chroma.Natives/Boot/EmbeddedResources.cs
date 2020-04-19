using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Boot
{
    internal static class EmbeddedResources
    {
        private static readonly Assembly ThisAssembly = Assembly.GetExecutingAssembly();

        public static string[] GetResourceNames()
            => ThisAssembly.GetManifestResourceNames();

        public static Stream GetResourceStream(string fullyQualifiedName)
            => ThisAssembly.GetManifestResourceStream(fullyQualifiedName);

        public static string ResourceNameToFileName(string fullyQualifiedName)
            => fullyQualifiedName.Replace($"Chroma.Natives.Binaries.{PlatformString}.", "");

        public static byte[] GetResourceBytes(string fullyQualifiedName)
        {
            using var embeddedResourceStream = GetResourceStream(fullyQualifiedName);
            using var memoryStream = new MemoryStream();
            embeddedResourceStream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        public static string PlatformString
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return "linux_64";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return "osx_64";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return "windows_64";
                else
                    throw new PlatformNotSupportedException("Unsupported platform.");
            }
        }
    }
}
