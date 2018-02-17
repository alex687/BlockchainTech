using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Node.Core.Constants;
using Node.Core.Crypto;
using Node.Core.Extensions;
using Node.Core.Models;
using Node.Core.Repositories;

namespace Node.CLI.Services
{
    public class MiningService
    {
        private readonly MiningJobsRepository _jobRepository;
        private readonly BlockService _blockService;
        private readonly TransactionService _transactionService;

        public MiningService(MiningJobsRepository jobRepository, BlockService blockService, TransactionService transactionService)
        {
            _jobRepository = jobRepository;
            _blockService = blockService;
            _transactionService = transactionService;
        }

        public Block CreateNewBlock(string minerAddress)
        {
            var lastBlock = _blockService.GetBlocks().Last();
            var blockIndex = lastBlock.Index + 1;
            var transactions = GetBlockTransactions(minerAddress, blockIndex);
            
            var blockToMine = new Block(blockIndex, transactions, 5, lastBlock.Hash, minerAddress, 0, DateTime.Now, string.Empty);

            _jobRepository.PutMinnerJob(minerAddress, blockToMine);

            return blockToMine;
        }

        private IEnumerable<Transaction> GetBlockTransactions(string minerAddress, int blockIndex)
        {
            var transactions = _transactionService
                .GetPendingTransactions()
                .Select(pt => new Transaction(pt, blockIndex))
                .ToList();

            var rewardAmount = transactions.CalculateReward();
            var rewardTransaction = CreateMinerReward(minerAddress, rewardAmount, blockIndex);
            transactions.Add(rewardTransaction);

            return transactions;
        }

        private static Transaction CreateMinerReward(string minerAddress, decimal  reward, int blockIndex)
        {
            var hash = Hash.ComputeHash(Genesis.MinerRewardSource, minerAddress, string.Empty, string.Empty, reward.ToString());
            return new Transaction(hash, Genesis.MinerRewardSource, minerAddress, reward, string.Empty, string.Empty, blockIndex);
        }

        public async Task<bool> AddBlock(Block block)
        {
            var minnerJob = _jobRepository.GetJob(block.MinedBy);
            if (minnerJob.Transactions.SequenceEqual(block.Transactions))
            {
                return await _blockService.AddBlock(block);
            }

            return false;
        }
    }
}
