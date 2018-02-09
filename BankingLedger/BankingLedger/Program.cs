using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using Liphsoft.Crypto.Argon2; //https://github.com/alipha/csharp-argon2 Search on Liphsoft.Crypto.Argon2 on NuGet

namespace BankingLedger
{
    public class Program
    {
        // UserCreator class allows application to create new users and
        // contains the data of all the users that have been created
        private UserCreator cu;

        // User that is currently logged in to the banking application
        private User loggedInUser;
       
        public UserCreator CU { get => cu; set => cu = value; }
        public User LoggedInUser { get => loggedInUser; set => loggedInUser = value; }

        public Program()
        {            
            cu = new UserCreator();
            loggedInUser = null;
        }

        static void Main(string[] args)
        {
            Program Banking = new Program();

            // Start banking application loop from the Login Menu
            Banking.LoginMenu();
        }

        // MenuSelect()
        // Displays a custom menu and welcome message depending on the menuItems list that is passed as an argument
        public int MenuSelect(string[] menuItems)
        {
            Console.Clear();

            // if user is logged in, display username in welcom message
            if (LoggedInUser == null)
            {
                Console.WriteLine("\nWelcome to Banking!\n");
            }
            else
            {
                Console.WriteLine("\nWelcome to Banking, {0}!\n", LoggedInUser.Username);
            }
            
            // get user input
            Console.WriteLine("What would you like to do?\n");

            Console.WriteLine(String.Join(Environment.NewLine, menuItems));

            Console.WriteLine("\nPlease enter the number of your selection: ");

            // parse user input string to int
            int selection = GetInputAsInt();

            // check if selection is valid, if valid return int selection, argumentException otherwise
            if (selection < 1 || selection > menuItems.Length)
            {
                Console.Clear();
                throw new ArgumentException("Invalid Selection");
            }

            return selection;
        }

        // LoginMenu() - Logic for the Login Menu
        // first menu of application, users can log in or create a new user
        // continues to main menu after completing either option
        public void LoginMenu()
        {
            // LoginMenuItems will be passed to MenuSelect() to render menu and get user selection
            var loginMenuItems = new[] { "1. Login", "2. Create New User", "3. Quit" };
            int selection = -1;

            do
            {
                if (selection == -1)
                {
                    try
                    {
                        // render menu and get user selection
                        selection = MenuSelect(loginMenuItems);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        selection = -1;
                    }
                }
                
                // "Login" option
                if (selection == 1)
                {
                    try
                    {
                        // Once user has successfully logged in, go to Main Menu
                        LoginUser();
                        MainMenu();

                        // After user returns from MainMenu(), go back to looping through Login Menu
                        selection = -1;
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                        selection = -1;
                    }
                }
                // "Create User" option
                else if (selection == 2)
                {
                    try
                    {
                        // Once user has created a new user, go to Main Menu logged in as new user
                        loggedInUser = CU.CreateUser();
                        Console.ReadKey();
                        MainMenu();

                        // After user returns from MainMenu(), go back to looping through Login Menu
                        selection = -1;
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();

                        // After user has seen exception, return to looping through Login Menu
                        selection = -1;
                    }
                }
                // "Quit" option
                else if (selection == 3)
                {
                    continue;
                }
                else
                {
                    selection = -1;
                }

            } while (selection != 3);

            return;
        }


