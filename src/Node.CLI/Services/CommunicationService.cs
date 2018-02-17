using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Node.CLI.InternalModels;
using Node.Core.Models;
using Node.Core.Repositories;
using Node.Requests;

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
            var url = address.AppendPathSegments("api", "peers");
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
                    .PostJsonAsync(newBlock.Block);
        }

        public void PublishTransaction(PendingTransaction transaction)
        {
            var transactionRequest = new PendingTransactionRequest
            {
                Amount = transaction.Amount,
                From = transaction.From,
                Hash = transaction.Hash,
                SenderPublickKey = transaction.SenderPublickKey,
                SenderSignature = transaction.SenderSignature,
                To = transaction.To
            };

            foreach (var peer in _peers.GetAll())
            {
                peer.Address.AppendPathSegments("api", "transactions")
                    .PostJsonAsync(transactionRequest);
            }
        }
    }
}