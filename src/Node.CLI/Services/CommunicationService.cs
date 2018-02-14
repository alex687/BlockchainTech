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

        public async Task PublishChain(ChainNotify newChain)
        {
            foreach (var peer in _peers.GetAll())
                await peer.Address.AppendPathSegments("api", "block", "sync")
                    .PostJsonAsync(newChain.Blocks);
        }

        public async Task PublishBlock(BlockNotify newBlock)
        {
            foreach (var peer in _peers.GetAll())
                await peer.Address.AppendPathSegments("api", "block", "notify")
                    .PostJsonAsync(newBlock);
        }

        public async Task PublishTransaction(Transaction transaction)
        {
            foreach (var peer in _peers.GetAll())
                await peer.Address.AppendPathSegments("api", "transactions")
                    .PostJsonAsync(transaction);
        }
    }
}