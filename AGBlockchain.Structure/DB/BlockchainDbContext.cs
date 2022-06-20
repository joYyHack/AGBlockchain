using AGBlockchain.Structure.Models;
using LiteDB;
using Microsoft.Extensions.Options;

namespace AGBlockchain.Structure.DB
{
    public class BlockchainDbContext : IBlockchainDbContext
    {
        public LiteDatabase DB { get; }
        public BlockchainDbTableOptions DBTableOptions { get; }

        public BlockchainDbContext(IOptions<BlockchainDbConnectionOptions> dbOptions, IOptions<BlockchainDbTableOptions> tableOptions)
        {
            DB = new($"{dbOptions.Value.ConnectionString}{dbOptions.Value.DbName}");
            DBTableOptions = tableOptions.Value;

            ClearDB();
            Configure();
            Initialize();
        }
       
        private void Initialize()
        {
            var blocks = DB.GetCollection<Block>(DBTableOptions.BlocksTableName);
            blocks.EnsureIndex(bl => bl.Height, unique: true);

            DB.GetCollection<MemPool>(DBTableOptions.MemPoolTableName);

            var transactions = DB.GetCollection<Transaction>(DBTableOptions.TransactionsTableName);
            transactions.EnsureIndex(tx => tx.Sender);
            transactions.EnsureIndex(tx => tx.Recipient);
        }

        private static void Configure()
        {
            BsonMapper.Global.EmptyStringToNull = false;
        }

        public void ClearDB()
        {
            if (DB is not null)
            {
                DB.DropCollection(DBTableOptions.BlocksTableName);
                DB.DropCollection(DBTableOptions.TransactionsTableName);
                DB.DropCollection(DBTableOptions.MemPoolTableName);
            }
        }
    }
}
