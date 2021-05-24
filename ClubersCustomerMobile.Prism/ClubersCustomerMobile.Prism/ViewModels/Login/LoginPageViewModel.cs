using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Interfaces;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private readonly IRegexHelper _regexHelper;
        private string _password;
        private bool _isEnabled;
        private bool _isRemember;
        private string _emailIcon = IconFont.Email;
        private string _passwordIcon = IconFont.Lock;
        private DelegateCommand _loginCommand;
        private DelegateCommand _registerCommand;
        private DelegateCommand _forgotPasswordCommand;
        public LoginPageViewModel(INavigationService navigationService, IApiService apiService,
                                  IRegexHelper regexHelper,
                                  IPageDialogService dialogService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _regexHelper = regexHelper;
            Title = "Inicio";
            IsRemember = true;
            Password = ".Clubers20";
            Email = "victor.ibarra@lab.com";

        }
        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPasswordAsync));
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));
        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));

        public string EmailIcon
        {
            get => _emailIcon;
            set => SetProperty(ref _emailIcon, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public string PasswordIcon
        {
            get => _passwordIcon;
            set => SetProperty(ref _passwordIcon, value);
        }
        public string Email { get; set; }
        public bool IsRemember
        {
            get => _isRemember;
            set => SetProperty(ref _isRemember, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email) || !_regexHelper.IsValidEmail(Email))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar un correo.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar una contraseña.", "Aceptar");
                return;
            }

            if (Password.Length < 8)
            {
                await _dialogService.DisplayAlertAsync("Error", "La contraseña debe contener al menos 8 carácteres", "Aceptar");
                return;
            }


            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Compruebe la conexión a Internet", "Aceptar");
                return;
            }

            TokenRequest request = new TokenRequest
            {
                password = Password,
                email = Email


            };

            Response response = await _apiService.GetTokenAsync(Constants.urlBase, "api/Account", "/token", request);

            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Email o contraseña incorrectos", "Aceptar");
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            /*EmailRequest emailRequest = new EmailRequest
            {
                //CultureInfo = Languages.Culture,
                Email = Email
            };*/

            //Response response2 = await _apiService.GetUserByEmail(url, "api", "/Account/GetUserByEmail", "bearer", token.Token, emailRequest);
            //Response response2 = await _apiService.GetUserById(url, "api", "/Account/GetByIdAsync", "bearer", token.Result.token);
            Response response2 = await _apiService.GetUserByEmailAsync(Constants.urlBase, Constants.servicePrefix, Constants.GetUserByEmailAsyncController, Email, Constants.tokenType, token.Result.token);

            UserResponse userResponse = (UserResponse)response2.Result;

            Settings.User = JsonConvert.SerializeObject(userResponse);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;

            IsRunning = false;
            IsEnabled = true;

            //await _navigationService.NavigateAsync("/MasterDetailPage/NavigationPage/HomePage");

            await _navigationService.NavigateAsync("/NavigationPage/CustomerTabbedPage?selectedTab=HomePage");
            Password = string.Empty;
            Settings.IsRemembered = IsRemember;
        }

        private async void ForgotPasswordAsync()
        {
            await _navigationService.NavigateAsync(nameof(RecoverPasswordPage));
        }

        private async void RegisterAsync()
        {
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }
    }
}
