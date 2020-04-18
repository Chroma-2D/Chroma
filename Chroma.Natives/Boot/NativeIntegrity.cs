using System.Security.Cryptography;
using System.Text;

namespace Chroma.Natives.Boot
{
    internal static class NativeIntegrity
    {
        private static readonly MD5 MD5 = MD5.Create();

        public static bool ChecksumsMatch(byte[] existing, byte[] embedded)
        {
            var existingHash = HexChecksumString(MD5.ComputeHash(existing));
            var embeddedHash = HexChecksumString(MD5.ComputeHash(embedded));

            return existingHash == embeddedHash;
        }

        private static string HexChecksumString(byte[] data)
        {
            var sb = new StringBuilder();

            foreach (var b in data)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
