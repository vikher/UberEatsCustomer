using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class FreeTrialPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private string _giftIconTrial = IconFont.Gift;
        private UserRequest _user;

        private DelegateCommand _customerTabbedCommand;
        private DelegateCommand _completeDataCommand;

        public FreeTrialPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = " Bienvenido a Clubers";
        }
        public DelegateCommand CustomerTabbedCommand => _customerTabbedCommand ?? (_customerTabbedCommand = new DelegateCommand(NavigateCustomertabbedAsync));
        public DelegateCommand CompleteDataCommand => _completeDataCommand ?? (_completeDataCommand = new DelegateCommand(CompleteDataAsync));

        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("User"))
            {
                User = parameters.GetValue<UserRequest>("User");
            }
        }
        public string GiftIconTrial
        {
            get => _giftIconTrial;
            set => SetProperty(ref _giftIconTrial, value);
        }
        private async void NavigateCustomertabbedAsync()
        {
            await NavigationService.NavigateAsync("/NavigationPage/CustomerTabbedPage?selectedTab=HomePage");
        }     
        private async void CompleteDataAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "User", User }
            };

            await _navigationService.NavigateAsync(nameof(CompleteDataPage), parameters);
        }

    }
}
