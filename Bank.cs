using System;
using System.Collections.Generic;

namespace CommBankApp
{
    // Central banking class managing all user accounts and banking operations
    // Handles login, signup, deposit, withdraw, transfer, balance and history
    class Bank
    {
        // Stores all registered users in the system
        private List<User> registeredUsers;

        private string bankName;

        // Read-only property to expose the bank name externally
        public string BankName => bankName;

        // Constructor - initialises the bank and loads a default test user
        public Bank(string name)
        {
            bankName        = name;
            registeredUsers = new List<User>();

            // Default test account pre-loaded as per project brief
            // Email: joe.doe@commbank.com.au | Password: Password123 | PIN: 1234
            SavingsAccount defaultAccount = new SavingsAccount(1000.00);
            User defaultUser = new User(
                "Joe Doe",
                "joe.doe@commbank.com.au",
                "Password123",
                "0400000000",
                30,
                "1234",
                defaultAccount
            );
            registeredUsers.Add(defaultUser);
        }

        // -----------------------------------------------------------
        // LOGIN
        // -----------------------------------------------------------

        // Manages the full login flow with a maximum of 3 attempts
        // Resets the daily withdrawal limit on each new successful login
        // Returns the matched User on success, or null on failure
        public User Login()
        {
            int  maxAttempts  = 3;
            int  attemptCount = 0;
            User loggedInUser = null;

            while (loggedInUser == null && attemptCount < maxAttempts)
            {
                Console.WriteLine();
                Console.Write("    Email:    ");
                string inputEmail = Console.ReadLine()?.Trim();

                Console.Write("    Password: ");
                string inputPassword = ReadMaskedInput();

                attemptCount++;

                // Search all registered users to find a matching account
                foreach (User user in registeredUsers)
                {
                    if (user.ValidateLogin(inputEmail, inputPassword))
                    {
                        loggedInUser = user;
                        break;
                    }
                }

                if (loggedInUser == null)
                {
                    int remaining = maxAttempts - attemptCount;
                    PrintColour("    \u2717 Invalid email or password.", ConsoleColor.Red);

                    if (remaining > 0)
                    {
                        Console.WriteLine($"    {remaining} attempt(s) remaining.\n");
                        Console.WriteLine("    1: Try again");
                        Console.WriteLine("    2: Main menu");
                        Console.WriteLine("    3: Quit");
                        Console.Write("\n    Select: ");
                        string choice = Console.ReadLine()?.Trim();

                        if (choice == "2") return null;
                        if (choice == "3") Quit();
                    }
                    else
                    {
                        Console.WriteLine();
                        PrintColour("    Account temporarily locked. Returning to main menu.", ConsoleColor.Red);
                        return null;
                    }
                }
            }

            // Reset daily withdrawal limit at the start of each new login session
            loggedInUser?.Account.ResetDailyLimit();
            return loggedInUser;
        }

        // -----------------------------------------------------------
        // SIGNUP
        // -----------------------------------------------------------

        // Collects user details and registers a new account in the system
        // User chooses between Savings and Checking account during registration
        public void Signup()
        {
            Console.WriteLine();
            PrintColour("    \u2500\u2500 Open a New CommBank Account \u2500\u2500", ConsoleColor.Cyan);
            Console.WriteLine();

            string username = RequireInput("    Full name:       ");
            string email    = RequireInput("    Email address:   ");
            string phone    = RequireInput("    Phone number:    ");

            // Keep asking until a valid positive integer age is provided
            int age = 0;
            while (true)
            {
                Console.Write("    Age:             ");
                if (int.TryParse(Console.ReadLine()?.Trim(), out age) && age > 0) break;
                PrintColour("    \u2717 Please enter a valid age.", ConsoleColor.Red);
            }

            // Keep asking until both password entries match
            string password = "";
            while (true)
            {
                Console.Write("    Create password: ");
                string pw1 = ReadMaskedInput();
                Console.Write("    Confirm password:");
                string pw2 = ReadMaskedInput();

                if (pw1.Length > 0 && pw1 == pw2) { password = pw1; break; }
                PrintColour("    \u2717 Passwords do not match. Please try again.", ConsoleColor.Red);
            }

            // Collect a 4-digit transaction PIN for securing banking operations
            string pin = SetTransactionPin();

            // Let the user select their preferred account type
            Account newAccount = SelectAccountType();

            registeredUsers.Add(new User(username, email, password, phone, age, pin, newAccount));

            Console.WriteLine();
            PrintColour($"    \u2713 Account created successfully! Welcome, {username}.", ConsoleColor.Green);
            PrintColour($"    Account Type   : {newAccount.AccountType}",                ConsoleColor.Green);
            PrintColour($"    Account Number : {newAccount.AccountNumber}",              ConsoleColor.Green);
        }

