using AGBlockchain.Service.IService;
using Coravel.Invocable;

namespace AGBlockchain.Main.Jobs
{
    public class BlockMiningJob : IInvocable
    {
        private readonly IBlockchainService blockchainService;
        public static bool BlockIsMined { get; private set; }
        public BlockMiningJob(IBlockchainService blockchainService)
        {
            this.blockchainService = blockchainService;
            BlockIsMined = false;
        }

        public Task Invoke()
        {
            BlockIsMined = blockchainService.MineBlock();

            return Task.FromResult(BlockIsMined);
        }
    }
}
