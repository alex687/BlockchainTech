using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Node.Core.Models;

namespace Minner
{
    public class NodeCommunicator
    {
        private readonly Logger _logger;
        private readonly string _minnerAddress;
        private readonly string _nodeUrl;

        public NodeCommunicator(Logger logger, string nodeUrl, string minnerAddress)
        {
            _logger = logger;
            _nodeUrl = nodeUrl;
            _minnerAddress = minnerAddress;
        }

        public async Task<Block> GetBlockToMine()
        {
            var block = await _nodeUrl
                .AppendPathSegments("api", "mining", _minnerAddress)
                .GetJsonAsync<Block>();

            _logger.Log($"Getting latest block: Index = {block.Index}");
            return block;
        }

        public async Task SendBlock(Block block)
        {
            _logger.Log($"Sending minned block: Index = {block.Index}");
            await _nodeUrl
                .AppendPathSegments("api", "mining")
                .PostJsonAsync(block);
        }
    }
}