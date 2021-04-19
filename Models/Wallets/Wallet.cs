using Models.Transactions;
using System;
using System.Collections.Generic;

namespace Models.Wallets
{
    public class Wallet
    {
        private readonly Guid _guid;
        private string _name;
        private string _description;
        private Currencies.Currency _currency;
        private decimal _startingBalance;
        private decimal _currentBalance;
        private readonly Guid _ownerGuid;
        private List<Transaction> _transactions;
        //private List<Guid> _transactionsGuids;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public Currencies.Currency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
        public decimal StartingBalance
        {
            get { return _startingBalance; }
        }

        public decimal CurrentBalance
        {
            get { return _currentBalance; }
            set { _currentBalance = value; }
        }

        public Guid OwnerGuid
        {
            get { return _ownerGuid; }
        }

        public Guid Guid
        {
            get { return _guid; }
        }

        public Wallet(Guid guid, string name, string description, Currencies.Currency currency, decimal startingBalance, decimal currentBalance, Guid ownerGuid)
        {
            this._guid = guid;
            this._name = name;
            this._description = description;
            this._currency = currency;
            this._startingBalance = startingBalance;
            this._currentBalance = currentBalance;
            this._ownerGuid = ownerGuid;
            this._transactions = new List<Transaction>();
            //this._transactionsGuids = new List<Guid>();
        }

        public bool AddTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                if (!_transactions.Contains(transaction))
                {
                    _transactions.Add(transaction);
                    decimal converted = Currencies.Convert(transaction.Currency, this._currency, transaction.AmountOfMoney);
                    _currentBalance += converted;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Transaction GetTransaction(int position)
        {
            if (position < 0 || position >= _transactions.Count)
            {
                return null;
            }
            else
            {
                return _transactions[position];
            }
        }

        public string showTransactions(int startingIndex, int endIndex)
        {
            if (startingIndex < 1 || startingIndex > _transactions.Count || endIndex < 1 || endIndex > _transactions.Count)
            {
                return "";
            }
            else
            {
                String res = "";
                for (int i = startingIndex - 1; i < endIndex; ++i)
                {
                    res += $"Transaction {i + 1}: {_transactions[i]} \n";
                }
                return res;
            }
        }

        public string showTransactions(int startingIndex)
        {
            return showTransactions(startingIndex, _transactions.Count);
        }

        public string showTransactions()
        {
            int maxToShow = _transactions.Count;
            if (maxToShow > 10)
            {
                maxToShow = 10;
            }
            return showTransactions(1, maxToShow);
        }

        public bool RemoveTransaction(int position)
        {
            if (position < 0 || position >= _transactions.Count)
            {
                return false;
            }
            else
            {
                _transactions.RemoveAt(position);
                return true;
            }
        }

        public bool RemoveTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                return false;
            }
            else
            {
                return _transactions.Remove(transaction);
            }
        }

        public bool ChangeValueOfTransaction(int position, decimal newValue)
        {
            var transactionToEdit = GetTransaction(position);
            if (transactionToEdit == null)
            {
                return false;
            }
            else
            {
                transactionToEdit.AmountOfMoney = newValue;
                return true;
            }
        }

        public bool ChangeCurrencyOfTransaction(int position, Currencies.Currency newValue)
        {
            var transactionToEdit = GetTransaction(position);
            if (transactionToEdit == null)
            {
                return false;
            }
            else
            {
                transactionToEdit.Currency = newValue;
                return true;
            }
        }

        public bool ChangeDateOfTransaction(int position, DateTimeOffset newValue)
        {
            var transactionToEdit = GetTransaction(position);
            if (transactionToEdit == null)
            {
                return false;
            }
            else
            {
                if (newValue != null)
                {
                    transactionToEdit.Date = newValue;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ChangeDescriptionOfTransaction(int position, string newValue)
        {
            var transactionToEdit = GetTransaction(position);
            if (transactionToEdit == null)
            {
                return false;
            }
            else
            {
                if (newValue != null)
                {
                    transactionToEdit.Description = newValue;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public decimal GetIncome()
        {
            decimal result = 0;
            if (_transactions.Count == 0)
            {
                return result;
            }
            else
            {
                foreach (var transaction in _transactions)
                {
                    if (transaction.AmountOfMoney > 0)
                    {
                        decimal converted = Currencies.Convert(transaction.Currency, this._currency, transaction.AmountOfMoney);
                        result += converted;
                    }
                }

                return result;
            }
        }

        public decimal GetSpending()
        {
            decimal result = 0;
            if (_transactions.Count == 0)
            {
                return result;
            }
            else
            {
                foreach (var transaction in _transactions)
                {
                    if (transaction.AmountOfMoney < 0)
                    {
                        decimal converted = Currencies.Convert(transaction.Currency, this._currency, transaction.AmountOfMoney);
                        result -= converted;
                    }
                }

                return result;
            }
        }


        public override string ToString()
        {
            return $"Wallet {Name}: {Description}, current balance: {CurrentBalance}{Currencies.currencyToString(_currency)}";
        }

        public bool Validate()
        {
            if (String.IsNullOrWhiteSpace(_name))
            {
                return false;
            }

            return true;
        }
    }
}

