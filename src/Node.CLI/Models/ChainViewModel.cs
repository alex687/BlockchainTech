using System.Collections.Generic;
using MediatR;

namespace Node.CLI.Models
{
    public class ChainViewModel : INotification
    {
        public IEnumerable<BlockViewModel> Blocks { get; set; }
    }
}