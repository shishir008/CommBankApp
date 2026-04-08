using System;

namespace CommBankApp
{
    // Entry point for the CommBank console application
    // Controls the main menu loop and directs user choices to the Bank class
    class Program
    {
        static void Main(string[] args)
        {
            // Create the Bank object with CommBank as the selected bank
            Bank bank = new Bank("Commonwealth Bank of Australia");

            // Keep the application running until the user chooses to quit
            while (true)
            {
                PrintBanner(bank.BankName);

                PrintOption("[1] Login");
                PrintOption("[2] Sign Up");
                PrintOption("[3] Quit");

                Console.Write("\n    Select Option: ");
                string option = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        // Attempt login and proceed to banking menu if successful
                        User loggedIn = bank.Login();
                        if (loggedIn != null)
                            ShowBankingMenu(loggedIn, bank);
                        else
                            Pause();
                        break;

                    case "2":
                        bank.Signup();
                        Pause();
                        break;

                    case "3":
                        bank.Quit();
                        break;

                    default:
                        bank.PrintColour("    \u2717 Invalid option. Please enter 1, 2 or 3.", ConsoleColor.Red);
                        Pause();
                        break;
                }
            }
        }

        // Displays the banking options screen after a successful login
        // Transaction features such as deposit and withdraw will be added in Assessment 3
        static void ShowBankingMenu(User user, Bank bank)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    \u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.WriteLine($"    \u2551  Commonwealth Bank of Australia          \u2551");
            Console.WriteLine("    \u255a\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255d");
            Console.ResetColor();

            bank.PrintColour($"\n    Welcome, {user.Username}!", ConsoleColor.Green);
            Console.WriteLine();

            PrintOption("[1] View Balance");
            PrintOption("[2] Deposit");
            PrintOption("[3] Withdraw");
            PrintOption("[4] Transfer");
            PrintOption("[5] Logout");

            Console.Write("\n    Select Option: ");
            string choice = Console.ReadLine()?.Trim();

            // Option 5 logs the user out and returns to the main menu
            if (choice == "5") return;

            Console.WriteLine();
            bank.PrintColour("    This feature will be available in the next update.", ConsoleColor.DarkGray);
            Pause();
        }

        // Draws a bordered box around a single menu option for a cleaner UI
        static void PrintOption(string text)
        {
            int width     = 24;
            string padded = text.PadRight(width);
            Console.WriteLine($"    \u250c{new string('\u2500', width+2 )}\u2510");
            Console.WriteLine($"    \u2502 {padded} \u2502");
            Console.WriteLine($"    \u2514{new string('\u2500', width+2 )}\u2518");
            Console.WriteLine();
        }

        // Prints the CommBank welcome banner at the top of the main menu
        static void PrintBanner(string bankName)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n    \u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.WriteLine($"    \u2551  Welcome to {bankName}\u2551");
            Console.WriteLine("    \u255a\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255d");
            Console.ResetColor();
            Console.WriteLine();
        }

        // Pauses the screen until the user presses any key
        static void Pause()
        {
            Console.WriteLine("\n    Press any key to continue...");
            Console.ReadKey(intercept: true);
        }
    }
}