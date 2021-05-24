using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class AvailableBalancePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private decimal _amount = 0;
        public ObservableCollection<AvailableBalance> _availableBalances;
        public ObservableCollection<AvailableBalance> AvailableBalances
        {
            get => _availableBalances;
            set => SetProperty(ref _availableBalances, value);
        }
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
        public AvailableBalancePageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Saldo disponible";
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadAvailableBalanceAsync();
        }
        private async void LoadAvailableBalanceAsync()
        {
            Response response = await _apiService.GetAvailableBalanceByUserIdAsync<AvailableBalance>(1, " / api", "/AvailableBalance", "bearer", "", "");
            var availableBalanceList = (List<AvailableBalance>)response.Result;
            AvailableBalances = new ObservableCollection<AvailableBalance>(availableBalanceList);
            foreach (var item in AvailableBalances)
            {
                Amount += item.Total;
            }
        }
    }
}
