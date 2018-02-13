using System;
using System.Collections.Generic;
using Node.Core.Models;

namespace Node.Core.Constants
{
    public static class Genesis
    {
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
                    to: "OUR ADDRESS THAT WE HAVE PRIVATE KEY",
                    amount: 8218382,
                    receivedOn: new DateTime(2018, 1, 1),
                    senderPublickKey: "Our public key",
                    senderSignature: "TEST",
                    hash: "ashh"
                )
            },
            hash : "87348723",
            minedBy : "God",
            nonce: 1,
            previousBlockHash: "0"
        );
    }
}