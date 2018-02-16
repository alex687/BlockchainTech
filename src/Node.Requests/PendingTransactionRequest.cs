namespace Node.Requests
{
    public class PendingTransactionRequest
    {
        public string Hash { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Amount { get; set; }

        public string SenderPublickKey { get; set; }

        public string SenderSignature { get; set; }
    }
}
