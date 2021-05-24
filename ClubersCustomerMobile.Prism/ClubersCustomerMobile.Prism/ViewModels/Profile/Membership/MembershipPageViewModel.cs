using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class MembershipPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private Membership _membership;

        public MembershipPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Membresía";
            LoadMembershipAsync();

        }

        public Membership Membership
        {
            get => _membership;
            set => SetProperty(ref _membership, value);
        }

        private async void LoadMembershipAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }
            UserResponse User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            Response1<Membership> response = await _apiService.GetMembershipAsync(Constants.urlBase, Constants.servicePrefix, Constants.GetMembershipByCustomerIdController, Constants.tokenType, token.Result.token, User.Result.Id);

            Membership = response.Result;

            IsRunning = false;
        }
    }
}
