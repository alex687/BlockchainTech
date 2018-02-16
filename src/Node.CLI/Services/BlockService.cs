using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Configurations;
using Node.CLI.InternalModels;
using Node.Core.Factory;
using Node.Core.Models;
using Node.Core.Repositories.Blockchain;

namespace Node.CLI.Services
{
    public class BlockService
    {
        private readonly IMediator _mediator;
        private readonly BlockchainInstanceHolder _blockchainInstance;
        private readonly BlockchainFactory _factory;

        public BlockService(IMediator mediator, BlockchainInstanceHolder blockchainInstance, BlockchainFactory factory)
        {
            _mediator = mediator;
            _blockchainInstance = blockchainInstance;
            _factory = factory;
        }

        public IEnumerable<Block> GetBlocks()
        {
            return _blockchainInstance.BlockRepository.GetBlocks();
        }

        public Block GetBlock(int id)
        {
            return _blockchainInstance.BlockRepository.GetBlock(id);
        }

        public int LastBlockId()
        {
            return _blockchainInstance.BlockRepository.GetBlocks().Count() - 1;
        }

        public async Task SyncBlocks(IEnumerable<Block> blocks)
        {
            if (HasBiggerWeigth(blocks))
            {
                var newBlockchain = _factory.CreateBlockchain();
                
                // Skipping Genesis block.
                foreach (var block in blocks.Skip(1))
                {
                    if (!newBlockchain.TryAddBlock(block))
                    {
                        return;
                    }
                }

                _blockchainInstance.BlockRepository = newBlockchain;
                var notifyObject = new ChainNotify(blocks);
                await _mediator.Publish(notifyObject);
            }
        }

        public async Task<bool> AddBlock(Block block)
        {
            if (_blockchainInstance.BlockRepository.TryAddBlock(block))
            {
				var notify = new BlockNotify(block);
				await _mediator.Publish(notify);
                return true;
            }
	
            return false;
        }

        private bool HasBiggerWeigth(IEnumerable<Block> newBlocks)
        {
            var currentBlocks = _blockchainInstance.BlockRepository.GetBlocks();
            var newBlockWeight = newBlocks.Sum(e => e.Difficulty);
            var oldBlockWeight = currentBlocks.Sum(e => e.Difficulty);

            return newBlockWeight > oldBlockWeight ||
                   newBlockWeight == oldBlockWeight && newBlocks.Count() > currentBlocks.Count();
        }
    }
}