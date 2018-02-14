using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.Core.Models;
using Node.Core.Validators.Block;

namespace Node.CLI.Services
{
    public class BlockService
    {
        private readonly BlockRepository _blockChain;
        private readonly IBlockValidator _blockValidator;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public BlockService(BlockRepository blockChain, IMediator mediator, IMapper mapper, IBlockValidator blockValidator)
        {
            _blockChain = blockChain;
            _mediator = mediator;
            _mapper = mapper;
            _blockValidator = blockValidator;
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
            if (HasBiggerWeigth(blocks))
            {
                _blockChain.SyncBlocks(blocks);

                var blocksViewModel = blocks.Select(b => _mapper.Map<Block, BlockViewModel>(b));
                var notifyObject = new ChainViewModel {Blocks = blocksViewModel};
                await _mediator.Publish(notifyObject);
            }
        }

        public async Task<bool> AddBlock(Block block)
        {
            var prevBlock = _blockChain.GetBlocks().Last();
            var isValid = _blockValidator.Validate(block, prevBlock);

            if (isValid)
            {
                await AddBlockInternal(block);
            }

            return isValid;
        }

        private async Task AddBlockInternal(Block block)
        {
            var notify = _mapper.Map<Block, BlockViewModel>(block);
            await _mediator.Publish(notify);
            _blockChain.AddBlock(block);
        }

        private bool HasBiggerWeigth(IEnumerable<Block> blocks)
        {
            var oldBlocks = _blockChain.GetBlocks();
            var newBlockWeight = blocks.Sum(e => e.Difficulty);
            var oldBlockWeight = oldBlocks.Sum(e => e.Difficulty);

            return newBlockWeight > oldBlockWeight ||
                   newBlockWeight == oldBlockWeight && blocks.Count() > oldBlocks.Count();
        }
    }
}