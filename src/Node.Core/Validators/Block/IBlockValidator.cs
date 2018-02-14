namespace Node.Core.Validators.Block
{
    public interface IBlockValidator
    {
        bool Validate(Models.Block block, Models.Block previousBlock);
    }
}