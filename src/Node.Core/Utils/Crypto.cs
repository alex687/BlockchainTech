using System.Security.Cryptography;
using System.Text;

namespace Node.Core.Utils
{
    public static class Crypto
    {
        public static string ComputeSha256Hash(string valueForHash)
        {
            byte[] hash;
            using (var sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(valueForHash));
            }

            var hashHex = new StringBuilder();
            foreach (var b in hash)
            {
                hashHex.Append($"{b:x2}");
            }

            return hashHex.ToString();
        }
    }
}
