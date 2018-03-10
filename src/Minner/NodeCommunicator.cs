using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Node.Core.Models;

namespace Minner
{
    public class NodeCommunicator
    {
        private readonly string _nodeUrl;

        public NodeCommunicator(string nodeUrl)
        {
            _nodeUrl = nodeUrl;
        }

        public async Task<Block> GetBlockToMine(string minnerAddress)
        {
            return await _nodeUrl
                .AppendPathSegments("api", "mining", minnerAddress)
                .GetJsonAsync<Block>();
        }

        public async Task<bool> SendBlock(Block block)
        {
            var response = await _nodeUrl
                .AppendPathSegments("api", "mining")
                .PostJsonAsync(block);

            var content = await response.Content.ReadAsStringAsync();

            return bool.Parse(content);
        }
    }
}