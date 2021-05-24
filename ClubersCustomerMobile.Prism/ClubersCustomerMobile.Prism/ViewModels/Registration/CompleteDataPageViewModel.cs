using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Interfaces;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CompleteDataPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private readonly IRegexHelper _regexHelper;

        private ImageSource _image;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _continueCommand;
        private bool _isRunning;
        private bool _isEnabled;
        private UserRequest _user;
        private SecretQuestion _secretQuestion;
        private ObservableCollection<SecretQuestion> _secretQuestions;
        public CompleteDataPageViewModel(INavigationService navigationService,
            IRegexHelper regexHelper,
            IFilesHelper filesHelper,
            IApiService apiService) : base(navigationService)
        {
            _filesHelper = filesHelper;
            _navigationService = navigationService;
            _regexHelper = regexHelper;
            _apiService = apiService;
            Title = "   Completar datos";
            Image = "https://clubersblob.blob.core.windows.net/users/profile.jpg";
            LoadSecretQuestionsAsync();


        }
        public DelegateCommand ContinueCommand => _continueCommand ?? (_continueCommand = new DelegateCommand(ContinueAsync));
        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));
        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public SecretQuestion SecretQuestion
        {
            get => _secretQuestion;
            set => SetProperty(ref _secretQuestion, value);
        }

        public ObservableCollection<SecretQuestion> SecretQuestions
        {
            get => _secretQuestions;
            set => SetProperty(ref _secretQuestions, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);


            if (parameters.ContainsKey("User"))
            {
                User = parameters.GetValue<UserRequest>("User");
            }


        }

        private async void LoadSecretQuestionsAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            string token1  = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJqYWlyLmdhcmNpYUBncnVwb3BlcnRpLmNvbS5teCIsImp0aSI6ImY5NWM3NDczLTk1MzgtNGFiMi04NjVlLWJhMDhjMzIxNGFhNSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlNvY2lvIFByb3ZlZWRvciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImphaXIuZ2FyY2lhQGdydXBvcGVydGkuY29tLm14IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiamFpci5nYXJjaWFAZ3J1cG9wZXJ0aS5jb20ubXgiLCJuYmYiOjE2MDExMzYxNDIsImV4cCI6MTYzMjY3MjE0MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo0OTc2MyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NDk3NjMifQ.sFJwmplwyq_M0Q1LnYX2t_KTVAHna8piJIV5GtVCphc";

            ListResponse<SecretQuestion> response = await _apiService.GetListAsync<SecretQuestion>(Constants.urlBase, Constants.servicePrefix, Constants.GetSecretQuestionsAsyncController, Constants.tokenType, token1, 0);
            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }

            var sc = (List<SecretQuestion>)response.Result;
            SecretQuestions = new ObservableCollection<SecretQuestion>(sc.OrderBy(c => c.SecretQuestionId));

            IsRunning = false;

        }
        private async void ContinueAsync()
        {
            //await NavigationService.NavigateAsync("/NavigationPage/CustomerTabbedPage?selectedTab=HomePage");
            await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
        }

        private async void ChangeImageAsync()
        {
            /*if (!IsTaxiUser)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ChangePhotoNoTaxiUser, Languages.Accept);
                return;
            }*/

            await CrossMedia.Current.Initialize();

            string source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you want to get the picture from?",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "From Camera")
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
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = _filesHelper.ReadFully(_file.GetStream());
            }

            //await _geolocatorService.GetLocationAsync();
            //if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
            //{
            //    User.Latitude = _geolocatorService.Latitude;
            //    User.Logitude = _geolocatorService.Longitude;
            //}

            User.PictureArray = imageArray;

            Response response = await _apiService.RegisterUserAsync("", "/api", "/Account/Register", User);
            IsRunning = false;
            IsEnabled = true;


            if (response.ResultCode != ResultCode.Success)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);

                return;
            }

            await App.Current.MainPage.DisplayAlert(Constants.AcceptMessage, Constants.RegisterMessge, Constants.AcceptMessage);
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.Document))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.DocumentError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.FirstNameError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.LastNameError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.AddressError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.Email) || !_regexHelper.IsValidEmail(User.Email))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.EmailError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.Phone))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.PhoneError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.NIP))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.NIPError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.SecretAnswer))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.SecretAnswerError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.Password) || User.Password?.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.PasswordError, Constants.AcceptMessage);
                return false;
            }

            if (string.IsNullOrEmpty(User.PasswordConfirm))
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.PasswordConfirmError1, Constants.AcceptMessage);
                return false;
            }

            if (User.Password != User.PasswordConfirm)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.PasswordConfirmError2, Constants.AcceptMessage);
                return false;
            }

            return true;
        }
    }
}
