using System.Security.Cryptography;
using System.Text;

namespace Chroma.Natives.Boot
{
    internal static class NativeIntegrity
    {
        private static readonly MD5 MD5 = MD5.Create();

        public static bool ChecksumsMatch(byte[] existing, byte[] embedded)
        {
            var existingHash = MD5.ComputeHash(existing);
            var embeddedHash = MD5.ComputeHash(embedded);

            for (var i = 0; i < existingHash.Length; i++)
            {
                if (existingHash[i] != embeddedHash[i])
                    return false;
            }

            return true;
        }
    }
}