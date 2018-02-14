using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.Core.Models;
using Node.Core.Repositories;
using Node.Core.Validators.Block;

namespace Node.CLI.Services
{
    public class BlockService
    {
        private readonly BlockRepository _blockChain;
        private readonly IMediator _mediator;

        public BlockService(BlockRepository blockChain, IMediator mediator)
        {
            _blockChain = blockChain;
            _mediator = mediator;
        }

        public IEnumerable<Block> GetBlocks()
        {
            return _blockChain.GetBlocks();
        }

        public Block GetBlock(int id)
        {
            return _blockChain.GetBlock(id);
        }

        public int LastBlockId()
        {
            return _blockChain.GetBlocks().Count() - 1;
        }

        public async Task SyncBlocks(IEnumerable<Block> blocks)
        {
            if (_blockChain.TrySyncBlocks(blocks))
            {
                var notifyObject = new ChainNotify(blocks);
                await _mediator.Publish(notifyObject);
            }
        }

        public async Task<bool> AddBlock(Block block)
        {
            if (_blockChain.TryAddBlock(block))
            {
				var notify = new BlockNotify(block);
				await _mediator.Publish(notify);
                return true;
            }
	
            return false;
        }


    }
}