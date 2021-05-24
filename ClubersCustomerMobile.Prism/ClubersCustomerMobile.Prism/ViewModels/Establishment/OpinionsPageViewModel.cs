using ClubersCustomerMobile.Prism.Services;
using Prism.Navigation;
using Prism.Services;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OpinionsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        public OpinionsPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Opiniones";
        }
    }
}

