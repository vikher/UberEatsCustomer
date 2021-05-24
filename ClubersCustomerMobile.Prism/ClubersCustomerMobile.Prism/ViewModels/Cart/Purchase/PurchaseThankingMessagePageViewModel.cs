using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class PurchaseThankingMessagePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _imageUrl;
        private Order _shoppingCartOrder;
        private Refund _refund;
        private string _giftIcon = IconFont.Gift;
        private DelegateCommand _homeCommand;
        private DelegateCommand _evaluateDeliveryCommand;
        public PurchaseThankingMessagePageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Mensaje de compra";
            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/clubers-278716.appspot.com/o/checked.png?alt=media&token=4cfbd219-287e-4ba4-b1fd-bd7c45ffc948";

        }
        public DelegateCommand HomeCommand => _homeCommand ?? (_homeCommand = new DelegateCommand(HomeAsync));
        public DelegateCommand EvaluateDeliveryCommand => _evaluateDeliveryCommand ?? (_evaluateDeliveryCommand = new DelegateCommand(EvaluateDeliveryAsync));
        public string GiftIcon
        {
            get => _giftIcon;
            set => SetProperty(ref _giftIcon, value);
        }
        public Refund Refund
        {
            get => _refund;
            set => SetProperty(ref _refund, value);
        }

        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        private async void HomeAsync()
        {
            await _navigationService.NavigateAsync("/NavigationPage/CustomerTabbedPage?selectedTab=HomePage");
        }
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }       
        private async void EvaluateDeliveryAsync()
        {

            if (ShoppingCartOrder == null)
                return;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            await _navigationService.NavigateAsync($"NavigationPage/{nameof(EvaluateDeliveryPage)}", parameters, useModalNavigation: true);
            //await _navigationService.NavigateAsync(nameof(ShareOptionsPage), parameters, useModalNavigation: true);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                Refund = ShoppingCartOrder.Refund;
            }
        }
    }
}
