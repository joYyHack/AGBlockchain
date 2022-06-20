using AGBlockchain.Structure.DB;
using AGBlockchain.Structure.Repo;
using AGBlockchain.Structure.Repo.IRepo;
using AGBlockchain.Structure.Services;
using AGBlockchain.Structure.Services.IRepo;

namespace AGBlockchain.Structure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IBlockchainDbContext dbContext;
        private IBlockRepo blockRepo;
        private IMemPoolRepo memPoolRepo;
        private ITransactionRepo transactionRepo;
        private bool disposedValue;

        public UnitOfWork(IBlockchainDbContext dbContext) => this.dbContext = dbContext;

        public IBlockRepo BlockRepo => blockRepo ??= new BlockRepo(dbContext);
        public IMemPoolRepo MemPoolRepo => memPoolRepo ??= new MemPoolRepo(dbContext);
        public ITransactionRepo TransactionRepo => transactionRepo ??= new TransactionRepo(dbContext);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    dbContext.DB.Dispose();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
