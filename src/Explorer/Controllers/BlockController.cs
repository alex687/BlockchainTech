using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Controllers
{
    public class BlockController : Controller
    {
        private readonly NodeCommunicator _nodeCommunicator;

        public BlockController(NodeCommunicator nodeCommunicator)
        {
            _nodeCommunicator = nodeCommunicator;
        }

        public async Task<IActionResult> Index()
        {
            var blocks = await _nodeCommunicator.GetBlocks();
            var orderedBlocks = blocks.OrderByDescending(b => b.Index);

            return View(orderedBlocks);
        }


        public async Task<IActionResult> Details(int id)
        {
            var blocks = await _nodeCommunicator.GetBlock(id);

            return View(blocks);
        }
    }
}
