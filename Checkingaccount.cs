using System;

namespace CommBankApp
{
    // CheckingAccount inherits from the base Account class
    // Extra Feature 2: Overdraft limit allows spending beyond zero up to a defined threshold
    // Useful for everyday spending accounts where short-term overdraft is permitted
    class CheckingAccount : Account
    {
        // The maximum amount this account is permitted to go into negative
        private double overdraftLimit;
        public double OverdraftLimit => overdraftLimit;

        // Constructor - sets the starting balance and overdraft limit
        // Calls the base Account constructor to initialise shared fields
        public CheckingAccount(double initialBalance, double overdraftLimit = 500.00)
            : base(initialBalance)
        {
            this.overdraftLimit = overdraftLimit;
            accountType         = "Checking Account";
        }

        // Overrides base Withdraw to allow spending into the overdraft zone
        // Withdrawal is only blocked when it would push balance past the overdraft limit
        public override bool Withdraw(double amount, out string failReason)
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

            if ((balance - amount) < -overdraftLimit)
            {
                failReason = "funds";
                return false;
            }

            balance              -= amount;
            dailyWithdrawnAmount += amount;
            LogTransaction($"  [WITHDRAW]  -${amount:F2}   |   Balance: ${balance:F2}");
            return true;
        }
    }
}