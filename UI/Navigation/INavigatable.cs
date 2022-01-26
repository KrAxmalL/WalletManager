using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Navigation
{
    public interface INavigatable<TObject> where TObject : Enum
    {
        public TObject Type { get; }

        public void ClearSensitiveData();
    }
}
