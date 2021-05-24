using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using ClubersCustomerMobile.Prism.Views.Cart.Purchase;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    //this class creates the order
    public class PinPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private string _pincode;
        private int _pinCodeAttempt;
        private bool _payEnabled;
        private Order _shoppingCartOrder;
        private DelegateCommand<string> _numberCommand;
        private DelegateCommand _eraseNumberCommand;
        private DelegateCommand _pinRecoveryCommand;
        private DelegateCommand _sendOrderCommand;
        private string UserPincode = "1234";


        public PinPageViewModel(INavigationService navigationService,
                                IPageDialogService dialogService,
                                IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            Title = "Ingresa tu NIP";
            PayEnabled = false;
        }
        public DelegateCommand SendOrderCommand => _sendOrderCommand ?? (_sendOrderCommand = new DelegateCommand(FinishOrderAsync));
        public DelegateCommand EraseNumberCommand => _eraseNumberCommand ?? (_eraseNumberCommand = new DelegateCommand(EraseNumber));
        public DelegateCommand PinRecoveryCommand => _pinRecoveryCommand ?? (_pinRecoveryCommand = new DelegateCommand(PinRecoveryAsync));
        public DelegateCommand<string> NumberCommand => _numberCommand ?? (_numberCommand = new DelegateCommand<string>(EnterNumber));
        private async void PinRecoveryAsync()
        {
            await _navigationService.NavigateAsync(nameof(PinRecoveryPage));
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        public bool PayEnabled
        {
            get => _payEnabled;
            set => SetProperty(ref _payEnabled, value);
        }

        public int PinCodeAttempt
        {
            get => _pinCodeAttempt;
            set => SetProperty(ref _pinCodeAttempt, value);
        }
        public string Pincode
        {
            get => _pincode;
            set => SetProperty(ref _pincode, value, () => RaisePropertyChanged(nameof(PincodeMasked)));
        }
        public string PincodeMasked { get { return string.Empty.PadLeft(Pincode != null ? Pincode.Length : 0, '*'); } }

        private async void FinishOrderAsync()
        {
            if (Pincode == UserPincode)
            {
            
                IsRunning = true;
                if (!_apiService.CheckConnection())
                {
                    IsRunning = false;
                    await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                    return;
                }
                /*
                UserResponse User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

             
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

                OrderRequest request = new OrderRequest
                {
                    EstablishmentId = ShoppingCartOrder.Establishment.Id,
                    CustomerId = User.Result.Id,
                    PaymentId = 1,
                    ConsumptionTypeId = 1,
                    StartDate = ShoppingCartOrder.StartDate,
                    DeliveryFee = 0,
                    TipAmount = ShoppingCartOrder.Tip.Amount,
                    TrackingStatusId = 1,
                    EstablishmentStaffId = 0,
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
                */
                IsRunning = false;
                
                await App.Current.MainPage.DisplayAlert(Constants.AcceptMessage, Constants.FinishOrderMessage, Constants.AcceptMessage);
               
                NavigationParameters parameters = new NavigationParameters
                        {
                            { "order", ShoppingCartOrder }
                        };

                await _navigationService.NavigateAsync(nameof(SuccessOrderPage), parameters);

            }

        }


        private async void EnterNumber(string key)
        {
            // Add the key to the input string until we have the max length of 6.
            if (Pincode == null || Pincode.Length < 4)
                Pincode += key;

            // If there's a pin and it's 6 in length we try a login.
            if (Pincode != null && Pincode.Length == 4)
            {
                if (PinCodeAttempt < 3)
                {
                    if (Pincode == UserPincode)
                    {
                        PayEnabled = true;
                        PinCodeAttempt = 0;
                        if (ShoppingCartOrder == null)
                            return;
                        FinishOrderAsync();
                        //return;

                        //await _navigationService.NavigateAsync(nameof(SuccessOrderPage), parameters);
                        //await _navigationService.NavigateAsync(nameof(PaymentPage), parameters);
                    }
                    else
                    {
                        PinCodeAttempt++;
                        await _dialogService.DisplayAlertAsync("Error!", "El pin es 1234 Numero de intentos" + PinCodeAttempt, "Aceptar");
                        Pincode = string.Empty;
                        return;

                    }

                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error!", "Haz bloqueado tu cuenta", "Aceptar");
                    return;
                }
            }
        }
        private void EraseNumber()
        {
            if(Pincode.Length > 0)
            {
                Pincode = Pincode.Remove(Pincode.Length - 1, 1);
            }
            else
            {
                return;
            }
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