        // Prompts the user to set and confirm a 4-digit transaction PIN
        // PIN is required before every deposit, withdrawal and transfer
        private string SetTransactionPin()
        {
            while (true)
            {
                Console.Write("    Set 4-digit PIN: ");
                string pin1 = ReadMaskedInput();

                Console.Write("    Confirm PIN:     ");
                string pin2 = ReadMaskedInput();

                if (pin1.Length == 4 && int.TryParse(pin1, out _) && pin1 == pin2)
                    return pin1;

                PrintColour("    \u2717 PIN must be exactly 4 digits and both entries must match.", ConsoleColor.Red);
            }
        }

        // Prompts the user to select between Savings and Checking account type
        // Returns a new account object based on the selection
        public Account SelectAccountType()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("    Select Account Type:");
                Console.WriteLine("    \u250c\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2510");
                Console.WriteLine("    \u2502 [1] Savings Account  (3% monthly interest) \u2502");
                Console.WriteLine("    \u2514\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2518");
                Console.WriteLine("    \u250c\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2510");
                Console.WriteLine("    \u2502 [2] Checking Account ($500 overdraft limit)\u2502");
                Console.WriteLine("    \u2514\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2518");
                Console.Write("\n    Select: ");
                string pick = Console.ReadLine()?.Trim();

                if (pick == "1") return new SavingsAccount(0.00);
                if (pick == "2") return new CheckingAccount(0.00);

