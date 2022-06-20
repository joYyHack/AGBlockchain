using AGBlockchain.Structure.DB;
using AGBlockchain.Structure.Models;
using AGBlockchain.Structure.Repo.IRepo;

namespace AGBlockchain.Structure.Repo
{
    public class MemPoolRepo : IMemPoolRepo
    {
        protected readonly IBlockchainDbContext ctx;

        public MemPoolRepo(IBlockchainDbContext dbContext) => ctx = dbContext;

        public List<Transaction> GetUncomfirmedTransactions() => ctx.DB.GetCollection<Transaction>(ctx.DBTableOptions.MemPoolTableName).FindAll().ToList();

        public void AddTransactions(params Transaction[] txs) => ctx.DB.GetCollection<Transaction>(ctx.DBTableOptions.MemPoolTableName).Insert(txs);

        public void ClearPool(List<Transaction> minedTxs) => ctx.DB.GetCollection<Transaction>(ctx.DBTableOptions.MemPoolTableName).DeleteMany(tx => minedTxs.Select(txToDelete => tx.Id).Contains(tx.Id));
    }
}
