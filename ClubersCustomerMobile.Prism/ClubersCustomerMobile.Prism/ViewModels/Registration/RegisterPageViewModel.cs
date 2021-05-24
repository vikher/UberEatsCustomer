using ClubersCustomerMobile.Prism.Interfaces;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IFilesHelper _filesHelper;
        private readonly IApiService _apiService;
        private readonly IRegexHelper _regexHelper;
        private MediaFile _file;
        private UserRequest _user;
        private ImageSource _image;
        private string _emailIcon = IconFont.Email;
        private string _passwordIcon = IconFont.Lock;
        private string _fullnameIcon = IconFont.Account;
        private DelegateCommand _registerCommand;
        private DelegateCommand _changeImageCommand;

        public RegisterPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IRegexHelper regexHelper,
                                         IApiService apiService,
                                         IFilesHelper filesHelper) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            _regexHelper = regexHelper;
            _filesHelper = filesHelper;
            Title = "Registro";
            Image = "https://SoccerWeb4.azurewebsites.net/images/noimage.png";
            User = new UserRequest();
        }
        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));
        public DelegateCommand RegisterCommand => _registerCommand ?? (_registerCommand = new DelegateCommand(RegisterAsync));
        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public string FullNameIcon
        {
            get => _fullnameIcon;
            set => SetProperty(ref _fullnameIcon, value);
        }
        public string EmailIcon
        {
            get => _emailIcon;
            set => SetProperty(ref _emailIcon, value);
        }

        public string PasswordIcon
        {
            get => _passwordIcon;
            set => SetProperty(ref _passwordIcon, value);
        }

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.Email) || !_regexHelper.IsValidEmail(User.Email))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar un email.", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(User.Phone))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar un número de teléfono.", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(User.Password))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe ingresar una contraseña.", "Aceptar");
                return false;
            }

            if (User.Password.Length < 6)
            {
                await _dialogService.DisplayAlertAsync("Error", "La contraseña debe contener al menos 6 caracteres.", "Aceptar");
                return false;
            }

            if (User.Phone.Length != 10)
            {
                await _dialogService.DisplayAlertAsync("Error", "El numero telefonico debe ser de 10 digitos.", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(User.PasswordConfirm))
            {
                await _dialogService.DisplayAlertAsync("Error", "Debe confirmar la contraseña.", "Aceptar");
                return false;
            }

            if (!User.Password.Equals(User.PasswordConfirm))
            {
                await _dialogService.DisplayAlertAsync("Error", "La contraseña y la confirmación no coinciden.", "Aceptar");
                return false;
            }
            return true;
        }

        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet("PictureSource", "Cancelar",
                null, "FromGallery", "FromCamera");

            if (source == "Cancelar")
            {
                _file = null;
                return;
            }

            if (source == "FromCamera")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                Image = ImageSource.FromStream(() =>
                {
                    System.IO.Stream stream = _file.GetStream();
                    return stream;
                });
            }
        }
        private async void RegisterAsync()
        {
            bool isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            //IsEnabled = false;
            string url = "";
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                //IsEnabled = true;
                await _dialogService.DisplayAlertAsync("Error", "Compruebe la conexión a Internet", "Aceptar");
                return;
            }

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            IsRunning = false;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "User", User }
                    };

            await _navigationService.NavigateAsync(nameof(VerifyPhonePage), parameters);

        }
    }
}