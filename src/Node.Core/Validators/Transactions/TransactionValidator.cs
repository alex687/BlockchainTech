using System.Collections.Generic;
using System.Linq;
using Node.Core.Crypto;
using Node.Core.Utils;
using Node.Core.Models;
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

        public bool MinedTransactionsValidate(IEnumerable<Transaction> transactions, int blockIndex)
        {
            var conatinsSameBlockIndex = transactions.All(t => t.BlockIndex == blockIndex);
            var hasOnlyOneCoinbaseTransaction = transactions.Count(IsCoinbase) <= 1;
            //var containDuplicateTransactions = transactions.Distinct();

            //TODO Check for duplicated transactions, in the block and in the repository.

            return hasOnlyOneCoinbaseTransaction && conatinsSameBlockIndex && PendingTransactionsValidate(transactions);
        }

        public bool PendingTransactionsValidate(IEnumerable<PendingTransaction> transactions)
        {
            var addressSpendAmmounts = new Dictionary<string, decimal>();
            foreach (var transaction in transactions)
            {
                if (!IsValidAddress(transaction) || !IsValidHash(transaction) || !IsValidSignature(transaction))
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

        public bool Validate(PendingTransaction transaction)
        {
            return PendingTransactionsValidate(new List<PendingTransaction> { transaction });
        }

        private bool IsCoinbase(PendingTransaction transaction)
        {
            return transaction.From == "conibase"; ;
        }

        private bool IsValidSignature(PendingTransaction transaction)
        {
            return transaction.IsValidSignature();
        }

        private bool IsValidAddress(PendingTransaction transaction)
        {
            return transaction.From == transaction.SenderPublickKey.ComputeRipeMd160Hash();
        }

        private bool IsValidHash(PendingTransaction transaction)
        {
            return transaction.Hash == transaction.ComputeHash();
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
    }
}
