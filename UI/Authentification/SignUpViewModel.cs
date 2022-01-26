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
    public class SignUpViewModel : INotifyPropertyChanged, INavigatable<AuthNavigatableTypes>
    {
        private RegistrationUser _regUser = new RegistrationUser();
        private Action _goToSignIn;

        public AuthNavigatableTypes Type
        {
            get
            {
                return AuthNavigatableTypes.SignUp;
            }
        }

        public String FirstName
        {
            get
            {
                return _regUser.FirstName;
            }

            set
            {
                if (_regUser.FirstName != value)
                {
                    _regUser.FirstName = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String LastName
        {
            get
            {
                return _regUser.LastName;
            }

            set
            {
                if (_regUser.LastName != value)
                {
                    _regUser.LastName = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String Email
        {
            get
            {
                return _regUser.Email;
            }

            set
            {
                if (_regUser.Email != value)
                {
                    _regUser.Email = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String Login
        {
            get
            {
                return _regUser.Login;
            }

            set
            {
                if (_regUser.Login != value)
                {
                    _regUser.Login = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }

            }
        }

        public String Password
        {
            get
            {
                return _regUser.Password;
            }

            set
            {
                if (_regUser.Password != value)
                {
                    _regUser.Password = value;
                    OnPropertyChanged();
                    SignUpCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand SignUpCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public DelegateCommand SignInCommand { get; }

        public SignUpViewModel(Action goToSignIn)
        {
            SignUpCommand = new DelegateCommand(SignUp, IsSignUpEnabled);
            CloseCommand = new DelegateCommand(() => Environment.Exit(0));
            _goToSignIn = goToSignIn;
            SignInCommand = new DelegateCommand(goToSignIn);
        }

        private bool IsSignUpEnabled()
        {
            return (!String.IsNullOrWhiteSpace(Login)) && (!String.IsNullOrWhiteSpace(Password) && !String.IsNullOrWhiteSpace(FirstName))
                    && (!String.IsNullOrWhiteSpace(LastName) && (!String.IsNullOrWhiteSpace(Email)));
        }

        private async void SignUp()
        {
            var authService = new AuthentificationService();
            try
            {
                await authService.RegisterUser(_regUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign up failed! {ex.Message}");
                return;
            }
            MessageBox.Show($"User successfully registered! Please, sign in!");
            _goToSignIn.Invoke();
        }

        public void ClearSensitiveData()
        {
            _regUser = new RegistrationUser();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
