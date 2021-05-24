using ClubersCustomerMobile.Prism.Extensions;
using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class AddNewPaymentPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _number;
        private string _name;
        private bool _numberIsVisible;
        private bool _nameIsVisible;
        private bool _validIsVisible;
        private bool _cvvIsVisible;
        private bool _numberErrorMsgIsVisible;
        private bool _cvvErrorMsgIsVisible;
        private string _cvvErrorMsgText;
        private string _nameErrorMsgText;
        private bool _nameErrorMsgIsVisible;
        private string _validErrorMsgText;
        private string _valid;
        private string _cvv;
        private Image _logoCreditCard;
        private ViewFlipper _cardSimulationInfo;
        private string _buttonText;
        private string _numberErrorMsgText;
        private readonly IPageDialogService _dialogService;
        private DelegateCommand _nextCommand;

        public AddNewPaymentPageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;
            Title = "Agregar forma de pago";
            ResetProps();
        }
        public DelegateCommand NextCommand => _nextCommand ?? (_nextCommand = new DelegateCommand(SaveNewPaymentMethodAsync));

        public string ButtonText
        {
            set { SetProperty(ref _buttonText, value); }
            get { return _buttonText; }
        }
        public ViewFlipper CardSimulationInfo
        {
            set { SetProperty(ref _cardSimulationInfo, value); }
            get { return _cardSimulationInfo; }
        }

        public Image LogoCreditCard
        {
            set { SetProperty(ref _logoCreditCard, value); }
            get { return _logoCreditCard; }
        }
        public string Number
        {
            set { SetProperty(ref _number, value); }
            get { return _number; }
        }
        public string Name
        {
            set { SetProperty(ref _name, value); }
            get { return _name; }
        }
        public bool NumberIsVisible
        {
            set { SetProperty(ref _numberIsVisible, value); }
            get { return _numberIsVisible; }
        }
        public bool NameIsVisible
        {
            set { SetProperty(ref _nameIsVisible, value); }
            get { return _nameIsVisible; }
        }
        public bool ValidIsVisible
        {
            set { SetProperty(ref _validIsVisible, value); }
            get { return _validIsVisible; }
        }
        public bool CvvIsVisible
        {
            set { SetProperty(ref _cvvIsVisible, value); }
            get { return _cvvIsVisible; }
        }
        public bool NumberErrorMsgIsVisible
        {
            set { SetProperty(ref _numberErrorMsgIsVisible, value); }
            get { return _numberErrorMsgIsVisible; }
        }
        public bool CvvErrorMsgIsVisible
        {
            set { SetProperty(ref _cvvErrorMsgIsVisible, value); }
            get { return _cvvErrorMsgIsVisible; }
        }
        public string CvvErrorMsgText
        {
            set { SetProperty(ref _cvvErrorMsgText, value); }
            get { return _cvvErrorMsgText; }
        }
        public string NameErrorMsgText
        {
            set { SetProperty(ref _nameErrorMsgText, value); }
            get { return _nameErrorMsgText; }
        }
        public bool NameErrorMsgIsVisible
        {
            set { SetProperty(ref _nameErrorMsgIsVisible, value); }
            get { return _nameErrorMsgIsVisible; }
        }

        public string ValidErrorMsgText
        {
            set { SetProperty(ref _validErrorMsgText, value); }
            get { return _validErrorMsgText; }
        }

        private bool _validErrorMsgIsVisible;
        public bool ValidErrorMsgIsVisible
        {
            set { SetProperty(ref _validErrorMsgIsVisible, value); }
            get { return _validErrorMsgIsVisible; }
        }

        public string NumberErrorMsgText
        {
            set { SetProperty(ref _numberErrorMsgText, value); }
            get { return _numberErrorMsgText; }
        }


        public string Valid
        {
            set { SetProperty(ref _valid, value); }
            get { return _valid; }
        }

        public string Cvv
        {
            set { SetProperty(ref _cvv, value); }
            get { return _cvv; }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ResetProps();
        }

        private void ResetProps()
        {
            NumberIsVisible = true;
            NameIsVisible = false;
            ValidIsVisible = false;
            CvvIsVisible = false;

            ButtonText = "Siguiente";

            CardSimulationInfo = new ViewFlipper();

            LogoCreditCard = new Image();
        }

        private async void SaveNewPaymentMethodAsync()
        {
            if (ButtonText == "Guardar")
            {
                Save();
            }
            if (NumberIsVisible)
            {
                if (!ValidatedNumberNext())
                    return;

                NumberIsVisible = false;
                NameIsVisible = !NumberIsVisible;
                return;
            }

            if (NameIsVisible)
            {
                if (!ValidatedNameNext())
                    return;

                NameIsVisible = false;
                ValidIsVisible = !NameIsVisible;
                return;
            }

            if (ValidIsVisible)
            {
                if (!ValidatedValidNext())
                    return;

                ValidIsVisible = false;
                CvvIsVisible = !ValidIsVisible;
                CardSimulationInfo.FlipState = FlipState.Back;

                ButtonText = "Guardar";

                return;
            }

            if (CvvIsVisible)
            {
                if (!ValidatedCvvNext())
                    return;

                CvvIsVisible = false;
                NumberIsVisible = !CvvIsVisible;
                CardSimulationInfo.FlipState = FlipState.Front;

                ButtonText = "Siguiente";

            }
        }

        private bool ValidatedNumberNext()
        {
            NumberErrorMsgIsVisible = false;

            if (string.IsNullOrEmpty(Number) || !Number.Length.Equals(19))
            {
                NumberErrorMsgText = "Número de tarjeta de crédito no válido";
                NumberErrorMsgIsVisible = true;
                return false;
            }

            return true;
        }

        private bool ValidatedNameNext()
        {
            NameErrorMsgIsVisible = false;

            if (string.IsNullOrEmpty(Name) || !Name.Contains(" "))
            {
                NameErrorMsgText = "Nombre de titular no válido";
                NameErrorMsgIsVisible = true;
                return false;
            }

            return true;
        }

        private bool ValidatedValidNext()
        {
            ValidErrorMsgIsVisible = false;

            if (string.IsNullOrEmpty(Valid) || !Valid.Length.Equals(5))
            {
                ValidErrorMsgText = "Fecha de validez no válida";
                ValidErrorMsgIsVisible = true;
                return false;
            }

            var invalidDateMsg = "";

            var monthValid = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            var yearValid = new List<string>();

            for (int i = DateTime.UtcNow.Year; i < DateTime.UtcNow.AddYears(15).Year; i++)
            {
                yearValid.Add(i.ToString().Substring(2, 2));
            }

            if (!monthValid.Contains(Valid.Substring(0, 2)))
                invalidDateMsg += "Mes inválido\n";

            if (!yearValid.Contains(Valid.Substring(3, 2)))
                invalidDateMsg += "Año no válido";

            if (!string.IsNullOrEmpty(invalidDateMsg))
            {
                ValidErrorMsgText = invalidDateMsg;
                ValidErrorMsgIsVisible = true;
                return false;
            }

            return true;
        }

        private bool ValidatedCvvNext()
        {
            CvvErrorMsgIsVisible = false;

            if (string.IsNullOrEmpty(Cvv) || Cvv.Length < 3)
            {
                CvvErrorMsgText = "Cvv inválido";
                CvvErrorMsgIsVisible = true;
                return false;
            }

            return true;
        }
        private async void Save()
        {
            await _dialogService.DisplayAlertAsync("Ok", "Tarjeta Guardada", "Aceptar");
            await _navigationService.GoBackAsync();

        }

    }
}