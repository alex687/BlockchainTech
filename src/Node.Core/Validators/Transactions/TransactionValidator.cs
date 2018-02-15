﻿using System.Collections.Generic;
using System.Linq;
using Node.Core.Utils;
using Node.Core.Models;
using Node.Core.Repositories;
using Node.Core.Repositories.Blockchain;

namespace Node.Core.Validators.Transactions
{
    public class TransactionValidator : ITransactionValidator
    {
        private readonly TransactionsRepository _transactionsRepository;

        public TransactionValidator(TransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public bool Validate(IEnumerable<Transaction> transactions)
        {
            var addressSpendAmmounts = new Dictionary<string, decimal>();
            foreach (var transaction in transactions)
            {
                /*if (!IsValidAddress(transaction))
                {
                    return false;
                }
                */
                if (!IsValidSignature(transaction))
                {
                    return false;
                }

                if (!addressSpendAmmounts.ContainsKey(transaction.From))
                {
                    addressSpendAmmounts[transaction.From] = 0;
                }

                if (!addressSpendAmmounts.ContainsKey(transaction.To))
                {
                    addressSpendAmmounts[transaction.To] = 0;
                }

                addressSpendAmmounts[transaction.From] -= transaction.Amount;
                addressSpendAmmounts[transaction.To] += transaction.Amount;
            }


            foreach (var address in addressSpendAmmounts.Keys)
            {
                var currentChainBalance = GetBalance(address);
                if (currentChainBalance + addressSpendAmmounts[address] < 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Validate(Transaction transaction)
        {
            return Validate(new List<Transaction> { transaction });
        }

        private decimal GetBalance(string address)
        {
            var addressTransactions = _transactionsRepository.GetTransactions(address);

            var from = addressTransactions
                .Where(t => t.From == address)
                .Sum(t => t.Amount);

            var to = addressTransactions
                .Where(t => t.To == address)
                .Sum(t => t.Amount);

            return to - from;
        }

        private bool IsCoinbase(Transaction transaction)
        {
            return transaction.From == "conibase"; ;
        }

        private bool IsValidSignature(Transaction transaction)
        {
            return true;
        }

        private bool IsValidAddress(Transaction transaction)
        {
            return transaction.From == transaction.SenderPublickKey.ComputeRipeMd160Hash();
        }
    }
}
