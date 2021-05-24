using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class ShoppingCartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private Order _shoppingCartOrder;
        private int _total;
        private Address _address;
        private string _briefcaseIcon = IconFont.Briefcase;
        private string _checkCircleIcon = IconFont.CheckCircle;
        private ObservableCollection<Product> _properties;
        private DelegateCommand _checkoutCommand;
        private DelegateCommand _clearBasketCommand;
        private DelegateCommand _savedAddressesCommand;

        public ShoppingCartPageViewModel(INavigationService navigationService,
            IApiService apiService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;
            Title = "Carrito de compras";
            //LoadAddressAsync();
        }
        public DelegateCommand CheckoutCommand => _checkoutCommand ?? (_checkoutCommand = new DelegateCommand(CheckoutAsynx));
        public DelegateCommand SavedAddressesCommand => _savedAddressesCommand ?? (_savedAddressesCommand = new DelegateCommand(SavedAddressesAsync));
        public DelegateCommand ClearBasketCommand => _clearBasketCommand ?? (_clearBasketCommand = new DelegateCommand(ClearBasketAsync));
        public string BriefcaseIcon
        {
            get => _briefcaseIcon;
            set => SetProperty(ref _briefcaseIcon, value);
        }
        public string CheckCircleIcon
        {
            get => _checkCircleIcon;
            set => SetProperty(ref _checkCircleIcon, value);
        }
        public Address Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
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
        private async void SavedAddressesAsync()
        {
            await _navigationService.NavigateAsync(nameof(SavedAddressesPage));
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                Products = new ObservableCollection<Product>(ShoppingCartOrder.Products);

                //Products = ShoppingCartOrder.Products;
                //ReloadTotal();
            }

            if (parameters.ContainsKey("selAddress"))
            {
                Address = parameters.GetValue<Address>("selAddress");
            }
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

        private async void CheckoutAsynx()
        {
            if (ShoppingCartOrder == null)
            {
                return;
            }
            else
            {
                NavigationParameters parameters = new NavigationParameters
            {
                { "order", ShoppingCartOrder },
                { "address", Address }
            };

                await _navigationService.NavigateAsync(nameof(PurchaseDetailPage), parameters);
            }
        }

        public void ReloadTotal()
        {
            if (ShoppingCartOrder.Products == null)
                return;

            //ShoppingCartOrder.ProductsCount = ShoppingCartOrder.Products.Count;
        }

        //private async void LoadAddressAsync()
        //{
        //    Response response = await _apiService.GetAllAddressesAsync<Address>("", " / api", "/Establishment", "bearer", "");
        //    var addresses = (List<Address>)response.Result;
        //    Address = addresses[0];

        //}
    }
}