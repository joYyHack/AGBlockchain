namespace AGBlockchain.Structure.Models
{
    public class Block
    {
        //Header
        public int Height { get; set; }
        public long TimeStamp { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        //Body
        public List<Transaction> Transactions { get; set; }
    }
}
