namespace Node.Core.Validators.Block
{
    public interface IBlockValidator
    {
        bool ValidateBlock(Models.Block block, Models.Block previousBlock);
    }
}