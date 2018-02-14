using MediatR;
using Node.Core.Models;

namespace Node.CLI.Models
{
    public class BlockNotify : INotification
    {
        public BlockNotify(Block block)
        {
            Block = block;
        }

        public Block Block { get; }
    }
}