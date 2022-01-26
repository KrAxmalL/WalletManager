using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinkevych_WalletManager.WalletsWPF.Navigation
{
    public abstract class NavigationBase<TObject> : BindableBase where TObject : Enum
    {
        protected List<INavigatable<TObject>> _viewModels = new List<INavigatable<TObject>>();

        public INavigatable<TObject> CurrentViewModel
        {
            get;
            protected set;
        }

        protected NavigationBase() { }

        public virtual void Navigate(TObject type)
        {
            if (CurrentViewModel != null && CurrentViewModel.Type.Equals(type))
            {
                return;
            }

            INavigatable<TObject> viewModel = _viewModels.FirstOrDefault(mainNavigatable => mainNavigatable.Type.Equals(type));

            if (viewModel == null)
            {
                viewModel = CreateViewModel(type);
                _viewModels.Add(viewModel);
            }

            viewModel.ClearSensitiveData();
            CurrentViewModel = viewModel;
            RaisePropertyChanged(nameof(CurrentViewModel));
        }

        protected abstract INavigatable<TObject> CreateViewModel(TObject type);
    }
}
