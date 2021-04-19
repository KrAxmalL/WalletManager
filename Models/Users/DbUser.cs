using DataStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Users
{
    public class DbUser : IStorable
    {

        public string FirstName
        {
            get;
        }
        public string LastName
        {
            get;
        }

        public string Email
        {
            get;
        }

        public string Login
        {
            get;
        }

        public string Password
        {
            get;
        }

        public Guid Guid
        {
            get;
            set;
        }

        public DbUser(string firstName, string lastName, string email, string login, string password)
        {
            Guid = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Login = login;
            Password = password;
        }

        public override string ToString()
        {
            return $"{FirstName}, {LastName}, email: {Email}, Guid: {Guid} Login: {Login}, Password: {Password}";
        }
    }
}

