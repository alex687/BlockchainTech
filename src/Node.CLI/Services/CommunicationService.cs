using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Node.CLI.Models;
using Node.CLI.Repositories;
using Node.Core.Models;

namespace Node.CLI.Services
{
    public class CommunicationService
    {
        private readonly PeerRepository _peers;

        public CommunicationService(PeerRepository peers)
        {
            _peers = peers;
        }

        public async Task<IEnumerable<Block>> GetPeerBlocks(string address)
        {
            var url = address.AppendPathSegments("api", "block");
            return await url.GetJsonAsync<IEnumerable<Block>>();
        }

        public async Task<IEnumerable<Peer>> GetPeerList(string address)
        {
            var url = address.AppendPathSegments("api", "peer");
            return await url.GetJsonAsync<IEnumerable<Peer>>();
        }

        public async Task PostPeerList(string address)
        {
            var url = address.AppendPathSegments("api", "peer");
            await url.PostJsonAsync(_peers.GetAll());
        }

        public async Task NotifyAll(ReplacedChainNotify newChain)
        {
            foreach (var peer in _peers.GetAll())
                await peer.Address.AppendPathSegments("api", "block", "sync")
                    .PostJsonAsync(newChain.Blocks);
        }

        public async Task NotifyAll(AddedBlockToChainNotify newBlock)
        {
            foreach (var peer in _peers.GetAll())
                await peer.Address.AppendPathSegments("api", "block", "notify")
                    .PostJsonAsync(newBlock);
        }
    }
}