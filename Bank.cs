using System;
using System.Collections.Generic;

namespace CommBankApp
{
    // Handles all core banking operations including login and signup
    // Maintains a list of all registered users in the system
    class Bank
    {
        // Private list to store all registered user accounts
        private List<User> registeredUsers;

        // Private field to store the bank name
        private string bankName;

        // Read-only property to expose the bank name outside this class
        public string BankName => bankName;

        // Constructor - sets up the bank with its name and loads a default test account
        public Bank(string name)
        {
            bankName        = name;
            registeredUsers = new List<User>();

            // Default test account loaded for login testing as per project brief
            // Email:johnDoe@gmail.com | Password: Password123
            User defaultUser = new User("Joe.Doe", "johnDoe@gmail.com", "Password123", "0400000000", 30);
            registeredUsers.Add(defaultUser);
        }

        // Handles the full login process with a maximum of 3 attempts
        // Returns the matched User object on success, or null if login fails
        public User Login()
        {
            int maxAttempts   = 3;
            int attemptCount  = 0;
            User loggedInUser = null;

            while (loggedInUser == null && attemptCount < maxAttempts)
            {
                Console.WriteLine();
                Console.Write("    Email:    ");
                string inputEmail = Console.ReadLine()?.Trim();

                Console.Write("    Password: ");
                string inputPassword = ReadMaskedPassword();

                attemptCount++;

                // Loop through all registered users to find a matching account
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

            return loggedInUser;
        }

        // Collects user details and registers a new account
        // All fields are required and passwords must match before saving
        public void Signup()
        {
            Console.WriteLine();
            PrintColour("    \u2500\u2500 Open a New Account \u2500\u2500", ConsoleColor.Cyan);
            Console.WriteLine();

            // Collect all required personal details
            string username = RequireInput("    Full name:       ");
            string email    = RequireInput("    Email address:   ");
            string phone    = RequireInput("    Phone number:    ");

            // Keep asking until a valid age is entered
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
                string pw1 = ReadMaskedPassword();
                Console.Write("    Confirm password:");
                string pw2 = ReadMaskedPassword();

                if (pw1.Length > 0 && pw1 == pw2) { password = pw1; break; }
                PrintColour("    \u2717 Passwords do not match. Please try again.", ConsoleColor.Red);
            }

            // Save the new user to the registered users list
            registeredUsers.Add(new User(username, email, password, phone, age));

            Console.WriteLine();
            PrintColour($"    \u2713 Account created successfully! Welcome, {username}.", ConsoleColor.Green);
        }

        // Prints a message in the specified colour then resets back to default
        public void PrintColour(string message, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        // Overloaded version - prints a message using the default console colour
        public void PrintColour(string message)
        {
            Console.WriteLine(message);
        }

        // Keeps prompting the user until they enter a non-empty value
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

        // Reads password input without showing characters on screen
        // Each keystroke is displayed as * for security
        private string ReadMaskedPassword()
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

        // Displays a farewell message and exits the application
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