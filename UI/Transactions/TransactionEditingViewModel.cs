using Models;
using Models.Transactions;
using Models.Wallets;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Pinkevych_WalletManager.WalletsWPF.RuntimeStorage;
using Pinkevych_WalletManager.WalletsWPF.Transactions;
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
    public class TransactionEditingViewModel : INotifyPropertyChanged, INavigatable<TransactionNavigatableTypes>
    {
        private Transaction _transactionToEdit;
        private decimal _newAmount;
        private DateTimeOffset _newDate;
        private string _newDescription;
        private Currencies.Currency _newCurrency;
        private List<Transaction> _transactions;
        private ObservableCollection<TransactionsDetailsViewModel> _transactionsViewModels;

        private Wallet _wallet;

        private Action _goToMainTransactions;

        private bool _isEnabled;

        public List<Transaction> Transactions
        {
            get
            {
                _transactions.Clear();
                foreach (var transaction in RuntimeDataStorage.Transactions)
                {
                    if (transaction.WalletGuid == _wallet.Guid)
                    {
                        _transactions.Add(transaction);
                    }
                }
                return _transactions;
            }
        }
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

        public Transaction TransactionToEdit
        {
            get
            {
                return _transactionToEdit;
            }
            set
            {
                _transactionToEdit = value;
                OnPropertyChanged();
                EditTransactionCommand.RaiseCanExecuteChanged();
            }
        }
        public decimal TransactionAmount
        {
            get
            {
                return _newAmount;
            }
            set
            {
                _newAmount = value;
                OnPropertyChanged();
                EditTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTimeOffset TransactionDate
        {
            get
            {
                return _newDate;
            }
            set
            {
                _newDate = value;
                OnPropertyChanged();
                EditTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public string TransactionDescription
        {
            get
            {
                return _newDescription;
            }
            set
            {
                _newDescription = value;
                OnPropertyChanged();
                EditTransactionCommand.RaiseCanExecuteChanged();
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
                return _newCurrency;
            }
            set
            {
                _newCurrency = value;
                OnPropertyChanged();
                EditTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand EditTransactionCommand { get; }

        public DelegateCommand GoToMainTransactionsCommand { get; }

        public TransactionEditingViewModel(Action goToMainTransactions, Wallet wallet)
        {
            _wallet = wallet;

            _transactions = new List<Transaction>();
            foreach (var transaction in RuntimeDataStorage.Transactions)
            {
                if (transaction.WalletGuid == _wallet.Guid)
                {
                    _transactions.Add(transaction);
                }
            }

            _transactionsViewModels = new ObservableCollection<TransactionsDetailsViewModel>();
            foreach (var viewModel in RuntimeDataStorage.TransactionsViewModels)
            {
                if (viewModel.TransactionWalletGuid == _wallet.Guid)
                {
                    _transactionsViewModels.Add(viewModel);
                }
            }
            
            EditTransactionCommand = new DelegateCommand(EditTransaction, isEditingEnabled);
            _goToMainTransactions = goToMainTransactions;
            GoToMainTransactionsCommand = new DelegateCommand(_goToMainTransactions);
            _isEnabled = true;
            TransactionCurrency = Currencies.Currency.Dollar;
        }

        private async void EditTransaction()
        {
            try
            {
                IsEnabled = false;
                if (_transactionToEdit == null)
                {
                    throw new Exception("Transaction wasn't chosen");
                }
                else
                {
                    await TransactionsService.EditTransaction(_transactionToEdit, _wallet, _newAmount, _newDate, _newDescription, _newCurrency);
                    _transactions.RemoveAll(transaction => transaction.Guid == _transactionToEdit.Guid);
                    foreach (var transViewModel in _transactionsViewModels)
                    {
                        if (transViewModel.TransactionGuid == _transactionToEdit.Guid)
                        {
                            _transactionsViewModels.Remove(transViewModel);
                            break;
                        }
                    }

                    _transactions.Add(_transactionToEdit);
                    OnPropertyChanged(nameof(Transactions));
                    _transactionsViewModels.Add(new TransactionsDetailsViewModel(_transactionToEdit));

                    MessageBox.Show("Successfully edited transaction!");
                    _goToMainTransactions.Invoke();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't edit transaction! {e.Message}");
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private bool isEditingEnabled()
        {
            return (TransactionToEdit != null) && (TransactionCurrency != 0);
        }

        public TransactionNavigatableTypes Type
        {
            get
            {
                return TransactionNavigatableTypes.EditingTransaction;
            }
        }

        public void ClearSensitiveData()
        {
            TransactionToEdit = null;
            TransactionAmount = 0;
            TransactionDate = new DateTimeOffset();
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
