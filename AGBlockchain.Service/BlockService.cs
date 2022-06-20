using AGBlockchain.Service.IService;
using AGBlockchain.Structure.Enums;
using AGBlockchain.Structure.Models;
using AGBlockchain.Structure.UnitOfWork;
using Newtonsoft.Json;
using Static.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace AGBlockchain.Service
{
    public class BlockService : IBlockService
    {
        private readonly IUnitOfWork repo;
        public BlockService(IUnitOfWork repo) => this.repo = repo;

        public Block CreateBlock(Block previousBlock, string hash, List<Transaction> transactions) =>
            CreateBlock(previousBlock.Height + 1, previousBlock.Hash, hash, transactions);

        public Block CreateBlock(int height, string previousHash, string hash, List<Transaction> transactions)
        {

            var newBlock = new Block
            {
                Height = height,
                TimeStamp = DateTime.UtcNow.Ticks,
                Hash = hash,
                PreviousHash = previousHash,
                Transactions = transactions
            };

            return CreateBlock(newBlock);
        }

        public Block CreateBlock(Block block)
        {
            var newBlock = repo.BlockRepo.CreateBlock(block);
            repo.TransactionRepo.AddNewTransactions(block.Transactions.ToArray());

            return newBlock;
        }

        public Block GetBlock(int blockNumber) => repo.BlockRepo.GetBlock(blockNumber);

        public Block GetBlock(BlockType blockType) => repo.BlockRepo.GetBlock(blockType);

        public List<Block> GetBlocks() => repo.BlockRepo.GetBlocks();
    }
}