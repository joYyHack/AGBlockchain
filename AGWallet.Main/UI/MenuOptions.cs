using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGWallet.Main.UI
{
    public enum MenuOptions
    {
        [Description("Create Account")]
        CreateAccount = 0,
        [Description("Login")]
        Login = 1,
        [Description("Send Money")]
        SendMoney = 2,
        [Description("Check Balance")]
        CheckBalance = 3,
        [Description("Transaction History")]
        TransactionHistory = 4,
        [Description("Exit")]
        Exit = 5
    }

    public enum MenuType
    {
        Initial = 1,
        Account = 2
    }
}
