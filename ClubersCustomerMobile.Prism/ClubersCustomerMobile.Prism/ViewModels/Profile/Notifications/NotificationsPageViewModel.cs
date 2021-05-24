using ClubersCustomerMobile.Prism.Services;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class NotificationsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        public NotificationsPageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Notificaciones";

            //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.Black;
            //((NavigationPage)Application.Current.MainPage).BarTextColor = Color.OrangeRed;
        }
    }
}
