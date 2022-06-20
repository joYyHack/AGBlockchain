using AGBlockchain.Structure.DB;
using AGBlockchain.Structure.Enums;
using AGBlockchain.Structure.Models;
using AGBlockchain.Structure.Services.IRepo;
using LiteDB;

namespace AGBlockchain.Structure.Services
{
    public class BlockRepo : IBlockRepo
    {
        private readonly IBlockchainDbContext ctx;

        public BlockRepo(IBlockchainDbContext dbContext) => ctx = dbContext;

        public List<Block> GetBlocks() => ctx.DB.GetCollection<Block>(ctx.DBTableOptions.BlocksTableName).FindAll().ToList();

        public Block GetBlock(int blockNumber) => ctx.DB.GetCollection<Block>(ctx.DBTableOptions.BlocksTableName).FindOne(bl => bl.Height == blockNumber);

        public Block GetBlock(BlockType blockType)
        {
            int order = Query.Ascending;

            if (blockType == BlockType.Genesis)
                order = Query.Ascending;
            else if (blockType == BlockType.Last)
                order = Query.Descending;

            var blockchain = ctx.DB.GetCollection<Block>(ctx.DBTableOptions.BlocksTableName);
            var lastBlock = blockchain.FindOne(Query.All(order));
            return lastBlock;
        }

        public Block CreateBlock(Block block)
        {
            var blockId = ctx.DB.GetCollection<Block>(ctx.DBTableOptions.BlocksTableName).Insert(block);
            return ctx.DB.GetCollection<Block>(ctx.DBTableOptions.BlocksTableName).FindById(blockId);
        }
    }
}
