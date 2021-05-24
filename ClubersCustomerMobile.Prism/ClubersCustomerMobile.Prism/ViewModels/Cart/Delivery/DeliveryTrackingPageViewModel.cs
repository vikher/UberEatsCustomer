using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Linq;
using Xamarin.Essentials;


namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class DeliveryTrackingPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Order _shoppingCartOrder;
        private Delivery _delivery;
        private DeliveryMen _deliveryMen;
        private Establishment _establishment;
        private DelegateCommand _deliveryTrackingDetailCommand;
        private DelegateCommand _orderAtHomeCommand;
        private DelegateCommand _callDeliveryManCommand;
        private DelegateCommand _messageDeliveryManCommand;
        private string _phoneIcon = IconFont.PhoneInTalk;
        private string _messageIcon = IconFont.Message;
        private string _truckIcon = IconFont.Truck;

        public DeliveryTrackingPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Progreso de compra";
            Delivery = new Delivery() { Id = 1, Status = "En proceso", StartDate = DateTime.Now, EndDate = DateTime.Now, OrderDeliveryNumber = 123245, DeliveryDetails = "Repartidor en camino - 10 min aprox." };
            //LoadDeliveryManAsync();
        }
        public DelegateCommand DeliveryTrackingDetailCommand => _deliveryTrackingDetailCommand ?? (_deliveryTrackingDetailCommand = new DelegateCommand(DeliveryTrackingDetailAsync));
        public DelegateCommand CallDeliveryManCommand => _callDeliveryManCommand ?? (_callDeliveryManCommand = new DelegateCommand(PlacePhoneCall));
        public DelegateCommand MessageDeliveryManCommand => _messageDeliveryManCommand ?? (_messageDeliveryManCommand = new DelegateCommand(SendSms));
        public DelegateCommand OrderAtHomeCommand => _orderAtHomeCommand ?? (_orderAtHomeCommand = new DelegateCommand(OrderAtHomeAsync));
        public string PhoneIcon
        {
            get => _phoneIcon;
            set => SetProperty(ref _phoneIcon, value);
        }
        public string MessageIcon
        {
            get => _messageIcon;
            set => SetProperty(ref _messageIcon, value);
        }
        public string TruckIcon
        {
            get => _truckIcon;
            set => SetProperty(ref _truckIcon, value);
        }
        public Establishment Establishment
        {
            get => _establishment;
            set => SetProperty(ref _establishment, value);
        }
        public DeliveryMen DeliveryMen
        {
            get => _deliveryMen;
            set => SetProperty(ref _deliveryMen, value);
        }
        public Delivery Delivery
        {
            get => _delivery;
            set => SetProperty(ref _delivery, value);
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }

        private async void LoadDeliveryManAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            Response1<DeliveryMen> response = await _apiService.GetDeliveryManAsync(Constants.urlBase, Constants.servicePrefix, Constants.GetDeliveryMenByOrderIdController, Constants.tokenType, token.Result.token, 19);
            
            if (response.ResultCode != ResultCode.Success)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }
            DeliveryMen = response.Result;

            IsRunning = false;
        }

        private async void DeliveryTrackingDetailAsync()
        {
            if (ShoppingCartOrder == null)
                return;

            Delivery.DeliveryMen = DeliveryMen;
            ShoppingCartOrder.Delivery = Delivery;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            await _navigationService.NavigateAsync(nameof(DeliveryTrackingDetailPage),parameters);
        }


        private async void OrderAtHomeAsync()
        {
            if (ShoppingCartOrder == null)
                return;

            Delivery.DeliveryMen = DeliveryMen;
            ShoppingCartOrder.Delivery = Delivery;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            await _navigationService.NavigateAsync(nameof(OrderAtHomePage),parameters);
        }

        public async void SendSms()
        {

            string messageText = "hi";
            string recipient = "+5215586765674";

            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        public void PlacePhoneCall()
        {

            string number = "+5215586765674";
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                //LoadDeliveryManAsync();
            }

            Establishment = ShoppingCartOrder.Establishment;
        }

    }
}
