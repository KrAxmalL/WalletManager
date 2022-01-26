using Models;
using Models.Transactions;
using Models.Wallets;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Pinkevych_WalletManager.WalletsWPF.RuntimeStorage;
using Prism.Commands;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pinkevych_WalletManager.WalletsWPF.Transactions
{
    public class TransactionAddingViewModel : INotifyPropertyChanged, INavigatable<TransactionNavigatableTypes>
    {
        private Transaction _transactionToAdd;
        private List<Transaction> _transactions;
        private ObservableCollection<TransactionsDetailsViewModel> _transactionsViewModels;

        private Wallet _wallet;

        private Action _goToMainTransactions;

        private bool _isEnabled;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
        public decimal TransactionAmount
        {
            get
            {
                return _transactionToAdd.AmountOfMoney;
            }
            set
            {
                _transactionToAdd.AmountOfMoney = value;
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public string TransactionDate
        {
            get
            {
                return _transactionToAdd.Date.ToString();
            }
            set
            {
                _transactionToAdd.Date = DateTimeOffset.Parse(value);
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }


        public string TransactionDescription
        {
            get
            {
                return _transactionToAdd.Description;
            }
            set
            {
                _transactionToAdd.Description = value;
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public string TransactionDisplayCurrency
        {
            get
            {
                return Currencies.currencyToString(TransactionCurrency);
            }
        }

        public Currencies.Currency TransactionCurrency
        {
            get
            {
                return _transactionToAdd.Currency;
            }
            set
            {
                _transactionToAdd.Currency = value;
                OnPropertyChanged();
                AddTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddTransactionCommand { get; }

        public DelegateCommand GoToMainTransactionsCommand { get; }

        public TransactionAddingViewModel(Action goToMainTransactions, ref Wallet wallet)
        {
            _wallet = wallet;
            _transactions = RuntimeDataStorage.Transactions;
            _transactionToAdd = new Transaction(new Guid(), 0, Currencies.Currency.Dollar, "", new DateTimeOffset(),
                                                                _wallet.Guid, AuthentificationService.ActiveUser.Guid);
            _transactionsViewModels = RuntimeDataStorage.TransactionsViewModels;
            AddTransactionCommand = new DelegateCommand(AddTransactionToCollection, isAddingEnabled);
            _goToMainTransactions = goToMainTransactions;
            GoToMainTransactionsCommand = new DelegateCommand(_goToMainTransactions);
            _isEnabled = true;
        }

        private async void AddTransactionToCollection()
        {
            try
            {
                IsEnabled = false;

                await TransactionsService.AddTransaction(_transactionToAdd);
                _transactions.Add(_transactionToAdd);

                TransactionsDetailsViewModel _newViewModel = new TransactionsDetailsViewModel(_transactionToAdd);
                _transactionsViewModels.Add(_newViewModel);
                _transactionToAdd = new Transaction(new Guid(), 0, Currencies.Currency.Dollar, "", new DateTimeOffset(),
                                                                _wallet.Guid, AuthentificationService.ActiveUser.Guid);
                _wallet.AddTransaction(_transactionToAdd);

                MessageBox.Show("Successfully added new transaction!");
                _goToMainTransactions.Invoke();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't add transaction! {e.Message}");
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private bool isAddingEnabled()
        {
            return true;
        }

        public TransactionNavigatableTypes Type
        {
            get
            {
                return TransactionNavigatableTypes.AddingTransaction;
            }
        }

        public void ClearSensitiveData()
        {           
            TransactionAmount = 0;
            TransactionDate = "";
            TransactionDescription = "";          
            TransactionCurrency = Currencies.Currency.Dollar;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
