using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class UpdatePasswordPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private string _password;
        private string _passwordConfirm;
        private DelegateCommand _updatePasswordCommand;

        public UpdatePasswordPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Actualizar contraseña";
        }
        public DelegateCommand UpdatePasswordCommand => _updatePasswordCommand ?? (_updatePasswordCommand = new DelegateCommand(UpdatePasswordAsync));

        public string Email { get; set; }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set => SetProperty(ref _passwordConfirm, value);
        }
        

        private async Task<bool> ValidateDataAsync()
        {

            if (string.IsNullOrEmpty(Password))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar una contraseña.", "Aceptar");
                return false;
            }

            if (Password.Length < 8)
            {
                await _dialogService.DisplayAlertAsync("Error", "La contraseña debe contener al menos 8 caracteres.", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(PasswordConfirm))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe confirmar la contraseña.", "Aceptar");
                return false;
            }

            if (!Password.Equals(PasswordConfirm))
            {
                await _dialogService.DisplayAlertAsync("Error", "La contraseña y la confirmación no coinciden.", "Aceptar");
                return false;
            }
            return true;
        }
        private async void UpdatePasswordAsync()
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

            await _dialogService.DisplayAlertAsync("Ok", "Contraseña Actualizada con Exito", "Aceptar");
            await NavigationService.NavigateAsync("/NavigationPage/LoginPage");

        }
    }
}