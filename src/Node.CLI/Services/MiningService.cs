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
        private readonly PendingTransactionRepository _transactionRepository;

        public MiningService(
            MiningJobsRepository jobRepository,
            BlockService blockService,
            PendingTransactionRepository transactionRepository)
        {
            _jobRepository = jobRepository;
            _blockService = blockService;
            _transactionRepository = transactionRepository;
        }

        public Block CreateNewBlock(string minerAddress)
        {
            var lastBlockId = _blockService.LastBlockId();
            var lastBlock = _blockService.GetBlock(lastBlockId);
            var transactions = _transactionRepository.GetPending();
            
            var blockToMine = new Block(
                ++lastBlockId,
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
