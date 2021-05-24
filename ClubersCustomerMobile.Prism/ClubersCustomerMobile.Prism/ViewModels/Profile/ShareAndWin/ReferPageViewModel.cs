using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class ReferPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private string _referralCode = "CLBRS239ET";
        private string _contentCopyIcon = IconFont.ContentCopy;
        private DelegateCommand _shareCommand;
        private DelegateCommand _copyToClipboardCommand;
        public ReferPageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;
            Title = "¡Recomienda y gana!";
        }
        public DelegateCommand ShareCommand => _shareCommand ?? (_shareCommand = new DelegateCommand(ShareAsync));
        public DelegateCommand CopyToClipboardCommand => _copyToClipboardCommand ?? (_copyToClipboardCommand = new DelegateCommand(CopyToClipboardAsync));

        public string ReferralCode
        {
            get => _referralCode;
            set => SetProperty(ref _referralCode, value);
        }
        public string ContentCopyIcon
        {
            get => _contentCopyIcon;
            set => SetProperty(ref _contentCopyIcon, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }
        private async void ShareAsync()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = ReferralCode,
                Title = "Share Text"
            });
        }

        private async void CopyToClipboardAsync()
        {
            await Clipboard.SetTextAsync(ReferralCode);
            if (Clipboard.HasText)
            {
                var text = await Clipboard.GetTextAsync();
                await _dialogService.DisplayAlertAsync("Éxito", string.Format("Código {0} copiado", text), "Ok");
            }
        }
    }
}


