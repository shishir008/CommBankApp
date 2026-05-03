using System;

namespace CommBankApp
{
    // Represents a registered bank customer
    // Each user owns one bank account and is secured by a 4-digit transaction PIN
    class User
    {
        // Private fields - not accessible from outside this class
        private string username;
        private string email;
        private string password;
        private string phone;
        private int    age;
        private string transactionPin;

        // Read-only properties to safely expose necessary user information
        public string  Username => username;
        public string  Email    => email;

        // The bank account linked to this user
        // Can be either a SavingsAccount or CheckingAccount
        public Account Account { get; private set; }

        // Constructor - creates a new user with all required personal details and account
        public User(string username, string email, string password, string phone, int age, string pin, Account account)
        {
            this.username       = username;
            this.email          = email;
            this.password       = password;
            this.phone          = phone;
            this.age            = age;
            this.transactionPin = pin;
            this.Account        = account;
        }

        // Validates login credentials - both email and password must match exactly
        public bool ValidateLogin(string inputEmail, string inputPassword)
        {
            return email == inputEmail && password == inputPassword;
        }

        // Validates the entered PIN against the stored transaction PIN
        // Used before processing any deposit, withdrawal or transfer
        public bool ValidatePin(string inputPin)
        {
            return transactionPin == inputPin;
        }

        // Destructor - called by the garbage collector when the object is no longer in use
        ~User()
        {
            // Cleanup for unmanaged resources would be handled here if needed
        }
    }
}