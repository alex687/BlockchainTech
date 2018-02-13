using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Node.CLI.Models;
using Node.Core;
using Node.Core.Models;

namespace Node.CLI.Services
{
    public class BlockService
    {
        private readonly IBlockchain _blockChain;
        private readonly IMediator _mediator;

        public BlockService(IBlockchain blockChain, IMediator mediator)
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

        public async Task SyncBlocks(IEnumerable<Block> blocks)
        {
            if (IsBlockBigger(blocks))
            {
                _blockChain.SyncBlocks(blocks);
                var notifyObject = new ChainViewModel {Blocks = blocks};
                await _mediator.Publish(notifyObject);
            }
        }

        public void AddBlock(Block block)
        {
            _blockChain.AddBlock(block);
        }

        private bool IsBlockBigger(IEnumerable<Block> blocks)
        {
            var oldBlocks = _blockChain.GetBlocks();
            var newBlockWeight = blocks.Sum(e => e.Difficulty);
            var oldBlockWeight = oldBlocks.Sum(e => e.Difficulty);

            return newBlockWeight > oldBlockWeight || 
                   (newBlockWeight == oldBlockWeight && blocks.Count() >  oldBlocks.Count());
        }
    }
}
