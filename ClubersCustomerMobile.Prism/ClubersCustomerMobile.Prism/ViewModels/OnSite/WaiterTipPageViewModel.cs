using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
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
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class WaiterTipPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private Tip _selectedTip;
        private Order _shoppingCartOrder;
        private Waiter _waiter;
        private double _total;
        private List<Tip> _tips;
        private List<Product> _products;
        private DelegateCommand _customTipCommand;
        private DelegateCommand _pinCommand;
        public WaiterTipPageViewModel(INavigationService navigationService,
                                  IPageDialogService dialogService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Agregar propina";
            SelectedTip = new Tip() { Amount = 0 };
            Waiter = new Waiter() { Id = 1, ImageUrl = "https://clubersblob.blob.core.windows.net/users/profile.jpg", Name = "Junior Sánchez" };
            Total = 0.0;
        }
        public DelegateCommand PinCommand => _pinCommand ?? (_pinCommand = new DelegateCommand(PinAsync));
        public DelegateCommand CustomTipCommand => _customTipCommand ?? (_customTipCommand = new DelegateCommand(ShowCustomTipAsync));
        public Waiter Waiter
        {
            get => _waiter;
            set => SetProperty(ref _waiter, value);
        }
        public List<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        public Tip SelectedTip
        {
            get => _selectedTip;
            set => SetProperty(ref _selectedTip, value, () => RaisePropertyChanged(nameof(Total)));
        }
        public List<Tip> Tips
        {
            get => _tips;
            set => SetProperty(ref _tips, value);
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
                    _total = (ShoppingCartOrder.Products.Sum(x => x.OnSiteTotal) + SelectedTip.Amount);
                }
                return _total;
            }

            set => SetProperty(ref _total, value);
        }
        private async void ShowCustomTipAsync()
        {
            //SelectedTip = null;
            await _navigationService.NavigateAsync(nameof(CustomTipPage));
        }
        //private async void LoadTipsAsync1()
        //{
        //    Response response = await _apiService.GetTipsAsync<Tip>("", " / api", "/Establishment", "bearer", "");
        //    Tips = (List<Tip>)response.Result;
        //    SelectedTip = new Tip() { Amount = 0 };
        //}

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

        private async void PinAsync()
        {
            await _navigationService.NavigateAsync(nameof(OnSitePinPage));
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

            }

            if (parameters.ContainsKey("customTip"))
            {
                SelectedTip = new Tip() { Amount = Double.Parse(parameters.GetValue<string>("customTip")) };
                ShoppingCartOrder.Tip.Amount = SelectedTip.Amount;
            }

        }

    }
}
