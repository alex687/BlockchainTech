using System;
using System.Linq;
using System.Threading.Tasks;
using Node.Core.Models;
using Node.Core.Repositories;

namespace Node.CLI.Services
{
    public class MiningService
    {
        private readonly MiningJobsRepository _jobRepository;
        private readonly BlockService _blockService;
        private readonly TransactionService _transactionService;

        public MiningService(
            MiningJobsRepository jobRepository,
            BlockService blockService,
            TransactionService transactionService)
        {
            _jobRepository = jobRepository;
            _blockService = blockService;
            _transactionService = transactionService;
        }

        public Block CreateNewBlock(string minerAddress)
        {
            var lastBlock = _blockService.GetBlocks().Last();
            var blockIndex = lastBlock.Index + 1;
            var transactions = _transactionService
                .GetPendingTransactions()
                .Select(pt => new Transaction(pt, blockIndex));

            var blockToMine = new Block(
                blockIndex,
                transactions,
                5,
                lastBlock.Hash,
                minerAddress,
                0,
                DateTime.Now,
                string.Empty);

            _jobRepository.PutMinnerJob(minerAddress, blockToMine);

            return blockToMine;
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
