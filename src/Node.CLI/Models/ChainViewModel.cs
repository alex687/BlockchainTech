using System.Collections.Generic;
using MediatR;
using Node.Core.Models;

namespace Node.CLI.Models
{
    public class ChainViewModel : INotification
    {
        public IEnumerable<Block> Blocks { get; set; }
    }
}