using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    public class TransactionsController
    {

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
