using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class PurchaseDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private Order _shoppingCartOrder;        
        private double _total;
        private double _remainingBalance;
        private Address _address;
        private Tip _selectedTip;
        private List<Tip> _tips;
        private double _availableBalance;
        private double _selectedBalance;
        private string _locationIcon = IconFont.MapMarker;
        private List<Product> _products;
        private DelegateCommand _customTipCommand;
        private DelegateCommand _checkoutCommand;
        private DelegateCommand _clearBasketCommand;
        private DelegateCommand _refreshTotalCommand;
        private DelegateCommand _savedAddressesCommand;

        public PurchaseDetailPageViewModel(INavigationService navigationService,
                                  IPageDialogService dialogService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Orden";
            SelectedTip = new Tip() { Amount = 0 };

            LoadBalanceAsync();
            SelectedTip.Amount = 0.0;
            Total = 0.0;
        }
        public DelegateCommand CheckoutCommand => _checkoutCommand ?? (_checkoutCommand = new DelegateCommand(CheckoutAsynx));
        public DelegateCommand ClearBasketCommand => _clearBasketCommand ?? (_clearBasketCommand = new DelegateCommand(ClearBasketAsync));
        public DelegateCommand RefreshTotalCommand => _refreshTotalCommand ?? (_refreshTotalCommand = new DelegateCommand(RefreshTotal));
        public DelegateCommand CustomTipCommand => _customTipCommand ?? (_customTipCommand = new DelegateCommand(ShowCustomTipAsync));
        public DelegateCommand SavedAddressesCommand => _savedAddressesCommand ?? (_savedAddressesCommand = new DelegateCommand(SavedAddressesAsync));
        public string LocationIcon
        {
            get => _locationIcon;
            set => SetProperty(ref _locationIcon, value);
        }
        public double AvailableBalance
        {
            get => _availableBalance;
            set => SetProperty(ref _availableBalance, value);
        }
        public double SelectedBalance
        {
            get => _selectedBalance;

            set => SetProperty(ref _selectedBalance, value, () => { RaisePropertyChanged(nameof(Total)); RaisePropertyChanged(nameof(RemainingBalance)); });

        }
        public double Total
        {
            get
            {
                if (ShoppingCartOrder == null)
                {
                    _total = 0;
                }
                else
                {
                    if (RemainingBalance == 0)
                    {
                        _total = (ShoppingCartOrder.Products.Sum(x => x.Total) + SelectedTip.Amount - AvailableBalance);

                    }
                    else
                    {
                        _total = (ShoppingCartOrder.Products.Sum(x => x.Total) + SelectedTip.Amount - SelectedBalance);
                    }
                }
                return _total;
            }

            set => SetProperty(ref _total, value);
        }

        public double RemainingBalance
        {
            get
            {
                _remainingBalance = AvailableBalance - SelectedBalance;
                if (_remainingBalance < 0 )
                {
                    _remainingBalance = 0;
                }
                return _remainingBalance;
            }
        }

        //public double RemainingBalance => AvailableBalance - SelectedBalance;
        public Address Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        //public string CompleteAddress
        //{
        //    get
        //    {
        //        return string.Format("{0} {1} {2} {3} {4} {5}", Address.Street, Address.Number, Address.Suburb, Address.City, Address.PostalCode, Address.State);
        //    }
        //}
        public Tip SelectedTip
        {
            get => _selectedTip; 
            set => SetProperty(ref _selectedTip, value, () => RaisePropertyChanged(nameof(Total)));
        }

        //[AutoInitialize("order", true)]
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }

        [AutoInitialize(true)]
        public List<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        public List<Tip> Tips
        {
            get => _tips;
            set => SetProperty(ref _tips, value);
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
                ShoppingCartOrder.Tip.Amount = SelectedTip.Amount;
                
                Products = ShoppingCartOrder.Products;
                Total = ShoppingCartOrder.TotalPrice;
                LoadTipsAsync(ShoppingCartOrder.Establishment.Id);
                //LoadAddressAsync();

            }

            if (parameters.ContainsKey("customTip"))
            {
                SelectedTip = new Tip() { Amount = Double.Parse(parameters.GetValue<string>("customTip")) };
                ShoppingCartOrder.Tip.Amount = SelectedTip.Amount;
            }

            if (parameters.ContainsKey("address"))
            {
                Address = parameters.GetValue<Address>("address");
            }
        }

        private void RefreshTotal()
        {
            ShoppingCartOrder.Tip = SelectedTip;
        }
        private async void ShowCustomTipAsync()
        {
            //SelectedTip = null;
            await _navigationService.NavigateAsync(nameof(CustomTipPage));
        }
        private async void ClearBasketAsync()
        {
            await _dialogService.DisplayAlertAsync("ok", "ok", "ok");
        }
        private async void CheckoutAsynx()
        {
            if (ShoppingCartOrder == null)
                return;

            if (IsNegative(RemainingBalance))
            {
                await _dialogService.DisplayAlertAsync("Error", "Tu saldo no puede ser negativo.", "Aceptar");
                return;
            }

            ShoppingCartOrder.Total = Total;
            ShoppingCartOrder.Tip = SelectedTip;
            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };

            await _navigationService.NavigateAsync(nameof(PinPage), parameters);

            /*var actionSheet = await _dialogService.DisplayActionSheetAsync("Cuentas con $200 saldo Clubers disponible", null ,  null, "CONTINUAR AHORRANDO", "UTILIZAR SALDO");
            switch (actionSheet)
            {
                case "CONTINUAR AHORRANDO":
                    if (ShoppingCartOrder == null)
                        return;

                    ShoppingCartOrder.Total = Total;
                    ShoppingCartOrder.Tip = SelectedTip;
                    NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };

                    await _navigationService.NavigateAsync(nameof(PinPage), parameters);

                    break;

                case "UTILIZAR SALDO":
                    if (ShoppingCartOrder == null)
                        return;

                    ShoppingCartOrder.Total = Total;
                    ShoppingCartOrder.Tip = SelectedTip;
                    NavigationParameters parameters2 = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };

                    await _navigationService.NavigateAsync(nameof(UseBalancePage), parameters2);

                    break;
            }*/
        }
        public void ReloadTotal()
        {
            if (ShoppingCartOrder.Products == null)
                return;
            //ShoppingCartOrder.ProductsCount = ShoppingCartOrder.Products.Count;
        }

        private async void LoadTipsAsync(int EstablishmentId)
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            ListResponse<Tip> response = await _apiService.GetListAsync<Tip>(Constants.urlBase, Constants.servicePrefix, Constants.GetTipsByEstablishmentIdAsyncController, Constants.tokenType, token.Result.token, EstablishmentId);
            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }
            Tips = (List<Tip>)response.Result;
            IsRunning = false;

        }
        //private async void LoadAddressAsync()
        //{
        //    Response response = await _apiService.GetAllAddressesAsync<Address>("", " / api", "/Establishment", "bearer", "");
        //    var addresses = (List<Address>)response.Result;
        //    Address = addresses[0];
        //}
        private void LoadBalanceAsync()
        {
            AvailableBalance = 10.5;
        }
        private bool IsNegative(double number)
        {
            return number < 0;
        }
    }
}