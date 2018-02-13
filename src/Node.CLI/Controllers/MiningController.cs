using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    public class MiningController
    {
        [HttpGet]
        public IEnumerable<Block> GetBlock(string address)
        {
            // we have to save the mining jobs per address
            throw new NotImplementedException();
        }

        [HttpPost]
        public IEnumerable<Block> SubmitBlock([FromBody] Block block)
        {
            throw new NotImplementedException();
        }
    }
}