using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Models;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController
    {
        private readonly TransactionService _transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public Transaction Get(string transactionHash)
        {
            return _transactionService.GetTransaction(transactionHash);
        }

        [HttpGet("{address}/confirmations/{confirmations}")]
        public decimal GetBalance(string address, int confirmations)
        {
            return _transactionService.GetBalance(address, confirmations);
        }

        [HttpPost]
        public async Task Send([FromBody] Transaction transaction)
        {
            await _transactionService.AddPendingTransaction(transaction);
        }
    }
}