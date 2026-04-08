using System;

namespace CommBankApp
{
    // Represents a registered bank customer
    // Stores all personal details and login credentials securely
    class User
    {
        // Private fields - cannot be accessed or modified from outside this class
        private string username;
        private string email;
        private string password;
        private string phone;
        private int age;

        // Read-only properties - allow outside classes to read but not change values
        public string Username => username;
        public string Email    => email;

        // Constructor - called when a new User object is created during signup
        public User(string username, string email, string password, string phone, int age)
        {
            this.username = username;
            this.email    = email;
            this.password = password;
            this.phone    = phone;
            this.age      = age;
        }

        // Checks if the provided email and password match the stored credentials
        // Returns true only when both values are correct
        public bool ValidateLogin(string inputEmail, string inputPassword)
        {
            return email == inputEmail && password == inputPassword;
        }

        // Destructor - called by the garbage collector when the object is no longer in use
        ~User()
        {
            // Cleanup for unmanaged resources would be handled here if needed
        }
    }
}