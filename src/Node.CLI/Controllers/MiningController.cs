using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    [Route("api/[controller]")]
    public class MiningController
    {
        private readonly MiningService _miningService;

        public MiningController(MiningService miningService)
        {
            _miningService = miningService;
        }

        [HttpGet("{address}")]
        public Block GetBlock(string address)
        {
            return _miningService.CreateNewBlock(address);
        }

        [HttpPost]
        public async Task<bool> SubmitBlock([FromBody] Block block)
        {
            return await _miningService.AddBlock(block);
        }
    }
}