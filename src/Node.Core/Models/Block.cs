using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Node.Core.Models
{
    public class Block
    {
        public Block(int index, List<Transaction> transactions, int difficulty, string previousBlockHash, string minedBy, long nonce, DateTime createdOn, string hash)
        {
            Index = index;
            Transactions = transactions.ToImmutableList();
            Difficulty = difficulty;
            PreviousBlockHash = previousBlockHash;
            MinedBy = minedBy;
            Nonce = nonce;
            CreatedOn = createdOn;
            Hash = hash;
        }

        public int Index { get; }

        public ImmutableList<Transaction> Transactions { get; }

        public int Difficulty { get; }

        public string PreviousBlockHash { get; }

        public string MinedBy { get; }

        public long Nonce { get; }

        public DateTime CreatedOn { get; }

        public string Hash { get; }
    }
}