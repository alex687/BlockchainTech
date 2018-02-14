namespace Node.Core.Validators.Block
{
    public interface IBlockValidator
    {
        bool ValidateB(Models.Block block, Models.Block previousBlock);
    }
}