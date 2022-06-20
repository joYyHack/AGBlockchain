using AGBlockchain.Service.IService;
using AGBlockchain.Structure.Enums;
using AGBlockchain.Structure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spectre.Console;
using Static.Helpers;

namespace AGBlockchain.Service
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IConfiguration configuration;
        public IBlockService BlockService { get; init; }
        public ITransactionService TransactionService { get; init; }
        public IMemPoolService MemPoolService { get; init; }

        public BlockchainService(IConfiguration configuration, IBlockService blockService,
            ITransactionService transactionService, IMemPoolService memPoolService)
        {
            this.configuration = configuration;

            BlockService = blockService;
            TransactionService = transactionService;
            MemPoolService = memPoolService;

            Initialize();
        }

        void Initialize()
        {
            var blocks = BlockService.GetBlocks().Count;
            if (blocks == 0)
            {
                CreateGenesisBlock();
                DropMoneyForICOAccounts();
            }
        }

        public void CreateGenesisBlock()
        {
            Console.WriteLine("Genesis block generating...");

            #region Genesis Block
            var tx = new Transaction
            {
                Id = string.Empty,
                Signature = string.Empty,
                Amount = 1000,
                Sender = "System",
                Recipient = "Satoshi Nakomoto",
                Fee = 0.001
            };

            var txHash = TransactionService.GetHash(tx);
            tx.Id = txHash;

            var block = new Block
            {
                Height = 1,
                TimeStamp = DateTime.UtcNow.Ticks,
                PreviousHash = string.Empty,
                Transactions = new List<Transaction>() { tx },
            };

            var hash = GenerateHash(block.TimeStamp, block.PreviousHash, block.Transactions, 0);
            block.Hash = hash;

            var genesisBlock = BlockService.CreateBlock(block);
            #endregion

            DisplayBlock(genesisBlock);
            //Console.WriteLine($"Genesis Block is created...Height: {genesisBlock.Height}...Transactions Count: {genesisBlock.Transactions.Count}");
        }

        public bool MineBlock()
        {
            Console.CursorVisible = false;

            AnsiConsole.WriteLine("\nBlock mining...");

            #region Block Mining
            var transactions = MemPoolService.GetUncomfirmedTransactions();
            var lastBlock = BlockService.GetBlock(BlockType.Last);

            string hash = string.Empty;
            ulong nonce = 0;
            AnsiConsole.Write("Hash Searching: ");

            while (!hash.StartsWith(configuration.GetSection("BlockDifficulty").Value))
            {
                if(hash != string.Empty)
                {
                    Console.SetCursorPosition(16, Console.CursorTop);
                    Console.Write(new string(' ', hash.Length));
                    Console.SetCursorPosition(16, Console.CursorTop);
                    
                }
                hash = GenerateHash(DateTime.UtcNow.Ticks, lastBlock.Hash, transactions, nonce++);
                Console.Write(hash);
            }

            var newBlock = BlockService.CreateBlock(lastBlock, hash, transactions);
            MemPoolService.ClearPoolOfMinedTransactions(transactions);
            #endregion 

            Console.WriteLine();
            DisplayBlock(newBlock);
            Console.CursorVisible = true;

            return true;
        }

        private static string GenerateHash(long timeStamp, string previousHash, List<Transaction> transactions, ulong nonce)
        {
            var data = string.Join(string.Empty, timeStamp, previousHash, JsonConvert.SerializeObject(transactions), nonce);
            return Utils.GenerateHash(data);
        }

        public void DropMoneyForICOAccounts()
        {
            ICOAccount.GetICOAccounts().ForEach(acc =>
            {
                #region Send Money For ICO Accounts
                var tx = new Transaction
                {
                    Timestamp = DateTime.Now.Ticks,
                    Signature = string.Empty,
                    Sender = "ICO",
                    Recipient = acc.Address,
                    Amount = acc.Balance,
                    Fee = 0
                };

                var txHash = TransactionService.GetHash(tx);
                tx.Id = txHash;

                MemPoolService.AddNewTransactions(tx);
                #endregion
            });
        }

        private static void DisplayBlock(Block block)
        {
            var table = new Table();
            table.BorderColor(Color.White);
            table.AddColumn(new TableColumn($"[tan]Block No {block.Height}[/]").Centered());

            table.AddRow($"[green]Timestamp[/]: {new DateTime(block.TimeStamp)}").AddEmptyRow();
            table.AddRow($"[green]Hash[/]: [yellow]{block.Hash}[/]").AddEmptyRow();
            table.AddRow($"[green]Previous Hash[/]: [blue]{block.PreviousHash}[/]").AddEmptyRow();
            table.AddRow($"[green]Transactions In Block[/]: {block.Transactions.Count}").AddEmptyRow();

            table.Columns[0].PadLeft(0);
            table.Columns[0].PadRight(0);
            AnsiConsole.Write(table);
        }
    }
}
