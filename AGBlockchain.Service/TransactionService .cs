using AGBlockchain.Service.IService;
using AGBlockchain.Structure.Models;
using AGBlockchain.Structure.UnitOfWork;
using EllipticCurve;
using Static.Helpers;

namespace AGBlockchain.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork repo;

        public TransactionService(IUnitOfWork repo) => this.repo = repo;

        public void AddNewTransactions(params Transaction[] txs) => repo.TransactionRepo.AddNewTransactions(txs);

        public string GetHash(Transaction tx)
        {
            var data = string.Join(string.Empty, tx.Timestamp, tx.Sender, tx.Amount, tx.Fee, tx.Recipient);
            return Utils.GenerateHash(data);
        }

        public List<Transaction> GetTransactions() => repo.TransactionRepo.GetTransactions();

        public List<Transaction> GetTransactions(string address) => repo.TransactionRepo.GetTransactions(address);

        public double GetBalance(string address)
        {
            var transactions = GetTransactions(address);
            var credit = transactions.Where(tr => tr.Sender.Equals(address)).Sum(tr => tr.Amount);
            var debit = transactions.Where(tr => tr.Recipient.Equals(address)).Sum(tr => tr.Amount);

            return debit - credit;
        }

        public bool VerifySignature(string publicKeyStr, string message, string signature)
        {
            var publicKeyBytes = Convert.FromHexString(publicKeyStr);
            var publicKey = PublicKey.fromString(publicKeyBytes);
            return Ecdsa.verify(message, Signature.fromBase64(signature), publicKey);
        }
    }
}
