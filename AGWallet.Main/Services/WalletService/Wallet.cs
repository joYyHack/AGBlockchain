using AGWallet.Main.Services.AccountService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AGBlockchain.gRPC.Client.Blockchain;

namespace AGWallet.Main.Services.WalletService
{
    public class Wallet
    {
        public BlockchainClient Blockchain { get; init; }
        public Account Account { get; private set; }
        public Wallet(BlockchainClient blockchain)
        {
            Blockchain = blockchain;
        }

        public Account CreateAccount()
        {
            Account = Account.Create();
            return Account;
        }

        public Account Login(string secret)
        {
            Account = Account.Login(secret);
            return Account;
        }

        public void ExitFromAccount()
        {
            Account = null; 
        }
    }
}
