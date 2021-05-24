using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OnSitePinPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private string _pincode;
        private int _pinCodeAttempt;
        private Order _shoppingCartOrder;
        private DelegateCommand<string> _numberCommand;
        private DelegateCommand _eraseNumberCommand;
        private DelegateCommand _pinRecoveryCommand;
        public OnSitePinPageViewModel(INavigationService navigationService,
                                  IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Title = "NIP";
        }

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
                    if (Pincode == "1234")
                    {
                        PinCodeAttempt = 0;
                        /*if (ShoppingCartOrder == null)
                            return;

                        NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };*/
                        await _navigationService.NavigateAsync(nameof(OnSitePaymentConfirmationPage));
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
            Pincode = Pincode.Remove(Pincode.Length - 1, 1);
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
