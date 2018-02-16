using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Node.Core.Models;

namespace Minner
{
    public class Communication
    {
        private readonly string _minnerAddress;
        private readonly string _nodeUrl;

        public Communication(string nodeUrl, string minnerAddress)
        {
            _nodeUrl = nodeUrl;
            _minnerAddress = minnerAddress;
        }

        public async Task<Block> GetBlockToMine()
        {
            return await _nodeUrl
                .AppendPathSegments("api", "mining", _minnerAddress)
                .GetJsonAsync<Block>();
        }

        public async Task SendBlock(Block block)
        {
            await _nodeUrl
                .AppendPathSegments("api", "mining")
                .PostJsonAsync(block);
        }
    }
}