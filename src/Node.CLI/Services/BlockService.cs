using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.InternalModels;
using Node.Core.Factory;
using Node.Core.Models;

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
            return _blockchainInstance.Blockchain.GetBlocks();
        }

        public Block GetBlock(int id)
        {
            return _blockchainInstance.Blockchain.GetBlock(id);
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

                foreach (var pendingTransaction in _blockchainInstance.Blockchain.GetPending())
                {
                    newBlockchain.AddPending(pendingTransaction);
                }

                _blockchainInstance.Blockchain = newBlockchain;
                var notifyObject = new ChainNotify(blocks);
                await _mediator.Publish(notifyObject);
            }
        }

        public async Task<bool> AddBlock(Block block)
        {
            if (_blockchainInstance.Blockchain.TryAddBlock(block))
            {
				var notify = new BlockNotify(block);
				await _mediator.Publish(notify);
                return true;
            }
	
            return false;
        }

        private bool HasBiggerWeigth(IEnumerable<Block> newBlocks)
        {
            var currentBlocks = _blockchainInstance.Blockchain.GetBlocks();
            var newBlockWeight = newBlocks.Sum(e => e.Difficulty);
            var oldBlockWeight = currentBlocks.Sum(e => e.Difficulty);

            return newBlockWeight > oldBlockWeight ||
                   newBlockWeight == oldBlockWeight && newBlocks.Count() > currentBlocks.Count();
        }
    }
}