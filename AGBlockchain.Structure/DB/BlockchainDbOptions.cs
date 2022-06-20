namespace AGBlockchain.Structure.DB
{
    public class BlockchainDbConnectionOptions
    {
        public string DbName { get; set; }
        public string ConnectionString { get; set; }
    }

    public class BlockchainDbTableOptions
    {
        public string BlocksTableName { get; set; }
        public string MemPoolTableName { get; set; }
        public string TransactionsTableName { get; set; }
    }
}
