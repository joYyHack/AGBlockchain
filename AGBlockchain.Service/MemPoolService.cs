using AGBlockchain.Service.IService;
using AGBlockchain.Structure.Models;
using AGBlockchain.Structure.UnitOfWork;

namespace AGBlockchain.Service
{
    public class MemPoolService : IMemPoolService
    {
        private readonly IUnitOfWork repo;

        public MemPoolService(IUnitOfWork repo) => this.repo = repo;

        public void AddNewTransactions(params Transaction[] txs) => repo.MemPoolRepo.AddTransactions(txs);

        public void ClearPoolOfMinedTransactions(List<Transaction> minedTxs) => repo.MemPoolRepo.ClearPool(minedTxs);

        public List<Transaction> GetUncomfirmedTransactions() => repo.MemPoolRepo.GetUncomfirmedTransactions();
    }
}
