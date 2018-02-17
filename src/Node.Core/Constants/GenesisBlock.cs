using System;
using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core.Constants
{
    public static class Genesis
    {
        public static readonly string MinerRewardSource = "coinbase";

        public static readonly Block Block = new Block
        (
            index:0,
            createdOn : new DateTime(2018, 1, 1),
            difficulty : 10,
            transactions : new List<Transaction>
            {
                new Transaction
                (
                    from : "World creation",
                    to: "8e74d1ae62c6aa3e08691b9a29cbd521da5f1e19",
                    amount: 8218382,
                    blockIndex:0,
                    senderPublickKey: "Our public key",
                    senderSignature: "TEST",
                    hash: "rakiq"
                )
            },
            hash : "87348723",
            minedBy : "God",
            nonce: 1,
            previousBlockHash: "0"
        );
    }
}