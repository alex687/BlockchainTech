using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Node.Core.Models;
using Org.BouncyCastle.Crypto.Digests;

namespace Node.Core.Utils
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

            var hashHex = new StringBuilder();
            foreach (var b in hash)
            {
                hashHex.Append($"{b:x2}");
            }

            return hashHex.ToString();
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


            var hashHex = new StringBuilder();
            foreach (var b in hash)
            {
                hashHex.Append($"{b:x2}");
            }

            return hashHex.ToString();
        }

        public static string ComputeHash(this Block block)
        {
            var transactionsHash = string.Join(string.Empty, block.Transactions.Select(t => t.ComputeHash()));

            return ComputeHash(block.Index.ToString(), block.PreviousBlockHash, block.CreatedOn.ToString("O"),
                transactionsHash, block.Difficulty.ToString(), block.Nonce.ToString());
        }

        public static string ComputeHash(this Transaction transaction)
        {
            return ComputeHash(transaction.From, transaction.To, transaction.SenderPublickKey,
                transaction.SenderSignature, transaction.Amount.ToString());
        }

        private static string ComputeHash(params string[] args)
        {
            string valueForHash = args.Aggregate((s1, s2) => s1 + s2);

            return valueForHash.ComputeSha256Hash();
        }
    }
}
