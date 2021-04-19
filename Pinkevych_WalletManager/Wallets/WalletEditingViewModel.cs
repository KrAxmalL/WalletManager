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
    public class WalletEditingViewModel : INotifyPropertyChanged, INavigatable<WalletNavigatableTypes>
    {
        private Wallet _walletToEdit;
        private string _newName;
        private string _newDescription;
        private Currencies.Currency _newCurrency;
        private List<Wallet> _wallets;
        private ObservableCollection<WalletsDetailsViewModel> _walletsViewModels;

        private Action _goToMainWallets;

        private bool _isEnabled;

        public List<Wallet> Wallets
        {
            get
            {
                return _wallets;
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

        public Wallet WalletToEdit
        {
            get
            {
                return _walletToEdit;
            }
            set
            {
                _walletToEdit = value;
                OnPropertyChanged();
                EditWalletCommand.RaiseCanExecuteChanged();
            }
        }
        public string WalletName
        {
            get
            {
                return _newName;
            }
            set
            {
                _newName = value;
                OnPropertyChanged();
                EditWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public string WalletDescription
        {
            get
            {
                return _newDescription;
            }
            set
            {
                _newDescription = value;
                OnPropertyChanged();
                EditWalletCommand.RaiseCanExecuteChanged();
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
                return _newCurrency;
            }
            set
            {
                _newCurrency = value;
                OnPropertyChanged();
                EditWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand EditWalletCommand { get; }

        public DelegateCommand GoToMainWalletsCommand { get; }

        public WalletEditingViewModel(Action goToMainWallets)
        {
            _wallets = RuntimeDataStorage.Wallets;
            _walletsViewModels = RuntimeDataStorage.WalletsViewModels;
            EditWalletCommand = new DelegateCommand(EditWallet, isEditingEnabled);
            _goToMainWallets = goToMainWallets;
            GoToMainWalletsCommand = new DelegateCommand(_goToMainWallets);
            _isEnabled = true;
            Trace.WriteLine(_wallets.Count);
        }

        private async void EditWallet()
        {
            try
            {
                IsEnabled = false;
                if (_walletToEdit == null)
                {
                    throw new Exception("Wallet wasn't chosen");
                }
                else
                {
                    await WalletsService.EditWallet(_walletToEdit, _newName, _newDescription, _newCurrency);
                    string oldName = _walletToEdit.Name;
                    _wallets.RemoveAll(wallet => wallet.Name == oldName);
                    foreach (var wallViewModel in _walletsViewModels)
                    {
                        if (wallViewModel.Name == oldName)
                        {
                            _walletsViewModels.Remove(wallViewModel);
                            break;
                        }
                    }
                    Trace.WriteLine("Editable guid: " + _walletToEdit.Guid.ToString());
                    _wallets.Add(_walletToEdit);
                    OnPropertyChanged(nameof(Wallets));
                    _walletsViewModels.Add(new WalletsDetailsViewModel(_walletToEdit));
                    Trace.WriteLine(_walletsViewModels.Count);
                    MessageBox.Show("Successfully edited wallet!");
                    _goToMainWallets.Invoke();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't edit wallet! {e.Message}");
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private bool isEditingEnabled()
        {
            return (!String.IsNullOrWhiteSpace(WalletName) && (WalletToEdit != null));
        }

        public WalletNavigatableTypes Type
        {
            get
            {
                return WalletNavigatableTypes.EditingWallet;
            }
        }

        public void ClearSensitiveData()
        {
            WalletToEdit = null;
            WalletName = "";
            WalletDescription = "";
            WalletCurrency = Currencies.Currency.Dollar;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
