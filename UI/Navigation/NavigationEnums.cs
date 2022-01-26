using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Navigation
{
    public enum AuthNavigatableTypes
    {
        SignIn,
        SignUp
    }

    public enum MainNavigatableTypes
    {
        Auth,
        Wallets,
        Transactions
    }

    public enum WalletNavigatableTypes
    {
        MainWallets,
        AddingWallet,
        EditingWallet,
        DeletingWallet,
    }

    public enum TransactionNavigatableTypes
    {
        WalletToTransaction,
        MainTransactions,
        AddingTransaction,
        EditingTransaction,
        DeletingTransaction
    }
}
