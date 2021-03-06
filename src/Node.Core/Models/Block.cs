﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;

namespace Node.Core.Models
{
    public class Block
    {
        [JsonConstructor]
        public Block(int index, IEnumerable<Transaction> transactions, int difficulty, string previousBlockHash,
            string minedBy, long nonce, DateTime createdOn, string hash)
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

        public long Nonce { get; set; }

        public DateTime CreatedOn { get; }

        public string Hash { get; }

        public override bool Equals(object obj)
        {
            return obj is Block item
                   && Hash.Equals(item.Hash)
                   && Index.Equals(item.Index)
                   && Difficulty.Equals(item.Difficulty)
                   && PreviousBlockHash.Equals(item.PreviousBlockHash)
                   && MinedBy.Equals(item.MinedBy)
                   && Nonce.Equals(item.Nonce)
                   && CreatedOn.Equals(item.CreatedOn)
                   && Transactions.SequenceEqual(item.Transactions);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }
    }
}