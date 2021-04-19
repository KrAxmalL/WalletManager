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
    public class WalletDeletingViewModel : INotifyPropertyChanged, INavigatable<WalletNavigatableTypes>
    {
        private Wallet _walletToDelete;
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

        public Wallet WalletToDelete
        {
            get
            {
                return _walletToDelete;
            }
            set
            {
                _walletToDelete = value;
                OnPropertyChanged();
                DeleteWalletCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand DeleteWalletCommand { get; }

        public DelegateCommand GoToMainWalletsCommand { get; }

        public WalletDeletingViewModel(Action goToMainWallets)
        {
            _wallets = RuntimeDataStorage.Wallets;
            _walletsViewModels = RuntimeDataStorage.WalletsViewModels;
            DeleteWalletCommand = new DelegateCommand(DeleteWallet, isDeletingEnabled);
            _goToMainWallets = goToMainWallets;
            GoToMainWalletsCommand = new DelegateCommand(_goToMainWallets);
            _isEnabled = true;
            Trace.WriteLine(_wallets.Count);
        }

        private async void DeleteWallet()
        {
            try
            {
                IsEnabled = false;
                if (_walletToDelete == null)
                {
                    throw new Exception("Wallet wasn't chosen");
                }
                else
                {
                    await WalletsService.DeleteWallet(_walletToDelete);
                    _wallets.RemoveAll(wallet => wallet.Name == _walletToDelete.Name);
                    foreach (var wallViewModel in _walletsViewModels)
                    {
                        if (wallViewModel.Name == _walletToDelete.Name)
                        {
                            _walletsViewModels.Remove(wallViewModel);
                            break;
                        }
                    }
                    Trace.WriteLine("Deleting guid: " + _walletToDelete.Guid.ToString());
                    Trace.WriteLine(_walletsViewModels.Count);
                    MessageBox.Show("Successfully deleted wallet!");
                    _goToMainWallets.Invoke();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Can't delete wallet! {e.Message}");
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private bool isDeletingEnabled()
        {
            return (WalletToDelete != null);
        }

        public WalletNavigatableTypes Type
        {
            get
            {
                return WalletNavigatableTypes.DeletingWallet;
            }
        }

        public void ClearSensitiveData()
        {
            WalletToDelete = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
