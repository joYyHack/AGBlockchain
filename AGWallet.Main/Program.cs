using AGBlockchain.gRPC.Client;
using AGWallet.Main.Services.WalletService;
using AGWallet.Main.UI;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Newtonsoft.Json;

var serverAddress = "https://localhost:7162";
using var grpcChannel = GrpcChannel.ForAddress(serverAddress);

var blockchainClient = new Blockchain.BlockchainClient(grpcChannel);
 
var wallet = new Wallet(blockchainClient);
var menu = new Menu(wallet);

menu.GenerateMenuAndWaitForResult();

Console.ReadKey();