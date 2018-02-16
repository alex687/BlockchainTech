using Newtonsoft.Json;

namespace Node.Core.Models
{
    public class Transaction : PendingTransaction
    {
        [JsonConstructor]
        public Transaction(string hash, string from, string to, decimal amount, string senderPublickKey, string senderSignature, int blockIndex)
        :base(hash, from, to, amount, senderPublickKey, senderSignature)
        {
            BlockIndex = blockIndex;
        }

        public Transaction(PendingTransaction pt, int blockIndex)
         : base(pt.Hash, pt.From, pt.To, pt.Amount, pt.SenderPublickKey, pt.SenderSignature)
        {
            BlockIndex = blockIndex;
        }

        public int BlockIndex { get; }

        public override bool Equals(object obj)
        {
            return obj is Transaction item
                   && base.Equals(obj)
                   && To.Equals(item.To);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}
