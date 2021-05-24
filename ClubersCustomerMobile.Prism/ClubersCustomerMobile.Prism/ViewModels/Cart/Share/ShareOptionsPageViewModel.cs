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
    public class ShareOptionsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Order _shoppingCartOrder;
        private DelegateCommand _shareWithPhotoCommand;

        public ShareOptionsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Compartir";
        }
        public DelegateCommand ShareWithPhotoCommand => _shareWithPhotoCommand ?? (_shareWithPhotoCommand = new DelegateCommand(ShareWithPhotoAsync));

        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        
        private async void ShareWithPhotoAsync()
        {

            if (ShoppingCartOrder == null)
                return;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            await _navigationService.NavigateAsync(nameof(ShareWithPhotoPage), parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");

            }
        }
    }
}
