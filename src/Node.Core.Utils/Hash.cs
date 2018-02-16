using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities.Encoders;

namespace Node.Core.Crypto
{
    public static class Hash
    {
        public static string ComputeSha256Hash(this string valueForHash)
        {
            byte[] hash;
            using (var sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueForHash));
            }

            return Hex.ToHexString(hash);
        }

        public static string ComputeRipeMd160Hash(this string valueForHash)
        {
            var ripeMd160 = new RipeMD160Digest();
            var hash = new byte[ripeMd160.GetDigestSize()];
            foreach (var @byte in Encoding.UTF8.GetBytes(valueForHash))
            {
                ripeMd160.Update(@byte);
            }
            ripeMd160.DoFinal(hash, 0);

            return Hex.ToHexString(hash);
        }
      
        public static string ComputeHash(params string[] args)
        {
            var valueForHash = new StringBuilder();
            foreach (var arg in args)
            {
                valueForHash.Append(arg);
            }

            return valueForHash.ToString().ComputeSha256Hash();
        }
    }
}
