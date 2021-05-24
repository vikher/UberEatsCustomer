using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CommandWithWaiterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private Order _shoppingCartOrder;
        private Waiter _waiter;
        private List<Product> _properties;
        private DelegateCommand _waiterTipCommand;
        public CommandWithWaiterPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Comanda";
            //Waiter = new Waiter() { Id = 1, ImageUrl = "https://clubersblob.blob.core.windows.net/users/profile.jpg", Name = "Junior Sánchez"};
        }
        public DelegateCommand WaiterTipCommand => _waiterTipCommand ?? (_waiterTipCommand = new DelegateCommand(WaiterTipAsync));

        public Waiter Waiter
        {
            get => _waiter;
            set => SetProperty(ref _waiter, value);
        }
        public List<Product> Products
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
        private async void WaiterTipAsync()
        {
            if (ShoppingCartOrder == null)
                return;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            await _navigationService.NavigateAsync(nameof(WaiterTipPage), parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                Products = ShoppingCartOrder.Products;
                LoadEstablishmentStaffAsync();
            }
        }

        private async void LoadEstablishmentStaffAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            Response1<Waiter> response = await _apiService.GetEstablishmentStaffAsync(Constants.urlBase, Constants.servicePrefix, Constants.GetEstablishmentStaffByOrderIdController, Constants.tokenType, token.Result.token, 19);

            if (response.ResultCode != ResultCode.Success)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }
            Waiter = response.Result;

            IsRunning = false;
        }
    }
}
