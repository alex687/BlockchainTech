using System.Linq;
using System.Threading.Tasks;
using Explorer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Controllers
{
    public class AddressController : Controller
    {
        private readonly NodeCommunicator _nodeCommunicator;

        public AddressController(NodeCommunicator nodeCommunicator)
        {
            _nodeCommunicator = nodeCommunicator;
        }

        [HttpGet("Address/Information/{address}")]
        public async Task<IActionResult> Information(string address)
        {
            var transactions = await _nodeCommunicator.GetTransactions(address);
            var orderedTransactions = transactions.OrderByDescending(b => b.BlockIndex);
            var balance = await _nodeCommunicator.GetBalance(address, 0);

            var model = new AddressInformationViewModel
            {
                Transactions = orderedTransactions.ToList(),
                Balance = balance,
                Address = address
            };

            return View(model);
        }

        [HttpPost("Address/Search")]
        public IActionResult Search(string address)
        {
            return RedirectToAction("Information", "Address", new { address = address });
        }
    }
}
