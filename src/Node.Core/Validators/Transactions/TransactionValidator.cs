using System.Collections.Generic;
using System.Linq;
using Node.Core.Constants;
using Node.Core.Crypto;
using Node.Core.Extensions;
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
            var validMinerTransaction = true;
            var conatinsSameBlockIndex = transactions.All(t => t.BlockIndex == blockIndex);
            var onlyOneMinerReward = transactions.Count(IsCoinbase) <= 1;
            var anyDuplicated = IsAllUniqueTransactions(transactions);
            var minerRewardTransaction = transactions.FirstOrDefault(IsCoinbase);

            if (minerRewardTransaction != null)
            {
                transactions = transactions.Except(new[] { minerRewardTransaction });
                validMinerTransaction = IsRewardInRange(minerRewardTransaction, transactions);
            }

            return PendingTransactionsValidate(transactions) &&
                   validMinerTransaction &&
                   onlyOneMinerReward &&
                   conatinsSameBlockIndex &&
                   anyDuplicated;
        }

        private static bool IsAllUniqueTransactions(IEnumerable<Transaction> transactions)
        {
            return transactions.GroupBy(x => x.Hash).All(g => g.Count() <= 1);
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
            return transaction.From == Genesis.MinerRewardSource;
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

        private bool IsRewardInRange(PendingTransaction minerRewardTransaction, IEnumerable<Transaction> transactions)
        {
            return transactions.CalculateReward() >= minerRewardTransaction.Amount;
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
