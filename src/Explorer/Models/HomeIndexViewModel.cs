using System.Collections.Generic;
using Node.Core.Models;

namespace Explorer.Models
{
    public class HomeIndexViewModel
    {
        public List<Block> Blocks { get; set; }

        public List<PendingTransaction> PendingTransactions { get; set; }
    }
}
