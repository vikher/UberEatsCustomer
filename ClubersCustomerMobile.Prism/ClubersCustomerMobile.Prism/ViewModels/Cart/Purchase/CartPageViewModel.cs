using Prism.Navigation;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public CartPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Promociones";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
