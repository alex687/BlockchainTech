using System;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Node.Requests;

namespace Wallet
{
    public class NodeCommunicator
    {
        private readonly string _nodeAddress;

        public NodeCommunicator(string nodeAddress)
        {
            _nodeAddress = nodeAddress;
        }

        public async Task<bool> PublishTransaction(PendingTransactionRequest transaction)
        {
            var response =  await _nodeAddress.AppendPathSegments("api", "transactions")
                .PostJsonAsync(transaction);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var definition = new { accepted = false };

                return JsonConvert.DeserializeAnonymousType(content, definition).accepted;
            }

            return false;
        }

        public async Task<decimal> GetBalance(string address, int confirmations)
        {
            var response = await _nodeAddress.AppendPathSegments("api", "transactions", address, "confirmations", confirmations).GetAsync();
            var ammount = await response.Content.ReadAsStringAsync();

            return decimal.Parse(ammount);
        }
    }
}
