using AGBlockchain.Structure.Models;

namespace AGBlockchain.Service.IService
{
    public interface IBlockchainService
    {
        IBlockService BlockService { get; init; }
        ITransactionService TransactionService { get; init; }
        IMemPoolService MemPoolService { get; init; }
        void CreateGenesisBlock();
        void DropMoneyForICOAccounts();
        bool MineBlock();
    }
}
