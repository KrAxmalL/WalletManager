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
    public class TransactionDeletingViewModel : INotifyPropertyChanged, INavigatable<TransactionNavigatableTypes>
    {

        private Transaction _transactionToDelete;
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
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public Transaction TransactionToDelete
        {
            get
            {
                return _transactionToDelete;
            }
            set
            {
                _transactionToDelete = value;
                OnPropertyChanged(nameof(TransactionToDelete));
                DeleteTransactionCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand DeleteTransactionCommand { get; }

        public DelegateCommand GoToMainTransactionsCommand { get; }

        public TransactionDeletingViewModel(Action goToMainTransactions, ref Wallet wallet)
        {
            _wallet = wallet;
            _transactionToDelete = null;

            _transactions = new List<Transaction>();           
            foreach (var transaction in RuntimeDataStorage.Transactions)
            {
                if(transaction.WalletGuid == _wallet.Guid)
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
            DeleteTransactionCommand = new DelegateCommand(DeleteTransaction, isDeletingEnabled);
            _goToMainTransactions = goToMainTransactions;
            GoToMainTransactionsCommand = new DelegateCommand(_goToMainTransactions);
            _isEnabled = true;
        }

        private async void DeleteTransaction()
        {
            try
            {
                IsEnabled = false;
                if (_transactionToDelete == null)
                {
                    throw new Exception("Transaction wasn't chosen");
                }
                else
                {
                    _wallet.RemoveTransaction(_transactionToDelete);
                    await TransactionsService.DeleteTransaction(_transactionToDelete);
                    _transactions.Remove(_transactionToDelete);

                    var _viewModelToDelete = _transactionsViewModels.FirstOrDefault(trans => trans.TransactionGuid == _transactionToDelete.Guid);
                    if(_viewModelToDelete != null)
                    {
                        _transactionsViewModels.Remove(_viewModelToDelete);
                    }

                    var runtSt = RuntimeDataStorage.Transactions;
                    var runtTr = runtSt.FirstOrDefault(tr => tr.Guid == _transactionToDelete.Guid);
                    if(runtTr != null)
                    {
                        runtSt.Remove(runtTr);
                    }

                    var runtStVm = RuntimeDataStorage.TransactionsViewModels;
                    var runtTrVm = runtStVm.FirstOrDefault(tr => tr.TransactionGuid == _transactionToDelete.Guid);
                    if (runtTrVm != null)
                    {
                        runtStVm.Remove(runtTrVm);
                    }

                    MessageBox.Show("Successfully deleted transaction!");
                    _goToMainTransactions.Invoke();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't delete transaction! {e.Message}");
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private bool isDeletingEnabled()
        {
            return (_transactionToDelete != null);
        }

        public TransactionNavigatableTypes Type
        {
            get
            {
                return TransactionNavigatableTypes.DeletingTransaction;
            }
        }

        public void ClearSensitiveData()
        {
            TransactionToDelete = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
