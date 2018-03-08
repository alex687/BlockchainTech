using System.Threading.Tasks;
using Explorer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Controllers
{
    public class TransactionController : Controller
    {
        private readonly NodeCommunicator _nodeCommunicator;

        public TransactionController(NodeCommunicator nodeCommunicator)
        {
            _nodeCommunicator = nodeCommunicator;
        }


        [HttpGet("Transaction/Details/{hash}")]
        public async Task<IActionResult> Details(string hash)
        {
            var transaction = await _nodeCommunicator.GetTransaction(hash);
            if (transaction == null)
            {
                View("NotFound", hash);
            }

            return View(transaction);
        }
    }
}
