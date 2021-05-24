using ClubersCustomerMobile.Prism.Interfaces;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class PinConfigurationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private readonly IRegexHelper _regexHelper;
        private string _pin;
        private string _pinConfirm;
        private string _lockIcon = IconFont.Lock;
        private DelegateCommand _updatePINCommand;

        public PinConfigurationPageViewModel(INavigationService navigationService,
                                 IPageDialogService dialogService,
                                 IRegexHelper regexHelper,
                                 IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            Title = "Recuperar PIN";
        }

        public DelegateCommand UpdatePINCommand => _updatePINCommand ?? (_updatePINCommand = new DelegateCommand(UpdatePINAsync));

        public string PIN
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }
        public string LockIcon
        {
            get => _lockIcon;
            set => SetProperty(ref _lockIcon, value);
        }

        public string PINConfirm
        {
            get => _pinConfirm;
            set => SetProperty(ref _pinConfirm, value);
        }
        private async Task<bool> ValidateDataAsync()
        {

            if (string.IsNullOrEmpty(PIN) || !_regexHelper.IsValidPIN(PIN))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar un PIN valido.", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(PINConfirm) || !_regexHelper.IsValidPIN(PINConfirm))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe confirmar el PIN.", "Aceptar");
                return false;
            }

            if (!PIN.Equals(PINConfirm))
            {
                await _dialogService.DisplayAlertAsync("Error", "Los PIN no coinciden.", "Aceptar");
                return false;
            }
            return true;
        }
        private async void UpdatePINAsync()
        {

            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                //IsEnabled = true;
                await _dialogService.DisplayAlertAsync("Error", "Compruebe la conexión a Internet", "Aceptar");
                return;
            }

            IsRunning = false;

            await _navigationService.NavigateAsync("../../");
        }
    }
}
