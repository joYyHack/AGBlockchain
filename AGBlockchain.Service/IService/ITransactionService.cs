using AGBlockchain.Structure.Models;

namespace AGBlockchain.Service.IService
{
    public interface ITransactionService
    {
        List<Transaction> GetTransactions();
        List<Transaction> GetTransactions(string address);
        double GetBalance(string address);
        void AddNewTransactions(params Transaction[] txs);
        bool VerifySignature(string publicKeyStr, string message, string signature);
        string GetHash(Transaction tx);
    }
}