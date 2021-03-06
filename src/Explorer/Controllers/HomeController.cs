﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Explorer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Explorer.Models;
using Microsoft.Extensions.Options;
using PagedList.Core;

namespace Explorer.Controllers
{
    public class HomeController : Controller
    {
        private readonly NodeCommunicator _nodeCommunicator;

        public HomeController(NodeCommunicator nodeCommunicator)
        {
            _nodeCommunicator = nodeCommunicator;
        }

        public IActionResult Index()
        {
            var address = _nodeCommunicator.NodeAddress;

            return View("Index", address);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
