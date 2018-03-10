using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Explorer.Configuration;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Node.Core.Models;
using Node.Requests;

namespace Explorer
{
    public class NodeCommunicator
    {
        private readonly string _nodeAddress;

        public NodeCommunicator(IOptions<NodeInformation> nodeInformation)
        {
            _nodeAddress = nodeInformation.Value.Address;
        }

        public string NodeAddress
        {
            get { return _nodeAddress; }
        }

        public async Task<IEnumerable<Block>> GetBlocks()
        {
            var url = _nodeAddress.AppendPathSegments("api", "block");
            return await url.GetJsonAsync<IEnumerable<Block>>();
        }

        public async Task<Block> GetBlock(int id)
        {
            var url = _nodeAddress.AppendPathSegments("api", "block", id.ToString());
            var request = new FlurlRequest(url);

            var response = await request.SendAsync(HttpMethod.Get);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await Task.FromResult(response).ReceiveJson<Block>();
        }

        public async Task<Transaction> GetTransaction(string hash)
        {
            var url = _nodeAddress.AppendPathSegments("api", "transactions");
            url.QueryParams.Add("transactionHash", hash);

            var request = new FlurlRequest(url);

            var response = await request.SendAsync(HttpMethod.Get);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await Task.FromResult(response).ReceiveJson<Transaction>();
        }


        public async Task<IEnumerable<Transaction>> GetTransactions(string address)
        {
            var url = _nodeAddress.AppendPathSegments("api", "transactions", address);

            var request = new FlurlRequest(url);

            var response = await request.SendAsync(HttpMethod.Get);

            return await Task.FromResult(response).ReceiveJson<IEnumerable<Transaction>>();
        }

        public async Task<decimal> GetBalance(string address, int confirmations)
        {
            return await _nodeAddress
                .AppendPathSegments("api", "transactions", address, "confirmations", confirmations)
                .GetJsonAsync<decimal>();
        }

        public async Task<IEnumerable<PendingTransaction>> GetPendingTransactions()
        {
            return await _nodeAddress
                .AppendPathSegments("api", "transactions", "pending")
                .GetJsonAsync<IEnumerable<PendingTransaction>>();
        }

        public async Task<bool> PublishTransaction(PendingTransactionRequest transaction)
        {
            var response = await _nodeAddress
                .AppendPathSegments("api", "transactions")
                .PostJsonAsync(transaction);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            var definition = new { accepted = false };
            return JsonConvert.DeserializeAnonymousType(content, definition).accepted;
        }
    }
}
