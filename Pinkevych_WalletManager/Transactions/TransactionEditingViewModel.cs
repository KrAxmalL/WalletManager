using Pinkevych_WalletManager.WalletsWPF.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Transactions
{
    public class TransactionEditingViewModel : INavigatable<TransactionNavigatableTypes>
    {
        public TransactionNavigatableTypes Type
        {
            get
            {
                return TransactionNavigatableTypes.EditingTransaction;
            }
        }

        public void ClearSensitiveData()
        {

        }
    }
}
