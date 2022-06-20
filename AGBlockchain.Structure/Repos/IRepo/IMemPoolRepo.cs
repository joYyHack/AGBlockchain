using AGBlockchain.Structure.Models;

namespace AGBlockchain.Structure.Repo.IRepo
{
    public interface IMemPoolRepo
    {
        List<Transaction> GetUncomfirmedTransactions();
        void AddTransactions(params Transaction[] txs);
        void ClearPool(List<Transaction> minedTxs);
    }
}
