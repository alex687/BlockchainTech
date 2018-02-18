using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Explorer.Configuration;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Node.Core.Models;

namespace Explorer
{
    public class NodeCommunicator
    {
        private readonly string _nodeAddress;

        public NodeCommunicator(IOptions<NodeInformation> nodeInformation)
        {
            _nodeAddress = nodeInformation.Value.Address;
        }

        public async Task<IEnumerable<Block>> GetBlocks()
        {
            var url = _nodeAddress.AppendPathSegments("api", "block");
            return await url.GetJsonAsync<IEnumerable<Block>>();
        }

        public async Task<Block> GetBlock(int id)
        {
            var url = _nodeAddress.AppendPathSegments("api", "block", id.ToString());

            return await url.GetJsonAsync<Block>();
        }
    }
}
