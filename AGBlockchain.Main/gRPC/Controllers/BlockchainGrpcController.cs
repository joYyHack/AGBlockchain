using AGBlockchain.gRPC.Server;
using AGBlockchain.Service.IService;
using AGBlockchain.Structure.Enums;
using AGBlockchain.Structure.Models;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace AGBlockchain.gRPC.Controllers
{
    public class BlockchainGrpcController : Blockchain.BlockchainBase
    {
        private readonly ILogger<BlockchainGrpcController> logger;
        private readonly IMapper mapper;

        private IBlockchainService Blockchain { get; init; }

        public BlockchainGrpcController(IBlockchainService blockchainService, IMapper mapper, ILogger<BlockchainGrpcController> logger)
        {
            Blockchain = blockchainService;

            this.logger = logger;
            this.mapper = mapper;
        }

        public override Task<BlockResponse> GenesisBlock(Empty _, ServerCallContext context)
        {
            var blockModel = GetBlock(BlockType.Genesis);
            return Task.FromResult(new BlockResponse { Block = blockModel });
        }

        public override Task<BlockResponse> LastBlock(Empty _, ServerCallContext context)
        {
            var blockModel = GetBlock(BlockType.Last);
            return Task.FromResult(new BlockResponse { Block = blockModel });
        }

        private BlockModel GetBlock(BlockType blockType)
        {
            var block = Blockchain.BlockService.GetBlock(blockType);
            return mapper.Map<BlockModel>(block);
        }

        public override Task<Response> Send(SendRequest request, ServerCallContext context)
        {
            string message = string.Empty;

            var balance = Blockchain.TransactionService.GetBalance(request.TransactionInput.Sender);

            var balanceIsOk = balance > (request.TransactionOutput.Amount + request.TransactionOutput.Fee);
            if (!balanceIsOk) message += "Insufficient balance!\n";

            var signatureIsValid = Blockchain.TransactionService
                .VerifySignature(request.PublicKey, request.TransactionId, request.TransactionInput.Signature);
            if (!signatureIsValid) message += "Signature is not!\n";


            var newTransaction = mapper.Map<Transaction>(request);

            var hashIsValid = Blockchain.TransactionService.GetHash(newTransaction).Equals(request.TransactionId);
            if (!hashIsValid) message += "Hash is not valid!\n";

            var everythingIsOk = balanceIsOk && signatureIsValid && hashIsValid;
            if (everythingIsOk)
                Blockchain.MemPoolService.AddNewTransactions(newTransaction);

            return Task.FromResult(new Response { IsSuccessful = everythingIsOk, Message = message }); ;
        }

        public override Task<TransactionsResponse> GetTransactions(AccountRequest request, ServerCallContext context)
        {
            var transactions = Blockchain.TransactionService.GetTransactions(request.Address);
            var transactionModels = mapper.Map<IEnumerable<TransactionModel>>(transactions);

            var transactionsReponse = new TransactionsResponse();
            transactionsReponse.Transactions.Add(transactionModels);

            return Task.FromResult(transactionsReponse);
        }

        public override Task<BalanceResponse> GetBalance(AccountRequest request, ServerCallContext context)
        {
            var balance = Blockchain.TransactionService.GetBalance(request.Address);
            return Task.FromResult(new BalanceResponse { Amount = balance });
        }

        public override Task<BlocksResponse> GetBlocks(BlockRequest request, ServerCallContext context)
        {
            var blocks = Blockchain.BlockService.GetBlocks();
            var blockModels = mapper.Map<List<Block>, List<BlockModel>>(blocks);

            var blocksReponse = new BlocksResponse();
            blocksReponse.Blocks.Add(blockModels);

            return Task.FromResult(blocksReponse);
        }
    }
}