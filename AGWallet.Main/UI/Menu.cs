using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGWallet.Main.Services.WalletService;
using Spectre.Console;
using EnumsNET;
using AGBlockchain.gRPC.Client;
using Static.Helpers;

namespace AGWallet.Main.UI
{
    public class Menu
    {
        Wallet Wallet { get; init; }
        public Menu(Wallet wallet)
        {
            Wallet = wallet;
        }

        private void Return()
        {
            AnsiConsole.WriteLine("Enter any button to return to main menu!");
            Console.ReadLine();
            GenerateMenuAndWaitForResult();
        }

        public void GenerateMenuAndWaitForResult()
        {
            var menuType = Wallet.Account is not null ? MenuType.Account : MenuType.Initial;
            GenerateMenuAndWaitForResult(menuType);
        }

        private void GenerateMenuAndWaitForResult(MenuType menuType)
        {
            var showFullMenu = Enums.Equals(MenuType.Account, menuType);

            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Wallet").Centered().Color(Color.Aqua));
            AnsiConsole.Write(new Rule());

            if (showFullMenu)
            {
                var rule = new Rule();
                rule.RuleStyle(new Style(Color.Grey15, Color.Default));

                DisplayAccountInfo("Private key", Wallet.Account.PrivateKeyStr);
                DisplayAccountInfo("Public key", Wallet.Account.PublicKeyStr);
                DisplayAccountInfo("Address", Wallet.Account.Address);
                DisplayAccountInfo("Secret", Wallet.Account.SecretNumber.ToString());

                AnsiConsole.Write(new Rule());

                void DisplayAccountInfo(string ruleTitile, string infoName)
                {
                    rule.RuleTitle($"[Green]{ruleTitile}[/]");
                    AnsiConsole.Write(rule);
                    AnsiConsole.Write(new Markup($"[White]{infoName}[/]{Environment.NewLine}").Centered().Fold());
                }
            }

            var chosenOption = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .AddChoices(
                                showFullMenu
                                ? new[] { MenuOptions.SendMoney.AsString(EnumFormat.Description),
                                          MenuOptions.CheckBalance.AsString(EnumFormat.Description),
                                          MenuOptions.TransactionHistory.AsString(EnumFormat.Description),
                                          MenuOptions.Exit.AsString(EnumFormat.Description)}

                                : new[] { MenuOptions.CreateAccount.AsString(EnumFormat.Description),
                                          MenuOptions.Login.AsString(EnumFormat.Description),
                                          MenuOptions.Exit.AsString(EnumFormat.Description)}
                                ));

            var menuOption = Enums.GetMember<MenuOptions>(chosenOption, false, EnumFormat.Description);
            CallFunction(menuOption.Value);

        }

        private void CallFunction(MenuOptions menuOption)
        {
            switch (menuOption)
            {
                case MenuOptions.CreateAccount:
                    CreateAccountAndDisplay();
                    break;
                case MenuOptions.Login:
                    DisplayAction("Login");
                    LoginToAccountAndDisplay();
                    break;
                case MenuOptions.SendMoney:
                    DisplayAction("Create Transaction");
                    SendMoneyAndDisplay();
                    break;
                case MenuOptions.CheckBalance:
                    DisplayAction("Balance");
                    CheckBalanceAndDisplay();
                    break;
                case MenuOptions.TransactionHistory:
                    DisplayAction("Transaction History");
                    GetTransactionsAndDisplay();
                    break;
                case MenuOptions.Exit:
                    Exit();
                    break;
                default:
                    AnsiConsole.Write("Default...");
                    break;
            }
        }

        private void CreateAccountAndDisplay()
        {
            Wallet.CreateAccount();
            GenerateMenuAndWaitForResult(MenuType.Account);
        }

        private void LoginToAccountAndDisplay()
        {
            var secretNumber = Ask<string>("Secret Number", "Please enter your [green]secret number[/] to login:");

            Wallet.Login(secretNumber);

            AnsiConsole.Markup("[green]Success![/]\n");
            Return();
        }

