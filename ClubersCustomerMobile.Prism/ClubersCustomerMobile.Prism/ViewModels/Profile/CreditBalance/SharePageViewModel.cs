using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class SharePageViewModel : ViewModelBase, IConfirmNavigationAsync
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        public string ImageLink;
        public string _referralText;
        public bool _isShareVisible;
        public bool _isChargeVisible;
        private DelegateCommand _shareCommand;
        private DelegateCommand _confirmCommand;
        public SharePageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Compartir";
            _isShareVisible = true;
            _isChargeVisible = false;
            _referralText = "Recibí 50% de reembolso de mi consumo en Hamburguesas El Jefe(FanPage) por pedir a Domicilio con #Clubers. Inscríbete Aquí";
            ImageLink = "https://firebasestorage.googleapis.com/v0/b/fbtest-1eec8.appspot.com/o/rapida.png?alt=media&token=40ae8691-43b4-4f70-8a06-3435a3c97acd";
        }
        public DelegateCommand ShareCommand => _shareCommand ?? (_shareCommand = new DelegateCommand(ShareTextAsync));
        public DelegateCommand ConfirmCommand => _confirmCommand ?? (_confirmCommand = new DelegateCommand(NavigateCreditBalanceConfirmAsync));
        public bool IsChargeVisible
        {
            get => _isChargeVisible;
            set => SetProperty(ref _isChargeVisible, value);
        }
        public bool IsShareVisible
        {
            get => _isShareVisible;
            set => SetProperty(ref _isShareVisible, value);
        }
        public string ReferralText
        {
            get => _referralText;
            set => SetProperty(ref _referralText, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }
        public byte[] GetBytes(string url)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadData(url);
            }
        }
        public async void ShareTextAsync()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ReferralText,
                Title = "Share Text"
            });

            IsShareVisible = false;
            IsChargeVisible = true;
        }

        private async void ShareImageAsync()
        {
            try
            {
                string filename = "temp.jpg";
                //***
                //*** Get the file name from the link
                //***
                Uri uri = new Uri(ImageLink);
                if (uri.IsFile)
                {
                    filename = System.IO.Path.GetFileName(uri.LocalPath);
                }
                //***
                //*** Build a temp path for the file
                //***
                string tempFileName = Path.Combine(FileSystem.CacheDirectory, filename);
                //***
                //*** Delete the file if exist in the system
                //*** 
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
                //***
                //*** Write the file to the directory
                //***
                File.WriteAllBytes(tempFileName, GetBytes(ImageLink));
                //***
                //*** Share file
                //***
                await Share.RequestAsync(new ShareFileRequest()
                {
                    File = new ShareFile(tempFileName),
                    Title = "Sharing image",
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _dialogService.DisplayAlertAsync("Error", $"Algo Fallo. {ex.Message}", "OK");
            }
        }

        //public Task<bool> CanNavigateAsync(INavigationParameters parameters)
        //{
        //    var answer = _dialogService.DisplayAlertAsync("Save", "Would you like to save?", "Save", "Cancel");
        //    bool isValid = answer.Result;
        //    if (isValid == true)
        //    {

        //    }

        //    return answer;
        //}

        private async void NavigateCreditBalanceConfirmAsync()
        {
            await _navigationService.NavigateAsync(nameof(LoginPage));
        }

        public Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            return _dialogService.DisplayAlertAsync("Save", "Would you like to save?", "Save", "Cancel");
        }
    }
}

