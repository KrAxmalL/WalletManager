using Pinkevych_WalletManager.WalletsWPF.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Transactions
{
    public class TransactionDeletingViewModel : INavigatable<TransactionNavigatableTypes>
    {
        public TransactionNavigatableTypes Type
        {
            get
            {
                return TransactionNavigatableTypes.DeletingTransaction;
            }
        }

        public void ClearSensitiveData()
        {

        }
    }
}
