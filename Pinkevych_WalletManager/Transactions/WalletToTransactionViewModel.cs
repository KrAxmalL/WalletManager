using Models.Wallets;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Pinkevych_WalletManager.WalletsWPF.RuntimeStorage;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Transactions
{
    public class WalletToTransactionViewModel : BindableBase, INavigatable<TransactionNavigatableTypes>
    {
        private List<Wallet> _wallets;
        private Wallet _currentWallet;

        private Action _goToWallets;
        private Action _goToTransactions;

        public List<Wallet> Wallets
        {
            get
            {
                return _wallets;
            }
        }

        public Wallet CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                GoToTransactionsCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand GoToWalletsCommand { get; }

        public DelegateCommand GoToTransactionsCommand { get; }

        public WalletToTransactionViewModel(Action goToWallets, Action goToTransactions)
        {
            _goToWallets = goToWallets;
            _goToTransactions = goToTransactions;
            GoToWalletsCommand = new DelegateCommand(_goToWallets);
            GoToTransactionsCommand = new DelegateCommand(_goToTransactions, CanGoToTransactions);
            _currentWallet = null;
            _wallets = RuntimeDataStorage.Wallets;
        }

        private bool CanGoToTransactions()
        {
            return (_currentWallet != null);
        }

        public TransactionNavigatableTypes Type
        {
            get
            {
                return TransactionNavigatableTypes.WalletToTransaction;
            }
        }

        public void ClearSensitiveData()
        {
            _currentWallet = null;
        }
    }
}
