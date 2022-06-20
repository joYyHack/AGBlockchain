using LiteDB;

namespace AGBlockchain.Structure.DB
{
    public interface IBlockchainDbContext
    {
        public LiteDatabase DB { get; }
        public BlockchainDbTableOptions DBTableOptions { get; }
        public void ClearDB();
    }
}
