using System;
using System.Collections.Generic;

namespace CommBankApp
{
    // Base class representing a generic bank account
    // SavingsAccount and CheckingAccount both inherit from this class
    class Account
    {
        // Protected so subclasses can read and update these directly
        protected double balance;
        protected string accountType;
        protected List<string> transactionHistory;

        // Tracks total amount withdrawn today for the daily limit feature
        protected double dailyWithdrawnAmount;

        // Maximum amount that can be withdrawn in a single day
        protected double dailyWithdrawalLimit;

        // Public read-only properties
        public double Balance            => balance;
        public string AccountType        => accountType;
        public double DailyWithdrawnAmount => dailyWithdrawnAmount;
        public double DailyWithdrawalLimit => dailyWithdrawalLimit;
        public string AccountNumber      { get; private set; }

        // Constructor - sets up a new account with a starting balance
        public Account(double initialBalance)
        {
            balance              = initialBalance;
            transactionHistory   = new List<string>();
            AccountNumber        = GenerateAccountNumber();
            dailyWithdrawnAmount = 0;
            dailyWithdrawalLimit = 2000.00;
        }

        // Adds the given amount to the account balance
        // Returns false if the amount is not a valid positive number
        public virtual bool Deposit(double amount)
        {
            if (amount <= 0)
                return false;

            balance += amount;
            LogTransaction($"  [DEPOSIT]   +${amount:F2}   |   Balance: ${balance:F2}");
            return true;
        }

        // Deducts the given amount from the balance
        // Checks for sufficient funds and daily withdrawal limit
        // Returns false if either check fails
        public virtual bool Withdraw(double amount, out string failReason)
        {
            failReason = "";

            if (amount <= 0)
            {
                failReason = "invalid";
                return false;
            }

            if (dailyWithdrawnAmount + amount > dailyWithdrawalLimit)
            {
                failReason = "daily";
                return false;
            }

            if (amount > balance)
            {
                failReason = "funds";
                return false;
            }

            balance              -= amount;
            dailyWithdrawnAmount += amount;
            LogTransaction($"  [WITHDRAW]  -${amount:F2}   |   Balance: ${balance:F2}");
            return true;
        }

        // Transfers an amount from this account to a target account
        // Uses the Withdraw method so daily limit and fund checks still apply
        public bool Transfer(Account targetAccount, double amount, out string failReason)
        {
            if (!Withdraw(amount, out failReason))
                return false;

            targetAccount.Deposit(amount);
            LogTransaction($"  [TRANSFER]  -${amount:F2}   |   Balance: ${balance:F2}");
            return true;
        }

        // Resets the daily withdrawal tracker — called at the start of each login session
        public void ResetDailyLimit()
        {
            dailyWithdrawnAmount = 0;
        }

        // Prints all recorded transactions for this account to the console
        public void PrintTransactionHistory()
        {
            Console.WriteLine($"    Account Holder : See account details");
            Console.WriteLine($"    Account Number : {AccountNumber}");
            Console.WriteLine($"    Account Type   : {accountType}");
            Console.WriteLine();

            if (transactionHistory.Count == 0)
            {
                Console.WriteLine("    No transactions recorded yet.");
                return;
            }

            Console.WriteLine("    \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500");
            foreach (string record in transactionHistory)
                Console.WriteLine(record);
            Console.WriteLine("    \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500");
        }

        // Adds a transaction entry to the history log
        protected void LogTransaction(string entry)
        {
            transactionHistory.Add(entry);
        }

        // Generates a unique random 8-digit account number
        private string GenerateAccountNumber()
        {
            Random rng = new Random();
            return rng.Next(10000000, 99999999).ToString();
        }

        // Destructor
        ~Account()
        {
            // Cleanup for unmanaged resources would be handled here if needed
        }
    }
}