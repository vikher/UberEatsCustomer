using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class VerifyPhonePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private DelegateCommand _freeTrialCommand;
        private DelegateCommand _resendOtpCommand;
        private UserRequest _user;


        string MyrecentOTP;
        private string _phoneNumber;
        private string _myOtp;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }


        public string MyOtp
        {
            get => _myOtp;
            set => SetProperty(ref _myOtp, value);
        }

        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public VerifyPhonePageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Verificación";

        }
        public DelegateCommand FreeTrialCommand => _freeTrialCommand ?? (_freeTrialCommand = new DelegateCommand(FreeTrialAsync));
        public DelegateCommand ResendOtpCommand => _resendOtpCommand ?? (_resendOtpCommand = new DelegateCommand(ResendOtpAsync));


        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            if (parameters.ContainsKey("User"))
            {
                User = parameters.GetValue<UserRequest>("User");
                PhoneNumber = "+52" + User.Phone;
                GetTwilioTxt();

            }
            else
            {
                return;
            }

        }
        private async void ResendOtpAsync()
        {
            GetTwilioTxt();
        }

        private async void FreeTrialAsync()
        {

            if (MyOtp == MyrecentOTP)
            {
                //Go to dashboard
                NavigationParameters parameters = new NavigationParameters
            {
                { "User", User }
            };

                await _navigationService.NavigateAsync(nameof(FreeTrialPage), parameters);

            }
            else
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Otp verification Error.", "Aceptar");
            }
        }

        public void GetTwilioTxt()
        {

            const string accountSid = "";// Your Account SId
            const string authToken = ""; // Your Account AuthToken

            TwilioClient.Init(accountSid, authToken);

            Random generator = new Random();
            String randomotp = generator.Next(0, 9999).ToString("D4");

            var message = MessageResource.Create(
                body: "Your new OTP is ?" + randomotp,
                from: new Twilio.Types.PhoneNumber("+12057720481"), //This is my Trail Number  https://www.twilio.com/console
                to: new Twilio.Types.PhoneNumber(PhoneNumber) //Add your phone number here
            );

            Console.WriteLine(message.Sid);

            MyrecentOTP = randomotp;

        }
    }
}
