using AGBlockchain.Structure.Enums;
using AGBlockchain.Structure.Models;

namespace AGBlockchain.Service.IService
{
    public interface IBlockService
    {
        List<Block> GetBlocks();
        Block GetBlock(int blockNumber);
        Block GetBlock(BlockType blockType);
        Block CreateBlock(Block block);
        Block CreateBlock(Block previousBlock, string hash, List<Transaction> transactions);
        Block CreateBlock(int height, string previousHash, string hash, List<Transaction> transactions);
    }
}
