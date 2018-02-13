using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Node.CLI.Services;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    [Route("api/[controller]")]
    public class MiningController
    {
        private readonly MiningService _miningService;

        public MiningController(MiningService miningService)
        {
            _miningService = miningService;
        }

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