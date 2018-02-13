using System;

namespace Node.Core.Models
{
    public class Transaction 
    {
        public string Hash { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public long Amount { get; set; }

        public string SenderPublickKey { get; set; }

        public string SenderSignature { get; set; }

        public DateTime ReceivedOn { get; set; }
    }
}
