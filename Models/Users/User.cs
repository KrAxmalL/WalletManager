using Models.Transactions;
using Models.Wallets;
using System;
using System.Collections.Generic;

namespace Models.Users
{
    public class User
    {
        private readonly Guid _guid;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _login;
        private List<Wallet> _wallets;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FullName
        {
            get
            {
                var result = "";
                if (!String.IsNullOrWhiteSpace(LastName))
                {
                    result += LastName + ", ";
                }
                if (!String.IsNullOrWhiteSpace(FirstName))
                {

                    result += FirstName;
                }
                else
                {
                    result = result.Replace(", ", "");
                }
                return result;
            }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public User(Guid guid, string firstName, string lastName, string email, string login)
        {
            this._guid = guid;
            this._firstName = firstName;
            this._lastName = lastName;
            this._email = email;
            this._login = login;
            _wallets = new List<Wallet>();
        }

        public bool AddWallet(Wallet wallet)
        {
            if (wallet != null && !_wallets.Contains(wallet))
            {
                _wallets.Add(wallet);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Wallet GetWallet(int position)
        {
            if (position < 0 || position >= _wallets.Count)
            {
                return null;
            }
            else
            {
                return _wallets[position];
            }
        }

        public bool ShareWallet(Wallet wallet, User toWhom)
        {
            if (wallet != null && _wallets.Contains(wallet))
            {
                return toWhom.AddWallet(wallet);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveWallet(Wallet wallet)
        {
            if (wallet == null)
            {
                return false;
            }
            else
            {
                return _wallets.Remove(wallet);
            }
        }

        public bool RemoveWallet(int position)
        {
            if (position < 0 || position >= _wallets.Count)
            {
                return false;
            }
            else
            {
                _wallets.RemoveAt(position);
                return true;
            }
        }

        public bool AddTransactionToWallet(Wallet wallet, Transaction transaction)
        {
            if (wallet != null && transaction != null)
            {
                return wallet.AddTransaction(transaction);
            }
            else
            {
                return false;
            }
        }

        public bool RemoveTransactionFromWallet(Wallet wallet, Transaction transaction)
        {
            if (wallet != null && transaction != null)
            {
                return wallet.RemoveTransaction(transaction);
            }
            else
            {
                return false;
            }
        }


        public bool ChangeValueOfTransaction(Wallet wallet, int transactionPosition, decimal newValue)
        {
            if (wallet != null)
            {
                return wallet.ChangeValueOfTransaction(transactionPosition, newValue);
            }
            else
            {
                return false;
            }
        }

        public bool ChangeValueOfTransaction(int walletPosition, int transactionPosition, decimal newValue)
        {
            Wallet wallet = GetWallet(walletPosition);
            return ChangeValueOfTransaction(wallet, transactionPosition, newValue);
        }

        public bool ChangeCurrencyOfTransaction(Wallet wallet, int transactionPosition, Currencies.Currency newValue)
        {
            if (wallet != null)
            {
                return wallet.ChangeCurrencyOfTransaction(transactionPosition, newValue);
            }
            else
            {
                return false;
            }
        }

        public bool ChangeCurrencyOfTransaction(int walletPosition, int transactionPosition, Currencies.Currency newValue)
        {
            Wallet wallet = GetWallet(walletPosition);
            return ChangeCurrencyOfTransaction(wallet, transactionPosition, newValue);
        }

        public bool ChangeDateOfTransaction(Wallet wallet, int transactionPosition, DateTimeOffset newValue)
        {
            if (wallet != null)
            {
                return wallet.ChangeDateOfTransaction(transactionPosition, newValue);
            }
            else
            {
                return false;
            }
        }

        public bool ChangeDateOfTransaction(int walletPosition, int transactionPosition, DateTimeOffset newValue)
        {
            Wallet wallet = GetWallet(walletPosition);
            return ChangeDateOfTransaction(wallet, transactionPosition, newValue);
        }

        public bool ChangeDescriptionOfTransaction(Wallet wallet, int transactionPosition, string newValue)
        {
            if (wallet != null)
            {
                return wallet.ChangeDescriptionOfTransaction(transactionPosition, newValue);
            }
            else
            {
                return false;
            }
        }

        public bool ChangeDescriptionOfTransaction(int walletPosition, int transactionPosition, string newValue)
        {
            Wallet wallet = GetWallet(walletPosition);
            return ChangeDescriptionOfTransaction(wallet, transactionPosition, newValue);
        }

        public string ShowWallets(int startingIndex, int endIndex)
        {
            if (startingIndex < 1 || startingIndex > _wallets.Count || endIndex < 1 || endIndex > _wallets.Count)
            {
                return "";
            }
            else
            {
                String res = "";
                for (int i = startingIndex - 1; i < endIndex; ++i)
                {
                    res += $"Transaction {i + 1}: {_wallets[i]} \n";
                }
                return res;
            }
        }

        public override string ToString()
        {
            return $"{FullName}, email: {_email}";
        }

        public bool Validate()
        {
            if (String.IsNullOrWhiteSpace(_firstName))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(_lastName))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(_email))
            {
                return false;
            }

            return true;
        }
    }
}

