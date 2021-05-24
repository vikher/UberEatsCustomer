using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OnSitePaymentConfirmationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _imageUrl;
        private Order _shoppingCartOrder;
        private Refund _refund;
        private DelegateCommand _continueCommand;
        public OnSitePaymentConfirmationPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "confirmacion";
            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/clubers-278716.appspot.com/o/checked.png?alt=media&token=4cfbd219-287e-4ba4-b1fd-bd7c45ffc948";
            Refund = new Refund() { Amount = 0 };
            LoadTermsAndConditionsAsync();
        }

        public DelegateCommand ContinueCommand => _continueCommand ?? (_continueCommand = new DelegateCommand(ContinueAsync));
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }

        public Refund Refund
        {
            get => _refund;

            set => SetProperty(ref _refund, value);
        }
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }
        private async void ContinueAsync()
        {
            /*if (ShoppingCartOrder == null)
                return;

            ShoppingCartOrder.Refund = Refund;
            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };*/
            await NavigationService.NavigateAsync(nameof(CustomerTabbedPage));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                Refund = new Refund() { Amount = ShoppingCartOrder.TotalPrice * .10 };
            }
            LoadTermsAndConditionsAsync();
        }

        private async void LoadTermsAndConditionsAsync()
        {
        }
    }
}
