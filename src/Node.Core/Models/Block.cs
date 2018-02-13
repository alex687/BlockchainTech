using System;
using System.Collections.Generic;

namespace Node.Core.Models
{
    public class Block
    {
        public static Block Genesis { get; } = new Block()
        {
            Index = 0,
            Transactions = new List<Transaction>
            {
                new Transaction
                {
                    Amount = 500,
                    From = "Me",
                    To = "Me",
                    SenderPublickKey = "",
                    ReceivedOn = DateTime.Now,
                    SenderSignature = "",
                    Hash = "024142"
                }
            },
            CreatedOn = DateTime.Now,
            Difficulty = 1,
            Hash = "0231"
        };

        public int Index { get; set; }

        public List<Transaction> Transactions { get; set; }

        public int Difficulty { get; set; }

        public string PreviousBlockHash { get; set; }

        public string MinedBy { get; set; }

        public long Nonce { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Hash { get; set; }
    }
}