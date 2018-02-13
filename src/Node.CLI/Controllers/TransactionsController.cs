using System;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
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
            // TODO transaction ViewModel
            throw new NotImplementedException();
        }

        [HttpGet("{address}/confirmations/{confirmations}")]
        public Transaction GetBalance(string address, int confirmations)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public void Send([FromBody] Transaction blocks)
        {
            // TODO transaction ViewModel
        }
    }
}