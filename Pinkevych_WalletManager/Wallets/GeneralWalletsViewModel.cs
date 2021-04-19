using Models.Users;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Pinkevych_WalletManager.WalletsWPF.RuntimeStorage;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Wallets
{
    public class GeneralWalletsViewModel : NavigationBase<WalletNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        private RuntimeDataStorage _mainStorage;
        private User _currentActiveUser;

        private Action _goToTransactions;

        public GeneralWalletsViewModel(Action goToTransactions)
        {
            _mainStorage = new RuntimeDataStorage();
            _currentActiveUser = AuthentificationService.ActiveUser;

            _goToTransactions = goToTransactions;
            Navigate(WalletNavigatableTypes.MainWallets);
        }

        /*private void LoadData()
        {
            _userWallets = WalletsService.GetWalletsNotAsync();
            _walletsViewModels = new ObservableCollection<WalletsDetailsViewModel>();
            foreach (var wallet in _userWallets)
            {
                _walletsViewModels.Add(new WalletsDetailsViewModel(wallet));
            }
        }
        private async void LoadDataAsync()
        {
            _userWallets = await WalletsService.GetWalletsAsync();
            _walletsViewModels = new ObservableCollection<WalletsDetailsViewModel>();
            foreach (var wallet in _userWallets)
            {
                _walletsViewModels.Add(new WalletsDetailsViewModel(wallet));
            }
            Trace.WriteLine(_walletsViewModels.Count);
            //Navigate(WalletNavigatableTypes.MainWallets);
        }*/

        protected override INavigatable<WalletNavigatableTypes> CreateViewModel(WalletNavigatableTypes type)
        {
            switch (type)
            {
                case WalletNavigatableTypes.AddingWallet: return new WalletAddingViewModel(() => Navigate(WalletNavigatableTypes.MainWallets));

                case WalletNavigatableTypes.EditingWallet: return new WalletEditingViewModel(() => Navigate(WalletNavigatableTypes.MainWallets));

                case WalletNavigatableTypes.DeletingWallet: return new WalletDeletingViewModel(() => Navigate(WalletNavigatableTypes.MainWallets));

                case WalletNavigatableTypes.MainWallets:
                    return new WalletsViewModel(() => Navigate(WalletNavigatableTypes.AddingWallet), () => Navigate(WalletNavigatableTypes.EditingWallet), () => Navigate(WalletNavigatableTypes.DeletingWallet),
                                                _goToTransactions);

                default: return null;
            }
        }


        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.Wallets;
            }
        }

        public void ClearSensitiveData()
        {
            CurrentViewModel.ClearSensitiveData();
        }
    }
}
