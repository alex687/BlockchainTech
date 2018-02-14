namespace Node.Core.Models
{
    public class Transaction
    {
        public Transaction(string hash, string from, string to, long amount, string senderPublickKey, string senderSignature, int blockIndex)
        {
            Hash = hash;
            From = from;
            To = to;
            Amount = amount;
            SenderPublickKey = senderPublickKey;
            SenderSignature = senderSignature;
            BlockIndex = blockIndex;
        }

        public string Hash { get; }

        public string From { get; }

        public string To { get; }

        public decimal Amount { get; }

        public string SenderPublickKey { get; }

        public string SenderSignature { get; }

        public int BlockIndex { get; }

        public override bool Equals(object obj)
        {
            return obj is Transaction item
                   && Hash.Equals(item.Hash)
                   && From.Equals(item.From)
                   && Amount.Equals(item.Amount)
                   && SenderPublickKey.Equals(item.SenderPublickKey)
                   && SenderSignature.Equals(item.SenderSignature)
                   && BlockIndex.Equals(item.BlockIndex)
                   && To.Equals(item.To);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}
