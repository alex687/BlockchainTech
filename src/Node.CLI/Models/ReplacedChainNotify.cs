using System.Collections.Generic;
using MediatR;
using Node.Core.Models;

namespace Node.CLI.Models
{
    public class ReplacedChainNotify : INotification
    {
        public ReplacedChainNotify(IEnumerable<Block> blocks)
        {
            Blocks = blocks;
        }

        public IEnumerable<Block> Blocks { get; }
    }
}