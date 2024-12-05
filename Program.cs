using BankConsoleApp.DAL;
using BankConsoleApp.Entities;
using BankConsoleApp.Framework;
using BankConsoleApp.Interfaces.Service_Interface;
using BankConsoleApp.Services;
using Sharprompt;
using Figgle;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;
using Azure;
using System.Transactions;

BankDbContext bankdbcontext = new BankDbContext();
ITransactionService _transactionService = new TransactionService(bankdbcontext);
ICardService _cardService = new CardService(bankdbcontext);
Card CurrentCard = new Card();
Result IsLogin;

string rootPath = "C:\\Users\\mahdi\\source\\repos\\BankConsoleApp";
string filePath = Path.Combine(rootPath, "TransactionCode.txt");
FirstScreen();

#region Login Menu
void login()
{
    do
    {
        Console.Clear();
        var welcomeBox = new Panel("Welcome to [green bold]MJ Bank system[/]");
        welcomeBox.Border = BoxBorder.Square;
        AnsiConsole.Write(welcomeBox);

        


        var cardNumber = AnsiConsole.Ask<string>("[green]>>[/] Card Number: ");


        //var password = AnsiConsole.Ask<string>("[green]>>[/] Password: ");
        AnsiConsole.Markup("[green]>>[/]");
        var password = Sharprompt.Prompt.Password("password");
        
        Loading();

        IsLogin = _cardService.IsPasswordValid(cardNumber, password);


        var panel = new Panel(IsLogin.Message);
        panel.Border = BoxBorder.Square;
        AnsiConsole.Write(panel);

        Console.ReadKey();

        if (IsLogin.IsSuccess)
        {
            Menu();
        }

    }
    while (!IsLogin.IsSuccess);
}
#endregion

