using AGBlockchain.Structure.Enums;
using AGBlockchain.Structure.Models;

namespace AGBlockchain.Structure.Services.IRepo
{
    public interface IBlockRepo
    {
        List<Block> GetBlocks();
        Block GetBlock(int blockNumber);
        Block GetBlock(BlockType blockType);
        Block CreateBlock(Block block);
    }
}
