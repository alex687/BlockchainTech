namespace Node.Core.Validators.Block
{
    public class PassingBlockValidator : IBlockValidator
    {
        public bool Validate(Models.Block block, Models.Block previousBlock)
        {
            return true;
        }
    }
}