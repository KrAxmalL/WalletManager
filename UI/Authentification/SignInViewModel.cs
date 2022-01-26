using Models.Users;
using Pinkevych_WalletManager.WalletsWPF.Navigation;
using Prism.Commands;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pinkevych_WalletManager.WalletsWPF.Authentification
{
    public class SignInViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableTypes>
    {
        private AuthUser _authUser = new AuthUser();
        private Action _goToSignUp;
        private Action _goToWallets;
        private bool _isEnabled;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public AuthNavigatableTypes Type
        {
            get
            {
                return AuthNavigatableTypes.SignIn;
            }
        }

        public String Login
        {
            get
            {
                return _authUser.Login;
            }

            set
            {
                if (_authUser.Login != value)
                {
                    _authUser.Login = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
                }

            }
        }

        public String Password
        {
            get
            {
                return _authUser.Password;
            }

            set
            {
                if (_authUser.Password != value)
                {
                    _authUser.Password = value;
                    OnPropertyChanged();
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand SignInCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public DelegateCommand SignUpCommand { get; }


        public SignInViewModel(Action goToSignUp, Action goToWallets)
        {
            SignInCommand = new DelegateCommand(SignIn, IsSignInEnabled);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));
            _goToSignUp = goToSignUp;
            _goToWallets = goToWallets;
            SignUpCommand = new DelegateCommand(_goToSignUp);
            _isEnabled = true;
        }

        private bool IsSignInEnabled()
        {
            return (!String.IsNullOrWhiteSpace(Login)) && (!String.IsNullOrWhiteSpace(Password));
        }

        private async void SignIn()
        {
            if (String.IsNullOrWhiteSpace(Login) || String.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Login or password is empty!");
            }
            else
            {
                var authService = new AuthentificationService();
                User user = null;
                try
                {
                    IsEnabled = false;
                    user = await authService.AuthentificateUser(_authUser);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sign in failed! {ex.Message}");
                    return;
                }
                finally
                {
                    IsEnabled = true;
                }
                MessageBox.Show($"You successfully signed in! User: {user.FullName}, {user.Email}");
                _goToWallets.Invoke();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearSensitiveData()
        {
            Password = "";
        }
    }
}
