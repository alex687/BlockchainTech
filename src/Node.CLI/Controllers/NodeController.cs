using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Node.Core;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    [Route("api/[controller]")]
    public class NodeController : Controller
    {
        private readonly IBlockchain _blockchain;

        public NodeController(IBlockchain blockchain)
        {
            _blockchain = blockchain;
        }

        [HttpGet]
        public IEnumerable<Block> GetBlocks()
        {
            return _blockchain.GetBlocks();
        }
        
        [HttpGet("{id}")]
        public Block GetBlock(int id)
        {
            return _blockchain.GetBlock(id);
        }
        
        [HttpPost]
        public void SyncBlocks([FromBody] IEnumerable<Block> blocks)
        {
            
        }
        
        public void AddBlock([FromBody] Block block)
        {
            _blockchain.AddBlock(block);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}