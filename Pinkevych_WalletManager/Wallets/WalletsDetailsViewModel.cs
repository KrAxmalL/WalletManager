using Models;
using Models.Wallets;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Wallets
{
    public class WalletsDetailsViewModel : BindableBase
    {
        private Wallet _wallet;

        public string Name
        {
            get
            {
                return _wallet.Name;
            }

            set
            {
                _wallet.Name = value;
                RaisePropertyChanged(nameof(DisplayName));
            }
        }

        public string Description
        {
            get
            {
                return _wallet.Description;
            }

            set
            {
                _wallet.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public decimal CurrentBalance
        {
            get
            {
                return _wallet.CurrentBalance;
            }

            set
            {
                _wallet.CurrentBalance = value;
                RaisePropertyChanged(nameof(DisplayName));
                RaisePropertyChanged(nameof(DisplayBalance));
            }
        }

        public string DisplayName
        {
            get
            {
                return $"{_wallet.Name}, {_wallet.CurrentBalance}{Currencies.currencyToSymbol(_wallet.Currency)}";
            }
        }

        public string DisplayBalance
        {
            get
            {
                return $"{CurrentBalance} {Currencies.currencyToSymbol(_wallet.Currency)}";
            }
        }

        public string DisplayIncome
        {
            get
            {
                return $"{_wallet.GetIncome()} {Currencies.currencyToSymbol(_wallet.Currency)}";
            }
        }

        public string DisplaySpending
        {
            get
            {
                return $"{_wallet.GetSpending()} {Currencies.currencyToSymbol(_wallet.Currency)}";
            }
        }

        public Guid WalletGuid
        {
            get
            {
                return _wallet.Guid;
            }
        }


        public WalletsDetailsViewModel(Wallet current)
        {
            _wallet = current;
        }
    }
}