        private void SendMoneyAndDisplay()
        {
            var recipientAddress = Ask<string>("Recipient", "Please enter the [green]recipient address[/]:");
            var amount = Ask<double>("Amount", "Please enter the [green]amount[/]:");
            var fee = Ask<double>("Fee", "Please enter [green]fee(number)[/]:");

            var balance = Wallet.Blockchain.GetBalance(new AccountRequest { Address = Wallet.Account.Address });

            if (balance.Amount < (amount + fee))
            {
                AnsiConsole.WriteException(new Exception("Sender Has Insufficient Balance"), ExceptionFormats.Default);
                Return();
            }

            var txIn = new TransactionInput
            {
                Timestamp = DateTime.Now.Ticks,
                Sender = Wallet.Account.Address,
            };

            var txOut = new TransactionOutput
            {
                Recipient = recipientAddress,
                Amount = amount,
                Fee = fee
            };

            var txInputOutputStr = string.Join(string.Empty, txIn.Timestamp, txIn.Sender, txOut.Amount, txOut.Fee, txOut.Recipient);
            var transactionHash = Utils.GenerateHash(txInputOutputStr);

            var signature = Wallet.Account.Sign(transactionHash);
            txIn.Signature = signature;

            var request = new SendRequest()
            {
                TransactionId = transactionHash,
                PublicKey = Wallet.Account.PublicKeyStr,
                TransactionInput = txIn,
                TransactionOutput = txOut
            };

            try
            {
                SendTransactionLiveDisplay("Sending", request);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.Default);

            }

            Return();
        }

        private void SendTransactionLiveDisplay(string statusText, SendRequest request)
        {
            AnsiConsole.Status()
                .Start(statusText, ctx =>
                {
                    // Simulate some work
                    ctx.SpinnerStyle(Style.Parse("yellow"));
                    ctx.Spinner(Spinner.Known.SimpleDotsScrolling);

                    var task = Task.Run(() => Wallet.Blockchain.Send(request));

                    while (!task.IsCompleted)
                    {
                        Thread.Sleep(1000);
                        if (task.Result.IsSuccessful)
                        {
                            AnsiConsole.MarkupLine("[green] Transaction was send succesfully! [/]");
                        }
                        else
                        {
                            throw new Exception("Error while processing transaction");
                        }
                    }
                });
        }

        private void CheckBalanceAndDisplay()
        {
            var balance = Wallet.Blockchain.GetBalance(new AccountRequest { Address = Wallet.Account.Address });
            AnsiConsole.MarkupLine($"Your Balance is [green]{balance.Amount}! [/]");
            Return();
        }

        private void GetTransactionsAndDisplay()
        {
            var transactionsResponse = Wallet.Blockchain.GetTransactions(new AccountRequest { Address = Wallet.Account.Address });

            int index = 0;
            transactionsResponse.Transactions.ToList().ForEach(transaction =>
            {

                var table = new Table();
                table.BorderColor(Color.White);
                table.AddColumn(new TableColumn($"[yellow]Transaction No {++index}[/]").Centered());

                table.AddRow($"[green]Timestamp[/]: {new DateTime(transaction.Timestamp)}").AddEmptyRow();
                table.AddRow($"[green]Id[/]: {transaction.Id}").AddEmptyRow();
                table.AddRow($"[green]Sender[/]: {transaction.Sender}").AddEmptyRow();
                table.AddRow($"[green]Recipient[/]: {transaction.Recipient}").AddEmptyRow();
                table.AddRow($"[green]Amount[/]: {(transaction.Recipient.Equals(Wallet.Account.Address) ? "+" : "-")}{transaction.Amount}").AddEmptyRow();
                table.AddRow($"[green]Signature[/]: {transaction.Signature}").AddEmptyRow();

                table.Columns[0].PadLeft(0);
                table.Columns[0].PadRight(0);
                AnsiConsole.Write(table);
            });

            Return();
        }

        private static void DisplayAction(string actionName)
        {
            var table = new Table();
            table.AddColumn($"[bold]{actionName}[/]");
            table.Centered();
            table.Border(TableBorder.Square);
            AnsiConsole.Write(table);
        }

        private static T Ask<T>(string title, string question)
        {
            var rule = new Rule($"[white]{title}[/]");
            rule.Alignment(Justify.Left);
            AnsiConsole.Write(rule);
            return AnsiConsole.Ask<T>($"{question}\n");
        }

        private void Exit()
        {
            Wallet.ExitFromAccount();
            GenerateMenuAndWaitForResult();
        }
    }
}
