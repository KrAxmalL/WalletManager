using Models.Transactions;
using Models.Users;
using Models.Wallets;
using Pinkevych_WalletManager.WalletsWPF.Transactions;
using Pinkevych_WalletManager.WalletsWPF.Wallets;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.RuntimeStorage
{
    public class RuntimeDataStorage
    {
        private static User _currentUser;
        private static List<Wallet> _wallets;
        private static ObservableCollection<WalletsDetailsViewModel> _walletsViewModels;
        private static List<Transaction> _transactions;
        private static ObservableCollection<TransactionsDetailsViewModel> _transactionsViewModels;

        public static User CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }

        public static List<Wallet> Wallets
        {
            get
            {
                return _wallets;
            }
        }

        public static ObservableCollection<WalletsDetailsViewModel> WalletsViewModels
        {
            get
            {
                return _walletsViewModels;
            }
        }

        public static List<Transaction> Transactions
        {
            get
            {
                return _transactions;
            }
        }

        public static ObservableCollection<TransactionsDetailsViewModel> TransactionsViewModels
        {
            get
            {
                return _transactionsViewModels;
            }
        }
        public RuntimeDataStorage()
        {
            //todo - try to make loading async
            LoadAllData();
            //Thread.Sleep(3000);
        }

        private async Task LoadAllDataAsync()
        {
            _wallets = await WalletsService.GetWalletsAsync();
            foreach (var wallet in _wallets)
            {
                _walletsViewModels.Add(new WalletsDetailsViewModel(wallet));
            }
        }

        private void LoadAllData()
        {
            _currentUser = AuthentificationService.ActiveUser;

            _transactions = TransactionsService.GetTransactions();
            _transactionsViewModels = new ObservableCollection<TransactionsDetailsViewModel>();
            foreach (var transaction in _transactions)
            {
                _transactionsViewModels.Add(new TransactionsDetailsViewModel(transaction));
            }
            Trace.WriteLine("Transactions view models: " + _transactionsViewModels.Count);

            _wallets = WalletsService.GetWallets();
            _walletsViewModels = new ObservableCollection<WalletsDetailsViewModel>();
            foreach (var wallet in _wallets)
            {
                foreach(var transaction in _transactions)
                {
                    if(transaction.WalletGuid == wallet.Guid)
                    {
                        wallet.AddTransaction(transaction);
                    }
                }
                _walletsViewModels.Add(new WalletsDetailsViewModel(wallet));
            }

            
        }
    }
}
