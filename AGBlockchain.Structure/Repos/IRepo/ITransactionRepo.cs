using AGBlockchain.Structure.Models;

namespace AGBlockchain.Structure.Services.IRepo
{
    public interface ITransactionRepo
    {
        List<Transaction> GetTransactions();
        List<Transaction> GetTransactions(string address);
        void AddNewTransactions(params Transaction[] txs);
    }
}
