using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CreditBalancePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private CreditBalance _selectedCb;
        public ObservableCollection<CreditBalance> _creditBalances;
        private DelegateCommand<CreditBalance> _selectCbCommand;

        private DelegateCommand _shareCommand;
        public CreditBalancePageViewModel(INavigationService navigationService,
                        IPageDialogService dialogService,
            IApiService apiService) : base(navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Acreditar Saldo";
        }
        public DelegateCommand<CreditBalance> CBSelectedCommand => _selectCbCommand ?? (_selectCbCommand = new DelegateCommand<CreditBalance>(NavigateToShareAsync));

        public CreditBalance SelectedCb
        {
            get => _selectedCb;
            set => SetProperty(ref _selectedCb, value);
        }
        public ObservableCollection<CreditBalance> CreditBalance
        {
            get => _creditBalances;
            set => SetProperty(ref _creditBalances, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadCreditBalanceAsync();
        }
        private async void LoadCreditBalanceAsync()
        {
            Response response = await _apiService.GetCreditBalanceByUserIdAsync<CreditBalance>(1, " / api", "/CreditBalance", "bearer", "", "");
            var creditBalanceList = (List<CreditBalance>)response.Result;
            CreditBalance = new ObservableCollection<CreditBalance>(creditBalanceList);
        }

        private async void NavigateToShareAsync(CreditBalance obj)
        {
            if (SelectedCb == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "CreditBalance", SelectedCb }
            };

            await _navigationService.NavigateAsync(nameof(SharePage), parameters);
            SelectedCb = null;
        }
    }
}

