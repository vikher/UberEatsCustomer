using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CommandPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Order _shoppingCartOrder;
        private readonly IPageDialogService _dialogService;
        private string _locationIcon1 = IconFont.MapMarker;
        private ObservableCollection<Product> _properties;
        private DelegateCommand _paymentMethodsCommand;
        public CommandPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Comanda";
        }
        public DelegateCommand PaymentMethodsCommand => _paymentMethodsCommand ?? (_paymentMethodsCommand = new DelegateCommand(PaymentMethodsAsync));
        public string LocationIcon1
        {
            get => _locationIcon1;
            set => SetProperty(ref _locationIcon1, value);
        }
        public ObservableCollection<Product> Products
        {
            get => _properties;
            set => SetProperty(ref _properties, value);
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        private async void ClearBasketAsync()
        {
            var action = await _dialogService.DisplayAlertAsync("Su pedido", "¿Estás segura de que quieres que tu cesta esté vacía?", "SI", "NO");

            switch (action)
            {
                case true:
                    ShoppingCartOrder.Products = null;
                    ShoppingCartOrder = null;
                    await _navigationService.GoBackAsync();
                    break;
                case false:
                    return;
            }

        }
        private async void PaymentMethodsAsync()
        {
            if (ShoppingCartOrder == null)
                return;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            await _navigationService.NavigateAsync(nameof(PaymentMethodsPage), parameters);

        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                //Products = ShoppingCartOrder.Products;
                Products = new ObservableCollection<Product>(ShoppingCartOrder.Products);

            }
        }
    }
}
