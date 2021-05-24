using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OrderAtHomePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;   
        private Order _shoppingCartOrder;
        private Delivery _delivery;
        private ObservableCollection<Product> _products;
        private string _locationIcon = IconFont.MapMarker;
        private DelegateCommand _purchaseThankingMessageCommand;

        public OrderAtHomePageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Orden en domicilio";
        }
        public DelegateCommand PurchaseThankingMessageCommand => _purchaseThankingMessageCommand ?? (_purchaseThankingMessageCommand = new DelegateCommand(PurchaseThankingMessageAsync));
        public string LocationIcon
        {
            get => _locationIcon;
            set => SetProperty(ref _locationIcon, value);
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        public Delivery Delivery
        {
            get => _delivery;
            set => SetProperty(ref _delivery, value);
        }   
        private async void PurchaseThankingMessageAsync()
        {
            if (ShoppingCartOrder == null)
                return;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };

            await _navigationService.NavigateAsync(nameof(PurchaseThankingMessagePage), parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                Delivery = ShoppingCartOrder.Delivery;
                Products = new ObservableCollection<Product>(ShoppingCartOrder.Products);

            }
        }
    }
}
