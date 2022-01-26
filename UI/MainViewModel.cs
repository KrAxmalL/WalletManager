using Pinkevych_WalletManager.WalletsWPF.Authentification;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Pinkevych_WalletManager.WalletsWPF.Transactions;
using Pinkevych_WalletManager.WalletsWPF.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych.FinanceManager.FinanceWPF
{
    public class MainViewModel : NavigationBase<MainNavigatableTypes>
    {
        public MainViewModel()
        {
            Navigate(MainNavigatableTypes.Auth);
        }

        protected override INavigatable<MainNavigatableTypes> CreateViewModel(MainNavigatableTypes type)
        {
            switch (type)
            {
                case MainNavigatableTypes.Auth: return new AuthViewModel(() => Navigate(MainNavigatableTypes.Wallets));

                case MainNavigatableTypes.Wallets: return new GeneralWalletsViewModel(() => Navigate(MainNavigatableTypes.Transactions));

                case MainNavigatableTypes.Transactions: return new GeneralTransactionsViewModel(() => Navigate(MainNavigatableTypes.Wallets));

                default: return null;
            }

        }
    }
}
