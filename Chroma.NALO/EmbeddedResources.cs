using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Chroma.NALO
{
    internal static class EmbeddedResources
    {
        public static string PlatformString
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return "linux_64";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return "windows_64";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return "osx_64";
                else
                    throw new PlatformNotSupportedException("Unsupported platform.");
            }
        }
        
        public static string PlatformSpecificNativeExtension
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return ".so";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return ".dll";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return ".dylib";
                else throw new PlatformNotSupportedException("Unsupported platform.");
            }
        }
        
        public static string[] GetResourceNames(Assembly assembly)
            => assembly.GetManifestResourceNames();

        public static Stream GetResourceStream(Assembly assembly, string fullyQualifiedName)
            => assembly.GetManifestResourceStream(fullyQualifiedName);

        public static string ResourceNameToFileName(Assembly assembly, string fullyQualifiedName)
            => fullyQualifiedName.Replace($"{assembly.GetName().Name}.Binaries.{PlatformString}.", "");

        public static byte[] GetResourceBytes(Assembly assembly, string fullyQualifiedName)
        {
            using var embeddedResourceStream = GetResourceStream(assembly, fullyQualifiedName);
            using var memoryStream = new MemoryStream();
            embeddedResourceStream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}
