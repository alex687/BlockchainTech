using System;
using MediatR;

namespace Node.CLI.Models
{
    public class TransactionViewModel : INotification
    {
        public string Hash { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Amount { get; set; }

        public string SenderPublickKey { get; set; }

        public string SenderSignature { get; set; }

        public DateTime ReceivedOn { get; set; }
    }
}