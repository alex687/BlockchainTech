using MediatR;
using Node.Core.Models;

namespace Node.CLI.Models
{
    public class AddedBlockToChainNotify : INotification
    {
        public AddedBlockToChainNotify(Block block)
        {
            Block = block;
        }

        public Block Block { get; }
    }
}