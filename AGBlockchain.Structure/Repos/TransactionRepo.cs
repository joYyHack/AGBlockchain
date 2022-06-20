using AGBlockchain.Structure.DB;
using AGBlockchain.Structure.Models;
using AGBlockchain.Structure.Services.IRepo;
using LiteDB;

namespace AGBlockchain.Structure.Services
{
    public class TransactionRepo : ITransactionRepo
    {
        private readonly IBlockchainDbContext ctx;
        public TransactionRepo(IBlockchainDbContext dbContext) => ctx = dbContext;

        public void AddNewTransactions(params Transaction[] txs) => ctx.DB.GetCollection<Transaction>(ctx.DBTableOptions.TransactionsTableName).Insert(txs);

        public List<Transaction> GetTransactions() => ctx.DB.GetCollection<Transaction>(ctx.DBTableOptions.TransactionsTableName).FindAll().ToList();

        public List<Transaction> GetTransactions(string address) => ctx.DB.GetCollection<Transaction>(ctx.DBTableOptions.TransactionsTableName)
                                                                        .Find(tx => address == tx.Sender || address == tx.Recipient)
                                                                        .OrderBy(tx => tx.Timestamp)
                                                                        .ToList();
    }
}
