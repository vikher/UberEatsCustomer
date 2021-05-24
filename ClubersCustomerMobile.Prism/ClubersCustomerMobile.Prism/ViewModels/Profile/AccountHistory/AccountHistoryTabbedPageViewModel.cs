using Prism.Navigation;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class AccountHistoryTabbedPageViewModel : ViewModelBase
    {
        public AccountHistoryTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Historial";
        }
    }
}

