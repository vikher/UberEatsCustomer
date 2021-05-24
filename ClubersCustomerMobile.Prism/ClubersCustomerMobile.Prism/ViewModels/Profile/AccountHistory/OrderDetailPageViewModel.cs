using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Navigation;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OrderDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Order _order;

        private bool _isRunning;

        public OrderDetailPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Detalle Domicilio";
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        public Order Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("orderdetail"))
            {
                Order = parameters.GetValue<Order>("orderdetail");

            }
        }
    }
}