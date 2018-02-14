using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    [Route("api/[controller]")]
    public class BlockController : Controller
    {
        private readonly BlockService _blockService;

        public BlockController(BlockService blockService)
        {
            _blockService = blockService;
        }

        [HttpGet]
        public IEnumerable<Block> GetBlocks()
        {
            return _blockService.GetBlocks();
        }

        [HttpGet("{id}")]
        public Block GetBlock(int id)
        {
            return _blockService.GetBlock(id);
        }

        [HttpPost("Sync")]
        public async Task SyncBlocks([FromBody] IEnumerable<Block> blocks)
        {
            await _blockService.SyncBlocks(blocks);
        }

        [HttpPost("Notify")]
        public async Task<object> Notify([FromBody] Block block)
        {
            var isAccepted = await _blockService.AddBlock(block);
            return new {Accepted  = isAccepted};
        }
    }
}