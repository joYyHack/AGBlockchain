using AGBlockchain.gRPC.Controllers;
using AGBlockchain.Main.Jobs;
using AGBlockchain.Service;
using AGBlockchain.Service.IService;
using AGBlockchain.Structure.DB;
using AGBlockchain.Structure.UnitOfWork;
using Coravel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();

builder.Services.Configure<BlockchainDbConnectionOptions>(builder.Configuration.GetSection(nameof(BlockchainDbConnectionOptions)));
builder.Services.Configure<BlockchainDbTableOptions>(builder.Configuration.GetSection(nameof(BlockchainDbTableOptions)));

builder.Services
    .AddSingleton<IBlockchainDbContext, BlockchainDbContext>()
    .AddSingleton<IUnitOfWork, UnitOfWork>()
    .AddSingleton<ITransactionService, TransactionService>()
    .AddSingleton<IMemPoolService, MemPoolService>()
    .AddSingleton<IBlockService, BlockService>()
    .AddSingleton<IBlockchainService, BlockchainService>();

builder.Services.AddScheduler();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

StartMiningAsync(app);

app.MapGrpcService<BlockchainGrpcController>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

static async Task StartMiningAsync(WebApplication app)
{
    var blockchain = app.Services.GetService<IBlockchainService>();
    await Task.Run(() =>
    {
        while(true) 
            blockchain.MineBlock();
    });   
}