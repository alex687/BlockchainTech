using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Services;
using Node.Core.Models;
using Node.Requests;

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

        [HttpGet("pending/")]
        public IEnumerable<PendingTransaction> GetPendingTransactions()
        {
            return _transactionService.GetPendingTransactions();
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

        [HttpGet("{address}")]
        public IEnumerable<Transaction> GetTransactions(string address)
        {
            return _transactionService.GetTransactions(address);
        }

        [HttpPost]
        public async Task<object> Send([FromBody] PendingTransactionRequest tr)
        {
            var transaction = new PendingTransaction(tr.Hash, tr.From, tr.To, tr.Amount, tr.SenderPublickKey, tr.SenderSignature);
            var isAccepted = await _transactionService.AddPendingTransaction(transaction);

            return new { Accepted = isAccepted };
        }
    }
}