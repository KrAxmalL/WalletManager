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

namespace Pinkevych_WalletManager.WalletsWPF.Wallets
{
    public class WalletsViewModel : BindableBase, INavigatable<WalletNavigatableTypes>
    {
        private WalletsDetailsViewModel _currentWallet;
        private ObservableCollection<WalletsDetailsViewModel> _wallets;

        private Action _goToAddingWallet;
        private Action _goToEditingWallet;
        private Action _goToDeletingWallet;
        private Action _goToTransactionsChoosing;

        public ObservableCollection<WalletsDetailsViewModel> Wallets
        {
            get
            {
                return _wallets;
            }

            set
            {
                _wallets = value;
                RaisePropertyChanged(nameof(Wallets));
            }
        }

        public WalletsDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }

            set
            {
                _currentWallet = value;
                RaisePropertyChanged(nameof(CurrentWallet));
            }
        }

        public DelegateCommand GoToAddingWalletCommand { get; }

        public DelegateCommand GoToEditingWalletCommand { get; }

        public DelegateCommand GoToDeletingWalletCommand { get; }

        public DelegateCommand GoToTransactionsChoosingCommand { get; }

        public WalletsViewModel(Action goToAddingWallet, Action goToEditingWallet, Action goToDeletingWallet, Action goToTransactionsChoosing)
        {
            _goToAddingWallet = goToAddingWallet;
            _goToEditingWallet = goToEditingWallet;
            _goToDeletingWallet = goToDeletingWallet;
            _goToTransactionsChoosing = goToTransactionsChoosing;
            GoToAddingWalletCommand = new DelegateCommand(_goToAddingWallet);
            GoToEditingWalletCommand = new DelegateCommand(_goToEditingWallet);
            GoToDeletingWalletCommand = new DelegateCommand(_goToDeletingWallet);
            GoToTransactionsChoosingCommand = new DelegateCommand(_goToTransactionsChoosing);
            Wallets = RuntimeDataStorage.WalletsViewModels;
        }

        public WalletNavigatableTypes Type
        {
            get
            {
                return WalletNavigatableTypes.MainWallets;
            }
        }

        public void ClearSensitiveData()
        {
            if (_wallets != null)
            {
                Trace.WriteLine("Wallets in main wallets view: " + _wallets.Count);
            }
            else
            {
                Trace.WriteLine("Wallets in main wallets view: null");
            }
        }
    }
}
