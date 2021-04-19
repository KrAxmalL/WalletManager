using Models.Wallets;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Pinkevych_WalletManager.WalletsWPF.RuntimeStorage;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Pinkevych_WalletManager.WalletsWPF.Transactions
{
    public class TransactionsViewModel : BindableBase, INavigatable<TransactionNavigatableTypes>
    {
        private TransactionsDetailsViewModel _currentTransaction;
        public static ObservableCollection<TransactionsDetailsViewModel> _transactions;
        private Wallet _wallet;

        private Action _goToAddingTransaction;
        private Action _goToEditingTransaction;
        private Action _goToDeletingTransaction;
        private Action _goToWallets;

        public string CurrentWalletString
        {
            get
            {
                return $"Current wallet: {_wallet.Name}";
            }
        }


        public ObservableCollection<TransactionsDetailsViewModel> Transactions
        {
            get
            {
                var transactionsOfWallet = RuntimeDataStorage.TransactionsViewModels;
                foreach (var transaction in transactionsOfWallet)
                {
                    Trace.WriteLine("Current transaction wallet id: " + transaction.TransactionWalletGuid);
                    Trace.WriteLine("Current wallet id: " + _wallet.Guid);
                    if (transaction.TransactionWalletGuid == _wallet.Guid && !_transactions.Contains(transaction))
                    {
                        _transactions.Add(transaction);
                    }
                }
                return _transactions;
            }

            set
            {
                _transactions = value;
                RaisePropertyChanged(nameof(Transactions));
            }
        }

        public TransactionsDetailsViewModel CurrentTransaction
        {
            get
            {
                return _currentTransaction;
            }

            set
            {
                _currentTransaction = value;
                RaisePropertyChanged(nameof(CurrentTransaction));
            }
        }

        public DelegateCommand GoToAddingTransactionCommand { get; }

        public DelegateCommand GoToEditingTransactionCommand { get; }

        public DelegateCommand GoToDeletingTransactionCommand { get; }

        public DelegateCommand GoToWalletsCommand { get; }

        public TransactionsViewModel(Action goToAddingTransaction, Action goToEditingTransaction, Action goToDeletingTransaction, Action goToWallets, Wallet wallet)
        {
            _goToAddingTransaction = goToAddingTransaction;
            _goToEditingTransaction = goToEditingTransaction;
            _goToDeletingTransaction = goToDeletingTransaction;
            _goToWallets = goToWallets;
            GoToAddingTransactionCommand = new DelegateCommand(_goToAddingTransaction);
            GoToEditingTransactionCommand = new DelegateCommand(_goToEditingTransaction);
            GoToDeletingTransactionCommand = new DelegateCommand(_goToDeletingTransaction);
            GoToWalletsCommand = new DelegateCommand(_goToWallets);

            _wallet = wallet;

            _transactions = new ObservableCollection<TransactionsDetailsViewModel>();

            var transactionsOfWallet = RuntimeDataStorage.Transactions;
            foreach (var transaction in transactionsOfWallet)
            {
                Trace.WriteLine("Current transaction wallet id: " + transaction.WalletGuid);
                Trace.WriteLine("Current wallet id: " + _wallet.Guid);
                if (transaction.WalletGuid == _wallet.Guid)
                {
                    _transactions.Add(new TransactionsDetailsViewModel(transaction));
                }
            }
        }

        public TransactionNavigatableTypes Type
        {
            get
            {
                return TransactionNavigatableTypes.MainTransactions;
            }
        }

        public void ClearSensitiveData()
        {          
            if (_transactions != null)
            {
                Trace.WriteLine("Transactions in main transactions view: " + _transactions.Count);
            }
            else
            {
                Trace.WriteLine("Transactions in main transactions view: null");
            }
        }

    }
}
