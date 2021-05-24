using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class SavedPaymentMethodsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private List<PaymentMethod> _paymentMethods;
        private DelegateCommand _addNewPaymentMethodCommand;
        private DelegateCommand _favoriteCommand;
        public SavedPaymentMethodsPageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Método de Pago";
            LoadSavedPaymentMethodsAsync();
        }
        public DelegateCommand AddNewPaymentMethodCommand => _addNewPaymentMethodCommand ?? (_addNewPaymentMethodCommand = new DelegateCommand(AddNewPaymentMethodAsync));
        public DelegateCommand FavoriteCommand => _favoriteCommand ?? (_favoriteCommand = new DelegateCommand(AddNewPaymentMethodAsync));
        
        public List<PaymentMethod> PaymentMethods
        {
            get => _paymentMethods;
            set => SetProperty(ref _paymentMethods, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadSavedPaymentMethodsAsync();
        }

        private async void AddNewPaymentMethodAsync()
        {
            await _navigationService.NavigateAsync(nameof(AddNewPaymentPage));
        }

        private async void LoadSavedPaymentMethodsAsync()
        {
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await _dialogService.DisplayAlertAsync("Error", "Compruebe la conexión a Internet", "Accept");
                return;
            }

            Response response = await _apiService.GetAllPaymentMethodsAsync<PaymentMethod>(Constants.urlBase, Constants.servicePrefix, Constants.controller, Constants.tokenType, Constants.accessToken);
            PaymentMethods = (List<PaymentMethod>)response.Result;
        }
    }
}
