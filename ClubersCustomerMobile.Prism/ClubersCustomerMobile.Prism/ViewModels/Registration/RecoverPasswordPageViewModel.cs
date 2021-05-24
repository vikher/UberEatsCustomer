using ClubersCustomerMobile.Prism.Interfaces;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubersCustomerMobile.Prism.ViewModels
{
   
    public class RecoverPasswordPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IRegexHelper _regexHelper;
        private readonly IApiService _apiService;
        private bool _isEnabled;
        private DelegateCommand _recoverCommand;
        public RecoverPasswordPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IRegexHelper regexHelper,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            Title = "Recuperar Contraseña";
            IsEnabled = true;
        }
        public DelegateCommand RecoverCommand => _recoverCommand ?? (_recoverCommand = new DelegateCommand(RecoverAsync));
        public string Email { get; set; }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        private async Task<bool> ValidateDataAsync()
        {

            if (string.IsNullOrEmpty(Email) || !_regexHelper.IsValidEmail(Email))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar un correo electrónico", "Aceptar");
                return false;
            }
            return true;
        }
        private async void RecoverAsync()
        {

            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                IsEnabled = true;
                await _dialogService.DisplayAlertAsync("Error", "Compruebe la conexión a Internet", "Aceptar");
                return;
            }



            EmailRequest request = new EmailRequest
            {
                email = Email,
                //CultureInfo = Languages.Culture
            };

            string url = App.Current.Resources["UrlAPI"].ToString();
            RecoverPasswordResponse response = await _apiService.RecoverPasswordAsync(url, "/api", "/Account/resetpassword", request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.result)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.ResultMessages.FirstOrDefault(), "Aceptar");
                return;
            }

            await App.Current.MainPage.DisplayAlert("Ok", response.ResultMessages.FirstOrDefault(), "Aceptar");
            await _navigationService.GoBackAsync();
            //await _navigationService.NavigateAsync(nameof(UpdatePasswordPage));
        }
    }
}
