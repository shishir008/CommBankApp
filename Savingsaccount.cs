using System;

namespace CommBankApp
{
    // SavingsAccount inherits from the base Account class
    // Extra Feature 1: Monthly interest that grows the account balance over time
    // Savings accounts strictly cannot go below zero - no overdraft allowed
    class SavingsAccount : Account
    {
        // Interest rate applied when the user requests monthly interest
        private double interestRate;
        public double InterestRate => interestRate;

        // Constructor - sets the starting balance and interest rate
        // Calls the base Account constructor to initialise shared fields
        public SavingsAccount(double initialBalance, double interestRate = 0.03)
            : base(initialBalance)
        {
            this.interestRate = interestRate;
            accountType       = "Savings Account";
        }

        // Calculates and applies monthly interest to the current balance
        // Records the transaction in the history log
        public void ApplyInterest()
        {
            double interest = balance * interestRate;
            balance        += interest;
            LogTransaction($"  [INTEREST]  +${interest:F2}   |   Balance: ${balance:F2}   (Rate: {interestRate * 100}%)");
        }

        // Overrides base Withdraw to strictly block any withdrawal that would go below zero
        // Savings accounts have no overdraft - balance must stay at or above zero
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
    }
}