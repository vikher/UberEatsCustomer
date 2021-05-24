using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Views;
using ClubersCustomerMobile.Prism.Views.CreditBalance;
using ClubersCustomerMobile.Prism.Views.Profile;
using ClubersCustomerMobile.Prism.Views.Profile.AvailableBalance;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private string _availableBalanceIcon = IconFont.AccountCash;
        private string _creditBalanceIcon = IconFont.CashPlus;
        private string _referIcon = IconFont.GiftOutline;
        private string _helpIcon = IconFont.HelpCircleOutline;
        private string _accountHistoryIcon = IconFont.History;
        private string _paymentMethodsIcon = IconFont.CreditCardOutline;
        private string _deliveryAddressIcon = IconFont.TruckDeliveryOutline;
        private string _notificationsIcon = IconFont.NotificationClearAll;
        private string _termsAndConditionsIcon = IconFont.FileDocumentOutline;
        private string _membershipIcon = IconFont.WalletMembership;
        private string _logoutIcon = IconFont.Logout;
        private string _qrCodeIcon = IconFont.Qrcode;
        private ImageSource _image;
        //private UserResponse _user;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _creditBalanceCommand;
        private DelegateCommand _availableBalanceCommand;
        private DelegateCommand _referCommand;
        private DelegateCommand _helpCommand;
        private DelegateCommand _accountHistoryCommand;
        private DelegateCommand _savedPaymentMethodsCommand;
        private DelegateCommand _savedAddressesCommand;
        private DelegateCommand _notificationsCommand;
        private DelegateCommand _termsAndConditionsCommand;
        private DelegateCommand _membershipCommand;
        private DelegateCommand _logoutCommand;

        public ProfilePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Perfil";
            Image = "https://clubersblob.blob.core.windows.net/users/profile.jpg";
        }
        public DelegateCommand CreditBalanceCommand => _creditBalanceCommand ?? (_creditBalanceCommand = new DelegateCommand(CreditBalanceAsync));
        public DelegateCommand AvailableBalanceCommand => _availableBalanceCommand ?? (_availableBalanceCommand = new DelegateCommand(AvailableBalanceAsync));
        public DelegateCommand ReferCommand => _referCommand ?? (_referCommand = new DelegateCommand(RefereAsync));
        public DelegateCommand HelpCommand => _helpCommand ?? (_helpCommand = new DelegateCommand(HelpAsync));
        public DelegateCommand AccountHistoryCommand => _accountHistoryCommand ?? (_accountHistoryCommand = new DelegateCommand(AccountHistoryAsync));
        public DelegateCommand TermsAndConditionsCommand => _termsAndConditionsCommand ?? (_termsAndConditionsCommand = new DelegateCommand(TermsAndConditionsAsync));
        public DelegateCommand SavedAddressesCommand => _savedAddressesCommand ?? (_savedAddressesCommand = new DelegateCommand(SavedAddressesAsync));
        public DelegateCommand SavedPaymentMethodsCommand => _savedPaymentMethodsCommand ?? (_savedPaymentMethodsCommand = new DelegateCommand(SavedPaymentMethodsAsync));
        public DelegateCommand NotificationCommand => _notificationsCommand ?? (_notificationsCommand = new DelegateCommand(NotificationsAsync));
        public DelegateCommand MembershipCommand => _membershipCommand ?? (_membershipCommand = new DelegateCommand(MembershipAsync));
        public DelegateCommand LogoutCommand => _logoutCommand ?? (_logoutCommand = new DelegateCommand(LogoutAsync));
        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));

        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public string QrCodeIcon
        {
            get => _qrCodeIcon;
            set => SetProperty(ref _qrCodeIcon, value);
        }
        public string AvailableBalanceIcon
        {
            get => _availableBalanceIcon;
            set => SetProperty(ref _availableBalanceIcon, value);
        }
        public string CreditBalanceIcon
        {
            get => _creditBalanceIcon;
            set => SetProperty(ref _creditBalanceIcon, value);
        }
        public string ReferIcon
        {
            get => _referIcon;
            set => SetProperty(ref _referIcon, value);
        }
        public string HelpIcon
        {
            get => _helpIcon;
            set => SetProperty(ref _helpIcon, value);
        }
        public string AccountHistoryIcon
        {
            get => _accountHistoryIcon;
            set => SetProperty(ref _accountHistoryIcon, value);
        }
        public string PaymentMethodsIcon
        {
            get => _paymentMethodsIcon;
            set => SetProperty(ref _paymentMethodsIcon, value);
        }
        public string DeliveryAddressIcon
        {
            get => _deliveryAddressIcon;
            set => SetProperty(ref _deliveryAddressIcon, value);
        }
        public string NotificationsIcon
        {
            get => _notificationsIcon;
            set => SetProperty(ref _notificationsIcon, value);
        }
        public string TermsAndConditionsIcon
        {
            get => _termsAndConditionsIcon;
            set => SetProperty(ref _termsAndConditionsIcon, value);
        }
        public string MembershipIcon
        {
            get => _membershipIcon;
            set => SetProperty(ref _membershipIcon, value);
        }
        public string LogoutIcon
        {
            get => _logoutIcon;
            set => SetProperty(ref _logoutIcon, value);
        }

        private async void TermsAndConditionsAsync()
        {
            await _navigationService.NavigateAsync(nameof(TermsAndConditionsPage));
        }

        private async void SavedAddressesAsync()
        {
            await _navigationService.NavigateAsync(nameof(SavedAddressesPage));
        }
        private async void AccountHistoryAsync()
        {
            await _navigationService.NavigateAsync(nameof(AccountHistoryTabbedPage));
        }

        private async void SavedPaymentMethodsAsync()
        {
            await _navigationService.NavigateAsync(nameof(SavedPaymentMethodsPage));
        }
        private async void RefereAsync()
        {
            await _navigationService.NavigateAsync(nameof(ReferPage));
        }
        private async void HelpAsync()
        {
            await _navigationService.NavigateAsync(nameof(HelpPage));
        }

        private async void NotificationsAsync()
        {
            await _navigationService.NavigateAsync(nameof(NotificationsPage));
        }
        private async void CreditBalanceAsync()
        {
            await _navigationService.NavigateAsync(nameof(CreditBalanceTabbedPage));
        }
        private async void AvailableBalanceAsync()
        {
            await _navigationService.NavigateAsync(nameof(AvailableBalancePage));
        }
        private async void MembershipAsync()
        {
            await _navigationService.NavigateAsync(nameof(MembershipPage));
        }

        private async void LogoutAsync()
        {
            if (Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.User = null;
                Settings.Token = null;
            }

            await _navigationService.NavigateAsync("/NavigationPage/LoginPage");
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
    }
}
