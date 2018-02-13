using System;

namespace Node.Core.Models
{
    public class Transaction
    {
        public Transaction(string hash, string from, string to, long amount, string senderPublickKey, string senderSignature, DateTime receivedOn)
        {
            Hash = hash;
            From = from;
            To = to;
            Amount = amount;
            SenderPublickKey = senderPublickKey;
            SenderSignature = senderSignature;
            ReceivedOn = receivedOn;
        }

        public string Hash { get; }

        public string From { get; }

        public string To { get; }

        public long Amount { get; }

        public string SenderPublickKey { get; }

        public string SenderSignature { get; }

        public DateTime ReceivedOn { get; }
    }
}
