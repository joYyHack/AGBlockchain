namespace AGBlockchain.Structure.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Signature { get; set; }
        public long Timestamp { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }

    }
}
