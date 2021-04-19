using Models.Users;
using Models.Wallets;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Pinkevych_WalletManager.WalletsWPF.RuntimeStorage;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Transactions
{
    public class GeneralTransactionsViewModel : NavigationBase<TransactionNavigatableTypes>, INavigatable<MainNavigatableTypes>
    {
        private RuntimeDataStorage _mainStorage;
        private User _currentActiveUser;
        private Wallet _currentWallet;

        private Dictionary<Wallet, List<INavigatable<TransactionNavigatableTypes>>> dict;

        private Action _goToWallets;

        public GeneralTransactionsViewModel(Action goToWallets)
        {
            _goToWallets = goToWallets;
            _mainStorage = new RuntimeDataStorage();
            _currentActiveUser = AuthentificationService.ActiveUser;
            dict = new Dictionary<Wallet, List<INavigatable<TransactionNavigatableTypes>>>();
            Navigate(TransactionNavigatableTypes.WalletToTransaction);
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
        protected override INavigatable<TransactionNavigatableTypes> CreateViewModel(TransactionNavigatableTypes type)
        {
            switch (type)
            {
                case TransactionNavigatableTypes.MainTransactions:
                    return new TransactionsViewModel(() => Navigate(TransactionNavigatableTypes.AddingTransaction),
                                                     () => Navigate(TransactionNavigatableTypes.EditingTransaction),
                                                     () => Navigate(TransactionNavigatableTypes.DeletingTransaction),
                                                     _goToWallets, _currentWallet);
                    
                case TransactionNavigatableTypes.AddingTransaction: return new TransactionAddingViewModel(() => Navigate(TransactionNavigatableTypes.MainTransactions), _currentWallet);

                case TransactionNavigatableTypes.EditingTransaction: return new TransactionEditingViewModel(/*() => Navigate(TransactionNavigatableTypes.MainTransactions), _currentWallet*/);

                case TransactionNavigatableTypes.DeletingTransaction: return new TransactionDeletingViewModel(/*() => Navigate(TransactionNavigatableTypes.MainTransactions), _currentWallet*/);

                case TransactionNavigatableTypes.WalletToTransaction: return new WalletToTransactionViewModel(_goToWallets, () => Navigate(TransactionNavigatableTypes.MainTransactions));

                default: return null;
            }
        }

        //needed to get a wallet for transactions view
        private Wallet getChosenWallet()
        {
            var viewModel = _viewModels.FirstOrDefault(viewModel => viewModel.Type == TransactionNavigatableTypes.WalletToTransaction);
            return ((WalletToTransactionViewModel)viewModel).CurrentWallet;
        }

        public MainNavigatableTypes Type
        {
            get
            {
                return MainNavigatableTypes.Transactions;
            }
        }


        public override void Navigate(TransactionNavigatableTypes type)
        { 
            if (type == TransactionNavigatableTypes.WalletToTransaction)
            {
                base.Navigate(type);
            }
            else
            {
                INavigatable<TransactionNavigatableTypes> currViewModel = null;
                _currentWallet = getChosenWallet();
                if (!dict.ContainsKey(_currentWallet)) 
                {
                   
                   dict.Add(_currentWallet, new List<INavigatable<TransactionNavigatableTypes>>());
                   currViewModel = CreateViewModel(type);
                   dict[_currentWallet].Add(currViewModel);
                }
                else
                {
                    currViewModel = dict[_currentWallet].FirstOrDefault(viewModel => viewModel.Type == type);
                    if(currViewModel == null)
                    {
                        currViewModel = CreateViewModel(type);
                        dict[_currentWallet].Add(CreateViewModel(type));
                    } 
                }
                CurrentViewModel = currViewModel;
                RaisePropertyChanged(nameof(CurrentViewModel));
            }
        }
        public void ClearSensitiveData()
        {
            Navigate(TransactionNavigatableTypes.WalletToTransaction);
            CurrentViewModel.ClearSensitiveData();
        }
    }
}
