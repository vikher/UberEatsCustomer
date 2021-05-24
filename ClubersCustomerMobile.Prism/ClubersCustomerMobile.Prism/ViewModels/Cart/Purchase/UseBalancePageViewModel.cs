using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class UseBalancePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private Order _shoppingCartOrder;
        private decimal _availableBalance;
        private decimal _selectedBalance;
        private DelegateCommand _checkoutCommand;
        public UseBalancePageViewModel(INavigationService navigationService,
            IApiService apiService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;
            Title = "Utilizar Saldo Clubers";
            LoadBalanceAsync();
        }
        public DelegateCommand CheckoutCommand => _checkoutCommand ?? (_checkoutCommand = new DelegateCommand(CheckoutAsynx));
        public decimal AvailableBalance
        {
            get => _availableBalance;
            set => SetProperty(ref _availableBalance, value);
        }
        public decimal SelectedBalance
        {
            get => _selectedBalance;
            set => SetProperty(ref _selectedBalance,value, () => RaisePropertyChanged(nameof(RemainingBalance)));
        }
        public decimal RemainingBalance => (AvailableBalance - SelectedBalance);
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        private async void CheckoutAsynx()
        {
            if (ShoppingCartOrder == null)
                return;

            if (IsNegative(RemainingBalance))
            {
                await _dialogService.DisplayAlertAsync("Error", "Tu saldo no puede ser negativo.", "Aceptar");
                return;
            }

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };
            await _navigationService.NavigateAsync(nameof(PinPage), parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters2)
        {
            base.OnNavigatedTo(parameters2);

            if (parameters2.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters2.GetValue<Order>("order");

            }
            LoadBalanceAsync();
        }
        private void LoadBalanceAsync()
        {
            AvailableBalance = 1000.5m;
        }
        private bool IsNegative(decimal number)
        {
            return number < 0;
        }
    }
}