#region Main Menu
void Menu()
{
    Console.Clear();

    var option = Sharprompt.Prompt.Select("Choose an option: ", new[] { "1. Transfer Money",
        "2. View my last Transaction", "3. View my Balance",
    "4. Change Password " , "5.Logout"});



    switch (option.Substring(0, 1))
    {
        #region Transfer Money
        case "1":
            Console.Clear();
            var transferBox = new Panel("[blue bold]Transfer Money Page[/]");
            transferBox.Border = BoxBorder.Double;
            AnsiConsole.Write(transferBox);


            var desid = Sharprompt.Prompt.Input<string>(
             "Enter Destination Id card number: ",
             validators: new[]
             {
                Sharprompt.Validators.Required(),
                Sharprompt.Validators.MinLength(16),
                Sharprompt.Validators.MaxLength(16)

             });


            double amount = Sharprompt.Prompt.Input<double>(
             "Enter Amount (based on $) ",
             validators: new[]
             {
                Sharprompt.Validators.Required(),
                Sharprompt.Validators.MinLength(1),


             });



            if (amount < InMemoryDB.CurrentCard.Balance && _cardService.GetByCardNumber(desid) != null)
            {
                var destinationdetail = new Panel($"[green bold]Owner: [/]{_cardService.GetByCardNumber(desid).HolderName}\n" +
                    $"[green bold]Card number: [/]{_cardService.GetByCardNumber(desid).CardNumber}");
                destinationdetail.Border = BoxBorder.Double;
                AnsiConsole.Write(destinationdetail);
                var confirm = Sharprompt.Prompt.Confirm("Do you Confirm this information?");
                if (confirm)
                {

                    var result = Verification();
                    var confirmcode = AnsiConsole.Ask<string>("Enter the Verification Code: [green](Open text file)[/]");
             
                    if (result == confirmcode)
                    {
                        _transactionService.Transfer(InMemoryDB.CurrentCard.CardNumber, desid, amount);
                        TransferAnimation();
                        AnsiConsole.Markup("[bold italic green]transfer has been successful![/]");
                        Console.ReadKey();
                        Menu();
                    }
                    else
                    {

                        var VerificationEror = new Panel("[bold italic red]Verification Code is Wrong![/]");
                        VerificationEror.Border = BoxBorder.Double;
                        AnsiConsole.Write(VerificationEror);

                        Console.ReadKey();
                        Menu();
                    }
                }
                else
                {
                    Menu();
                }
                break;


            }
            else
            {
                
                AnsiConsole.Markup("[bold italic red]Your Balance is not Enough![/]");
                Console.ReadKey();
                break;
            }
            break;
        #endregion

        #region Last Transactions
        case "2":
            Console.Clear();
            var transactions = _transactionService.GetById(InMemoryDB.CurrentCard.Id);

            var transactionBox = new Panel($"Last transactions for [bold green]{InMemoryDB.CurrentCard.HolderName}: [/]");
            transactionBox.Border = BoxBorder.Rounded;
            AnsiConsole.Write(transactionBox);


            var table = new Table();
            //table.Border()
            //we can change border here...
            table.AddColumn("[blue bold]Transaction Id[/]");
            table.AddColumn("[blue bold]Source CardNumber[/]");
            table.AddColumn("[blue bold]From[/]");
            table.AddColumn("[blue bold]Destination CardNumber[/]");
            table.AddColumn("[blue bold]To[/]");
            table.AddColumn("[blue bold]Amount[/]");
            table.AddColumn("[blue bold]Transaction Date[/]");
            table.Caption = new TableTitle("[italic]List Of All Incomes and Outcomes[/]");

            foreach (var item in transactions)
            {
                string SourceName = _cardService.GetByCardNumber(item.SourceCardNumber).HolderName;
                string DestinationName = _cardService.GetByCardNumber(item.DestinationCardNumber).HolderName;

                table.AddRow(Convert.ToString(item.TransactionId), item.SourceCardNumber, SourceName,
                    item.DestinationCardNumber,DestinationName, Convert.ToString(item.Amount), Convert.ToString(item.TransactionDate));


            };




            AnsiConsole.Write(table);

            Console.ReadKey();
            Menu();
            break;
        #endregion

        #region View Balance
        case "3":
            Console.Clear();

            var panel = new Panel
                ($"dear [bold italic green]{InMemoryDB.CurrentCard.HolderName}[/], your balance is [bold underline green]{InMemoryDB.CurrentCard.Balance}[/]");
            panel.Border = BoxBorder.Square;
            AnsiConsole.Write(panel);

            AnsiConsole.Markup(" Press Enter to [bold green]Back[/]!");
            Console.ReadKey();
            Menu();
            break;
        #endregion

        #region Change Password
        case "4":
            Console.Clear();

            var oldpass = AnsiConsole.Ask<string>("[green]>>[/] Enter [red]Old[/] Password:  ");
            var newpass = AnsiConsole.Ask<string>("[green]>>[/] Enter [green]new[/] Password:  ");
            var ChangePassresult = _cardService.ChangePassword(InMemoryDB.CurrentCard.CardNumber, oldpass, newpass);
            if (ChangePassresult)
            {
                var ChangePasswordConfirm = new Panel("[bold italic green] Password Changed Successfully! [/]");
                ChangePasswordConfirm.Border = BoxBorder.Double;
                AnsiConsole.Write(ChangePasswordConfirm);


                Console.ReadKey();
                Menu();
            }
            else
            {
                var ChangePasswordEror = new Panel("[bold italic red]Old Pass is wrong![/]");
                ChangePasswordEror.Border = BoxBorder.Double;
                AnsiConsole.Write(ChangePasswordEror);
                Console.ReadKey();
                Menu();
            }

            break;
        #endregion

        #region Logout
        case "5":
            InMemoryDB.CurrentCard = null;

            FirstScreen();
            break;
            #endregion
    }
}
#endregion



#region Other Methods
void FirstScreen()
{
    Console.Clear();
    Console.WriteLine("\n\n\n\n\n\n\n\n");
    AnsiConsole.Write(
          new Markup(FiggleFonts.Big.Render("Welcome  To  MJ  Bank  !"))
          .Centered());


    AnsiConsole.Markup("                                                " +
        " Press Enter to [bold green]Start[/]!");


    Console.ReadKey();
    login();
    return;
}

void Loading()
{
    AnsiConsole.Status()
                .Start("Processing...", ctx =>
                {
                    System.Threading.Thread.Sleep(1000);
                    ctx.Status("Logging in...");
                    System.Threading.Thread.Sleep(1000);

                });
}

void TransferAnimation()
{
    AnsiConsole.Progress()
           .Start(ctx =>
           {
               var task = ctx.AddTask("[green]Processing...[/]");
               while (!task.IsFinished)
               {
                   task.Increment(10);
                   System.Threading.Thread.Sleep(250);
               }
           });
}

string Verification()
{
    Random random = new Random();
    string randomNumber = Convert.ToString(random.Next(1000, 10000));
    File.WriteAllText(filePath, randomNumber.ToString());
    return randomNumber;
    
}
#endregion