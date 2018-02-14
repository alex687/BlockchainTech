using System;
using System.Collections.Generic;
using MediatR;

namespace Node.CLI.Models
{
    public class BlockViewModel : INotification
    {
        public int Index { get; set; }

        public IEnumerable<TransactionViewModel> Transactions { get; set; }

        public int Difficulty { get; set; }

        public string PreviousBlockHash { get; set; }

        public string MinedBy { get; set; }

        public long Nonce { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Hash { get; set; }
    }
}