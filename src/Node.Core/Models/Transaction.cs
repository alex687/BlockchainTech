using System;

namespace Node.Core.Models
{
    public class Transaction
    {
        public string Hash { get; set; }

        public Address From { get; set; }

        public Address To { get; set; }

        public long Amount { get; set; }

        public string SenderPublickKey { get; set; }

        public string SenderSignature { get; set; }

        public DateTime ReceivedOn { get; set; }

        public static string GetTransactionHash(object transaction)
        {
            var forHash = JsonConvert.SerializeObject(transaction);

            var hash = Crypto.ComputeSha256Hash(forHash);
            return hash;
        }
    }
}
