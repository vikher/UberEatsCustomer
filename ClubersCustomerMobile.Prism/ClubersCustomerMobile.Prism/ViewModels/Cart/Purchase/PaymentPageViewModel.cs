using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class PaymentPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        //public ICommand PayCommand { get; set; }
        public CardInfo CardInfo { get; set; } = new CardInfo();
        IPayService _payService;

        private DelegateCommand _payCommand;
        string paymentClientToken = "<Payment token returned by the API HERE>";

        public PaymentPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Pago";
            _payService = Xamarin.Forms.DependencyService.Get<IPayService>();
        }
        public DelegateCommand PayCommand => _payCommand ?? (_payCommand = new DelegateCommand(CreatePayment));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            GetPaymentConfig();
        }

        async Task GetPaymentConfig()
        {

            await _payService.InitializeAsync(paymentClientToken);

        }


        private async void CreatePayment()
        {
            //UserDialogs.Instance.ShowLoading("Loading");

            if (_payService.CanPay)
            {
                try
                {
                    _payService.OnTokenizationSuccessful += OnTokenizationSuccessful;
                    _payService.OnTokenizationError += OnTokenizationError;
                    await _payService.TokenizeCard(CardInfo.CardNumber.Replace(" ", string.Empty), CardInfo.Expiry.Substring(0, 2), $"{DateTime.Now.ToString("yyyy").Substring(0, 2)}{CardInfo.Expiry.Substring(3, 2)}", CardInfo.Cvv);

                }
                catch (Exception ex)
                {
                    //UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error", "Unable to process payment", "Ok");
                    System.Diagnostics.Debug.WriteLine(ex);
                }

            }
            else
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    //UserDialogs.Instance.HideLoading();
                    await App.Current.MainPage.DisplayAlert("Error", "Payment not available", "Ok");
                });
            }
        }

        async void OnTokenizationSuccessful(object sender, string e)
        {
            _payService.OnTokenizationSuccessful -= OnTokenizationSuccessful;
            System.Diagnostics.Debug.WriteLine($"Payment Authorized - {e}");
            //UserDialogs.Instance.HideLoading();
            await App.Current.MainPage.DisplayAlert("Success", $"Payment Authorized: the token is{e}", "Ok");
            //await _navigationService.NavigateAsync(nameof(SuccessOrderPage), parameters);

        }

        async void OnTokenizationError(object sender, string e)
        {
            _payService.OnTokenizationError -= OnTokenizationError;
            System.Diagnostics.Debug.WriteLine(e);
            //UserDialogs.Instance.HideLoading();
            await App.Current.MainPage.DisplayAlert("Error", "Unable to process payment", "Ok");

        }

    }
}

