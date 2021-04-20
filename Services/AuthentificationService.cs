using DataStorage;
using Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class AuthentificationService
    {
        private FileDataStorage<DbUser> _storage = new FileDataStorage<DbUser>();
        private static User _currentActiveUser;

        public static User ActiveUser
        {
            get
            {
                return _currentActiveUser;
            }
        }

        public async Task<User> AuthentificateUser(AuthUser candidate)
        {
            if (String.IsNullOrWhiteSpace(candidate.Login) || String.IsNullOrWhiteSpace(candidate.Password))
            {
                throw new ArgumentException("Login or password is empty!");
            }

            var users = await _storage.GetAllAsync();

            var dbUser = users.FirstOrDefault(user => user.Login == candidate.Login && user.Password == candidate.Password);
            if (dbUser == null)
            {
                throw new Exception("Wrong login or password!");
            }
            _currentActiveUser = new User(dbUser.Guid, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Login);
            return _currentActiveUser;
        }

        public async Task<bool> RegisterUser(RegistrationUser candidate)
        {
            var users = await _storage.GetAllAsync();
            foreach (var userToShow in users)
            {
                Console.WriteLine(userToShow.ToString());
            }

            var dbUser = users.FirstOrDefault(user => user.Login == candidate.Login);
            if (dbUser != null)
            {
                throw new Exception("User already exists!");
            }

            if (String.IsNullOrWhiteSpace(candidate.Login) || String.IsNullOrWhiteSpace(candidate.Password) || String.IsNullOrWhiteSpace(candidate.FirstName)
                || String.IsNullOrWhiteSpace(candidate.LastName) || String.IsNullOrWhiteSpace(candidate.Email))
            {
                throw new ArgumentException("One or more fields is empty!");
            }

            if (candidate.Login.Length < 3)
            {
                throw new Exception("Login must be 3 symbols or longer!");
            }

            if (candidate.Password.Length < 5)
            {
                throw new Exception("Password must be 5 symbols or longer!");
            }

            dbUser = new DbUser(candidate.FirstName, candidate.LastName, candidate.Email, candidate.Login, candidate.Password);
            await _storage.AddOrUpdateAsync(dbUser);
            return true;
        }
    }
}