        // MainMen() - Logic for the Main Menu
        // menu available after logging in or creating a new user
        // users can make deposits and withdrawals, check their balance, and view their transaction history
        // returns to login menu if user logs out
        public void MainMenu()
        {
            // mainMenuItems will be passed to MenuSelect() to render menu and get user selection
            var mainMenuItems = new[] { "1. Record Deposit", "2. Record Withdrawal", "3. Check Balance", "4. View Transaction History", "5. Logout" };
            int selection = -1;

            do
            {
                // render menu and get user selection
                selection = MenuSelect(mainMenuItems);

                // "Deposit" selection
                if (selection == 1)
                {
                    try
                    {
                        Deposit();
                        Console.ReadKey();
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                }
                // "Withdrawal" selection
                else if (selection == 2)
                {
                    try
                    {
                        Withdrawal();
                        Console.ReadKey();
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadKey();
                    }
                }
                // "Check Balance" selection
                else if (selection == 3)
                {
                    CheckBalance();
                    Console.ReadKey();
                }
                // "Transaction History" selection
                else if (selection == 4)
                {
                    GetTransactionHistory();
                    Console.ReadKey();
                }
                // "Logout" selection
                else if (selection == 5)
                {
                    LogoutUser();

                    // Users can opt out of logging out
                    // if they do so, they return to the Main Menu
                    // logging out returns to the Login Menu
                    if (LoggedInUser == null)
                    {
                        return;
                    }
                    else
                    {
                        selection = -1;
                    }
                }
                else
                {
                    selection = -1;
                }

            } while (selection != 5);

        }


        // GetInputAsInt()
        // Reads user input and parse as integer
        // returns FormatException if input can't be parsed as int
        // gets menu selections from users
        public int GetInputAsInt()
        {
            string input = Console.ReadLine();
            int iInput;

            // Attempt to format user input as integer
            try
            {
               iInput = Int32.Parse(input);
            }
            catch (FormatException error)
            {
                Console.Clear();
                Console.WriteLine(error.Message);
                return -1;
            }

            return iInput;
        }

        // LoginUser()
        // gets user credentials and attempts to authenticate user from current user list
        // if user is authenticated successfully, user is set to loggedInUser
        public void LoginUser()
        {
            Console.Clear();
            Console.WriteLine("\nPlease enter your Login credentials...\n");

            Console.WriteLine("Username: ");

            string userName = Console.ReadLine();

            Console.WriteLine("Password: ");

            string userPass = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    userPass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && userPass.Length > 0)
                    {
                        userPass = userPass.Substring(0, (userPass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);

            //string userPass = Console.ReadLine();

            // Authenticate and login user
            // if user credentials are bad, throw invalid login exception
            if (!AuthenticateUser(userName, userPass))
            {
                Console.Clear();
                throw new ArgumentException("Invalid Login");
            }
        }

        // LoginUser()
        // receives user credentials and attempts to authenticate user from current user list
        // if user is authenticated successfully, sets user to loggedInUser
        // returns true if login is valid
        public bool AuthenticateUser(string userName, string userPass)
        {
            // find username in list of users
            User QuarantineUser = CU.UserList.Find(x => x.Username == userName);

            var hasher = new PasswordHasher();

            // if username is found in user list
            // attempt to verify credentials
            if (QuarantineUser != null)  
            {
                // verify user password against one-way hash code stored in database
                if (hasher.Verify(QuarantineUser.Password, userPass))
                {
                    // if verified, set QuarantineUser to LoggedInUser
                    LoggedInUser = QuarantineUser;
                    return true;
                }
            }

            return false;
        }

        // LogoutUser()
        // gets user verification on logout
        // if yes, sets LoggedInUser to null
        public void LogoutUser()
        {
            Console.Clear();
            Console.WriteLine("Are you sure you want to logout? Y/N: ");
            ConsoleKeyInfo logoutKey = Console.ReadKey();

            if (logoutKey.Key == ConsoleKey.Y)
            {
                LoggedInUser = null;
            }
            else
            {
                return;
            }
        }

        // Deposit()
        // Allows the LoggedInUser to make a deposit
        public void Deposit()
        {
            // get deposit amount from user
            Console.Clear();
            Console.WriteLine("Please enter how much you would like to deposit in dollars and cents (e.g. 51.50): ");
            string depositString = Console.ReadLine();

            // check if deposit string is null or empty
            if (depositString == null || depositString == "" || depositString == " ")
            {
                throw new ArgumentException("Deposit amount cannot be empty");
            }
            // check that deposit string only contains numbers and the decimal point
            else if (Regex.IsMatch(depositString, @"[^0-9.]+"))
            {
                throw new ArgumentException("Deposit amount must be a decimal number of dollars and cents");
            }

            // convert deposit string to double
            double depositAmount = Convert.ToDouble(depositString);

            // check that deposit string has a decimal point, if so, check that there are only two digits after the decimal point
            if (Regex.IsMatch(depositString, @"[.]+") && depositString.Substring(depositString.IndexOf(".")).Length > 3)
            {
                throw new ArgumentException("Deposit amount can only be specified to two decimal places");
            }
            // check that deposit string is greater than zero
            else if (depositAmount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero");
            }

            double originalBalance = LoggedInUser.Balance;

            // add deposit to current balance
            LoggedInUser.Balance += depositAmount;

            // add deposit to user's transaction history
            LoggedInUser.AddHistory(string.Format("{0} Deposit: {1:C2}, Previous Balance: {3:C2}, New Balance: {2:C2}\n", DateTime.Now, 
                depositAmount, 
                LoggedInUser.Balance, 
                originalBalance));

            // display new balance
            Console.WriteLine(String.Format("\nDeposit Successful!\nPrevious Balance: {1:C2}\nDeposit Amount: {2:C2}\nNew balance is: {0:C2}\n",
                LoggedInUser.Balance, 
                originalBalance, 
                depositAmount));

            return;
        }

        // Withdrawal()
        // allows the user to make a withdrawal
        public void Withdrawal()
        {
            // get withdrawal amount from user
            Console.Clear();
            Console.WriteLine("Current Balance {0:C2}\nPlease enter how much you would like to withdraw in dollars and cents (e.g. 51.50): ", LoggedInUser.Balance);
            string withdrawalString = Console.ReadLine();

            // check that withdrawal string is not empty or null
            if (withdrawalString == null || withdrawalString == "" || withdrawalString == " ")
            {
                throw new ArgumentException("Withdrawal amount cannot be empty");
            }
            // check that withdrawal string only contains numbers and the decimal point
            else if (Regex.IsMatch(withdrawalString, @"[^0-9.]+"))
            {
                throw new ArgumentException("Withdrawal amount must be a decimal number of dollars and cents");
            }

            // convert withdrawal amount to double
            double withdrawalAmount = Convert.ToDouble(withdrawalString);

            // check if withdrawal string has a decimal point, if so, check that there are only two digits after the decimal point
            if (Regex.IsMatch(withdrawalString, @"[.]+") && withdrawalString.Substring(withdrawalString.IndexOf(".")).Length > 3)
            {
                throw new ArgumentException("Withdrawal amount can only be specified to two decimal places");
            }
            // check that the withdrawal amount is greater than zero
            else if (withdrawalAmount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero");
            }
            // check that the withdrawal amount is less than the user's current balance
            else if (withdrawalAmount > loggedInUser.Balance)
            {
                throw new ArgumentException(string.Format("Insufficient Funds to withdraw {0:C2}; Current Balance: {1:C2}", 
                    withdrawalAmount, 
                    loggedInUser.Balance));
            }

            double originalBalance = LoggedInUser.Balance;

            // subtract withdrawal amount from user's current balance
            LoggedInUser.Balance -= withdrawalAmount;

            // add withdrawal to transaction history
            LoggedInUser.AddHistory(string.Format("{0} Withdrawal: {1:C2}, Previous Balance: {3:C2}, New Balance: {2:C2}\n", DateTime.Now, withdrawalAmount, LoggedInUser.Balance, originalBalance));

            // display new balance
            Console.WriteLine(String.Format("\nWithdrawal Successful!\nPrevious Balance: {1:C2}\nWithdrawal Amount: {2:C2}\nNew balance is: {0:C2}\n", LoggedInUser.Balance, originalBalance, withdrawalAmount));

            return;
        }

        // CheckBalance() - displays the user's current balance
        public double CheckBalance()
        {
            double currentBalance = LoggedInUser.Balance;

            Console.Clear();
            Console.WriteLine("\n{0}'s current balance at {1} is: {2}\n", LoggedInUser.Username, DateTime.Now, currentBalance.ToString("C", CultureInfo.CurrentCulture));
            return currentBalance;
        }

        // GetTransactionHistory() - displays the LoggedInUser's transaction history
        public string GetTransactionHistory()
        {
            Console.Clear();
            Console.WriteLine("Transaction History for {0}\n", LoggedInUser.Username);
            Console.WriteLine(String.Join(Environment.NewLine, LoggedInUser.History));

            return LoggedInUser.History[0];
        } 
    }
}
