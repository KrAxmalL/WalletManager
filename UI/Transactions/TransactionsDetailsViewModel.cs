using Models;
using Models.Transactions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Transactions
{
    public class TransactionsDetailsViewModel : BindableBase
    {
        private Transaction _transaction;

        public string DisplayName
        {
            get
            {
                return $"Performed transaction on  {_transaction.Date.ToString()} at {_transaction.AmountOfMoney}{Currencies.currencyToSymbol(_transaction.Currency)}";
            }
        }

        public string Amount
        {
            get
            {
                return $"{_transaction.AmountOfMoney}{Currencies.currencyToSymbol(_transaction.Currency)}";
            }

            set
            {
                _transaction.AmountOfMoney = Decimal.Parse(value);
                RaisePropertyChanged(nameof(Amount));
            }
        }

        public string TransactionCurrency
        {
            get
            {
                return Currencies.currencyToString(_transaction.Currency);
            }
        }

        public string Description
        {
            get
            {
                return _transaction.Description;
            }

            set
            {
                _transaction.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public string Date
        {
            get
            {
                return _transaction.Date.ToString();
            }

            set
            {
                _transaction.Date = DateTimeOffset.Parse(value);
                RaisePropertyChanged(nameof(Date));
            }
        }

        public Guid TransactionWalletGuid
        {
            get
            {
                return _transaction.WalletGuid;
            }
            
        }

        public Guid TransactionGuid
        {
            get
            {
                return _transaction.Guid;
            }
        }

            

        public TransactionsDetailsViewModel(Transaction current)
        {
            _transaction = current;
        }
    }
}