                PrintColour("    \u2717 Please enter 1 or 2.", ConsoleColor.Red);
            }
        }

        // -----------------------------------------------------------
        // PIN VERIFICATION
        // -----------------------------------------------------------

        // Prompts the user to enter their transaction PIN before any operation
        // Allows up to 3 attempts before cancelling the transaction
        // Returns true if PIN is verified, false if all attempts are exhausted
        public bool VerifyPin(User user)
        {
            int attempts = 3;

            while (attempts > 0)
            {
                Console.Write("    Enter PIN: ");
                string inputPin = ReadMaskedInput();

                if (user.ValidatePin(inputPin))
                    return true;

                attempts--;
                PrintColour($"    \u2717 Incorrect PIN. {attempts} attempt(s) remaining.", ConsoleColor.Red);
            }

            PrintColour("    \u2717 Too many incorrect PIN attempts. Transaction cancelled.", ConsoleColor.Red);
            return false;
        }

        // -----------------------------------------------------------
        // DEPOSIT
        // -----------------------------------------------------------

        // Prompts for a deposit amount, validates input and adds to balance
        // Requires PIN verification before processing
        public void Deposit(User user)
        {
            Console.WriteLine();

            if (!VerifyPin(user)) return;

            Console.Write("    Enter deposit amount: $");
            string input = Console.ReadLine()?.Trim();

            // Reject any non-numerical input as per brief requirement
            if (!double.TryParse(input, out double amount))
            {
                PrintColour("    \u2717 Only numerical data to be entered for deposit.", ConsoleColor.Red);
                return;
            }

            // Reject zero or negative values
            if (amount <= 0)
            {
                PrintColour("    \u2717 Deposit amount must be greater than zero.", ConsoleColor.Red);
                return;
            }

            user.Account.Deposit(amount);
            PrintColour($"    \u2713 ${amount:F2} deposited successfully.", ConsoleColor.Green);
            PrintColour($"    Current Balance: ${user.Account.Balance:F2}", ConsoleColor.Green);
        }

        // -----------------------------------------------------------
        // WITHDRAW
        // -----------------------------------------------------------

        // Prompts for a withdrawal amount, validates input and deducts from balance
        // Requires PIN verification and checks daily limit and available funds
        public void Withdraw(User user)
        {
            Console.WriteLine();

            if (!VerifyPin(user)) return;

            Console.Write("    Enter withdrawal amount: $");
            string input = Console.ReadLine()?.Trim();

            // Reject non-numerical input with the exact message specified in the brief
            if (!double.TryParse(input, out double amount))
            {
                PrintColour("    \u2717 Only numerical data to be entered for withdrawal.", ConsoleColor.Red);
                return;
            }

            if (amount <= 0)
            {
                PrintColour("    \u2717 Withdrawal amount must be greater than zero.", ConsoleColor.Red);
                return;
            }

            // Attempt the withdrawal and display appropriate message based on result
            if (user.Account.Withdraw(amount, out string reason))
            {
                PrintColour($"    \u2713 ${amount:F2} withdrawn successfully.", ConsoleColor.Green);
                PrintColour($"    Current Balance: ${user.Account.Balance:F2}", ConsoleColor.Green);
            }
            else
            {
                if (reason == "daily")
                    PrintColour($"    \u2717 Daily withdrawal limit of ${user.Account.DailyWithdrawalLimit:F2} reached.", ConsoleColor.Red);
                else
                    PrintColour("    \u2717 Not sufficient funds available.", ConsoleColor.Red);
            }
        }

        // -----------------------------------------------------------
        // VIEW BALANCE
        // -----------------------------------------------------------

        // Displays current account balance and account details clearly
        public void ViewBalance(User user)
        {
            Console.WriteLine();
            Console.WriteLine($"    Account Holder : {user.Username}");
            Console.WriteLine($"    Account Number : {user.Account.AccountNumber}");
            Console.WriteLine($"    Account Type   : {user.Account.AccountType}");
            Console.WriteLine("    \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500");
            PrintColour($"    Current Balance: ${user.Account.Balance:F2}", ConsoleColor.Cyan);

            // Show remaining daily withdrawal allowance for transparency
            double remaining = user.Account.DailyWithdrawalLimit - user.Account.DailyWithdrawnAmount;
            PrintColour($"    Daily Withdraw Remaining: ${remaining:F2}", ConsoleColor.DarkCyan);
            Console.WriteLine("    \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500");
        }

        // -----------------------------------------------------------
        // TRANSFER
        // -----------------------------------------------------------

        // Transfers funds from the logged-in user to another registered user
        // Requires PIN verification and applies all the same withdrawal rules
        public void Transfer(User sender)
        {
            Console.WriteLine();

            if (!VerifyPin(sender)) return;

            Console.Write("    Enter recipient email: ");
            string recipientEmail = Console.ReadLine()?.Trim();

            // Search registered users for the recipient account
            User recipient = null;
            foreach (User user in registeredUsers)
            {
                if (user.Email == recipientEmail && user.Email != sender.Email)
                {
                    recipient = user;
                    break;
                }
            }

            if (recipient == null)
            {
                PrintColour("    \u2717 Recipient account not found.", ConsoleColor.Red);
                return;
            }

            Console.Write("    Enter transfer amount: $");
            string input = Console.ReadLine()?.Trim();

            if (!double.TryParse(input, out double amount))
            {
                PrintColour("    \u2717 Only numerical data to be entered for transfer.", ConsoleColor.Red);
                return;
            }

            if (amount <= 0)
            {
                PrintColour("    \u2717 Transfer amount must be greater than zero.", ConsoleColor.Red);
                return;
            }

            if (sender.Account.Transfer(recipient.Account, amount, out string reason))
            {
                PrintColour($"    \u2713 ${amount:F2} transferred to {recipient.Username} successfully.", ConsoleColor.Green);
                PrintColour($"    Current Balance: ${sender.Account.Balance:F2}", ConsoleColor.Green);
            }
            else
            {
                if (reason == "daily")
                    PrintColour($"    \u2717 Daily withdrawal limit of ${sender.Account.DailyWithdrawalLimit:F2} reached.", ConsoleColor.Red);
                else
                    PrintColour("    \u2717 Not sufficient funds available.", ConsoleColor.Red);
            }
        }

        // -----------------------------------------------------------
        // INTEREST - SAVINGS ACCOUNT FEATURE
        // -----------------------------------------------------------

        // Applies monthly interest to the user's savings account
        // Only available for SavingsAccount holders
        public void ApplyInterest(User user)
        {
            if (user.Account is SavingsAccount savings)
            {
                savings.ApplyInterest();
                PrintColour($"    \u2713 Monthly interest applied at {savings.InterestRate * 100}%.", ConsoleColor.Green);
                PrintColour($"    Current Balance: ${savings.Balance:F2}", ConsoleColor.Green);
            }
            else
            {
                PrintColour("    \u2717 Interest is only available for Savings Accounts.", ConsoleColor.Red);
            }
        }

        // -----------------------------------------------------------
        // HELPERS
        // -----------------------------------------------------------

        // Prints a message in a specified colour then resets to default
        public void PrintColour(string message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // Overloaded version - prints using the current default console colour
        public void PrintColour(string message)
        {
            Console.WriteLine(message);
        }

        // Keeps prompting until the user enters a non-empty value
        private string RequireInput(string prompt)
        {
            string input = "";
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input))
                    PrintColour("    \u2717 This field cannot be left blank.", ConsoleColor.Red);
            }
            return input;
        }

        // Reads input without displaying characters on screen
        // Used for passwords and PINs - each keystroke shows as *
        private string ReadMaskedInput()
        {
            string result = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(intercept: true);

                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    result += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && result.Length > 0)
                {
                    result = result[..^1];
                    Console.Write("\b \b");
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return result;
        }

        // Displays a farewell message and exits the application cleanly
        public void Quit()
        {
            Console.WriteLine();
            PrintColour("    Thank you for banking with CommBank. Goodbye!", ConsoleColor.Yellow);
            Console.WriteLine();
            Environment.Exit(0);
        }

        // Destructor - called when the Bank instance is garbage collected
        ~Bank()
        {
            // Cleanup for unmanaged resources would be handled here if needed
        }
    }
}