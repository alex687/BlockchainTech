namespace Minner
{
    public class Acceptance
    {
        private readonly string _criatiria;

        public Acceptance(int difficulty)
        {
            _criatiria = new string('0', difficulty);
        }

        public bool IsValid(string hash)
        {
            return hash.StartsWith(_criatiria);
        }
    }
}