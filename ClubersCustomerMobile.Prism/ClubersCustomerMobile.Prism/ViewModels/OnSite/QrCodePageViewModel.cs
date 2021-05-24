using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class QrCodePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Order _shoppingCartOrder;

        public QrCodePageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Código QR";
        }

        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
            }

            SendCommandOrderAsync();
        }
        private async void SendCommandOrderAsync()
        {

            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            UserResponse User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);


            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            OrderRequest request = new OrderRequest
            {
                EstablishmentId = ShoppingCartOrder.Establishment.Id,
                CustomerId = User.Result.Id,
                PaymentId = 1,
                ConsumptionTypeId = 3,
                StartDate = ShoppingCartOrder.StartDate,
                DeliveryFee = 0,
                TipAmount = ShoppingCartOrder.Tip.Amount,
                TrackingStatusId = 1,
                EstablishmentStaffId = 1,
                Total = ShoppingCartOrder.TotalPrice,
                TableNumber = ShoppingCartOrder.TableNumber,
                OrderProducts = ShoppingCartOrder.Products,
            };

            Response response = await _apiService.PostAsync<OrderRequest>(Constants.urlBase, Constants.servicePrefix, Constants.PostOrderController, request, Constants.tokenType, token.Result.token);

            if (response.ResultCode != ResultCode.Success)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                IsRunning = false;

                return;
            }

            IsRunning = false;

            await App.Current.MainPage.DisplayAlert(Constants.AcceptMessage, Constants.FinishOrderMessage, Constants.AcceptMessage);

            NavigationParameters parameters = new NavigationParameters
                        {
                            { "order", ShoppingCartOrder }
                        };

            await _navigationService.NavigateAsync(nameof(CommandWithWaiterPage), parameters);
        }
    }
}
