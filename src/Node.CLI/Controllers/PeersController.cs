using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Node.Core.Models;

namespace Node.CLI.Controllers
{
    public class PeersController
    {
        [HttpGet]
        public IEnumerable<string> GetPeers()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IEnumerable<string> AddPeer()
        {
            throw new NotImplementedException();
        }
    }
}
