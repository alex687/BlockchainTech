using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.Requests;
using Wallet;

namespace Explorer.Controllers
{
    public class FaucetController : Controller
    {
        private readonly NodeCommunicator _communicator;
        private WalletManager _walletManager;

        public FaucetController(NodeCommunicator communicator, WalletManager walletManager)
        {
            _communicator = communicator;
            _walletManager = walletManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string address )
        {
            var blocks = await _communicator.GetBlocks();
            var transactions = blocks.OrderByDescending(b => b.Index).Take(5).SelectMany(b => b.Transactions);

            bool isAccepted = false;
            if (transactions.Count(t => t.To == address && t.From == _walletManager.GetAddress()) < 5)
            {
                var pendingTransaction = new PendingTransactionRequest
                {
                    SenderPublickKey = _walletManager.GetPublicKey(),
                    From = _walletManager.GetAddress(),
                    To = address,
                    Amount = 5
                };

                _walletManager.SignTransactionRequest(pendingTransaction);
                _walletManager.SetTransactionRequestHash(pendingTransaction);
                isAccepted = await _communicator.PublishTransaction(pendingTransaction);
            }
            

            return View("Index", new {IsAccepted = isAccepted});
        }
    }
}
