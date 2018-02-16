using Newtonsoft.Json;

namespace Node.Core.Models
{
    public class PendingTransaction
    {
        [JsonConstructor]
        public PendingTransaction(string hash, string from, string to, decimal amount, string senderPublickKey, string senderSignature)
        {
            Hash = hash;
            From = from;
            To = to;
            Amount = amount;
            SenderPublickKey = senderPublickKey;
            SenderSignature = senderSignature;
        }

        public string Hash { get; }

        public string From { get; }

        public string To { get; }

        public decimal Amount { get; }

        public string SenderPublickKey { get; }

        public string SenderSignature { get; }

        public override bool Equals(object obj)
        {
            return obj is PendingTransaction item
                   && Hash.Equals(item.Hash)
                   && From.Equals(item.From)
                   && Amount.Equals(item.Amount)
                   && SenderPublickKey.Equals(item.SenderPublickKey)
                   && SenderSignature.Equals(item.SenderSignature)
                   && To.Equals(item.To);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}
