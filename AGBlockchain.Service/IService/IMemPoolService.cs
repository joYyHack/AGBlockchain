using AGBlockchain.Structure.Models;

namespace AGBlockchain.Service.IService
{
    public interface IMemPoolService
    {
        List<Transaction> GetUncomfirmedTransactions();
        void AddNewTransactions(params Transaction[] txs);
        void ClearPoolOfMinedTransactions(List<Transaction> minedTxs);
    }
}
