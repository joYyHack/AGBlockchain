using AGBlockchain.Structure.Repo.IRepo;
using AGBlockchain.Structure.Services.IRepo;

namespace AGBlockchain.Structure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBlockRepo BlockRepo { get; }
        IMemPoolRepo MemPoolRepo { get; }
        ITransactionRepo TransactionRepo { get; }
    }
}
