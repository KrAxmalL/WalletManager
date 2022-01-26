using Models;
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

namespace Pinkevych_WalletManager.WalletsWPF.Wallets
{
    public class WalletAddingViewModel : INotifyPropertyChanged, INavigatable<WalletNavigatableTypes>
    {
        private Wallet _walletToAdd = new Wallet(new Guid(), "", "", Currencies.Currency.Dollar, 0, 0, AuthentificationService.ActiveUser.Guid);
        private List<Wallet> _wallets;
        private ObservableCollection<WalletsDetailsViewModel> _walletsViewModels;

        private Action _goToMainWallets;

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
        public string WalletName
        {
            get
            {
                return _walletToAdd.Name;
            }
            set
            {
                _walletToAdd.Name = value;
                OnPropertyChanged();
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public string WalletDescription
        {
            get
            {
                return _walletToAdd.Description;
            }
            set
            {
                _walletToAdd.Description = value;
                OnPropertyChanged();
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public decimal WalletBalance
        {
            get
            {
                return _walletToAdd.CurrentBalance;
            }
            set
            {
                _walletToAdd.CurrentBalance = value;
                OnPropertyChanged();
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public string WalletDisplayCurrency
        {
            get
            {
                return Currencies.currencyToString(WalletCurrency);
            }
        }

        public Currencies.Currency WalletCurrency
        {
            get
            {
                return _walletToAdd.Currency;
            }
            set
            {
                _walletToAdd.Currency = value;
                OnPropertyChanged();
                AddWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand AddWalletCommand { get; }

        public DelegateCommand GoToMainWalletsCommand { get; }

        public WalletAddingViewModel(Action goToMainWallets)
        {
            _wallets = RuntimeDataStorage.Wallets;
            _walletsViewModels = RuntimeDataStorage.WalletsViewModels;
            AddWalletCommand = new DelegateCommand(AddWalletToCollection, isAddingEnabled);
            _goToMainWallets = goToMainWallets;
            GoToMainWalletsCommand = new DelegateCommand(_goToMainWallets);
            _isEnabled = true;
        }

        private async void AddWalletToCollection()
        {
            if (_walletToAdd.CurrentBalance < 0)
            {
                MessageBox.Show("Balance can't be less than 0!");
            }
            else
            {
                try
                {
                    IsEnabled = false;
                    await WalletsService.AddWallet(_walletToAdd);
                    _wallets.Add(_walletToAdd);
                    _walletsViewModels.Add(new WalletsDetailsViewModel(_walletToAdd));
                    _walletToAdd = new Wallet(new Guid(), "", "", Currencies.Currency.Dollar, 0, 0, RuntimeDataStorage.CurrentUser.Guid);

                    MessageBox.Show("Successfully added new wallet!");
                    _goToMainWallets.Invoke();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Can't add wallet! {e.Message}");
                }
                finally
                {
                    IsEnabled = true;
                }
            }
        }

        private bool isAddingEnabled()
        {
            return (!String.IsNullOrWhiteSpace(WalletName) && !String.IsNullOrWhiteSpace(WalletBalance.ToString()));
        }

        public WalletNavigatableTypes Type
        {
            get
            {
                return WalletNavigatableTypes.AddingWallet;
            }
        }

        public void ClearSensitiveData()
        {
            _wallets = RuntimeDataStorage.Wallets;
            _walletsViewModels = RuntimeDataStorage.WalletsViewModels;
            WalletName = "";
            WalletDescription = "";
            WalletBalance = 0;
            WalletCurrency = Currencies.Currency.Dollar;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
