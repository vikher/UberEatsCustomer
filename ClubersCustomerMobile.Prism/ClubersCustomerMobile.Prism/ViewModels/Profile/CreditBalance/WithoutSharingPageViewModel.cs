using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class WithoutSharingPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        public ObservableCollection<CreditBalance> _creditBalances;
        private DelegateCommand _shareOptionsCommand;
        public WithoutSharingPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService )
        {

            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Sin compartir";
            LoadCreditBalanceAsync();

        }
        public DelegateCommand ShareOptionsCommand => _shareOptionsCommand ?? (_shareOptionsCommand = new DelegateCommand(ShareOptionsAsync));
        public ObservableCollection<CreditBalance> CreditBalance
        {
            get => _creditBalances;
            set => SetProperty(ref _creditBalances, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
        private async void LoadCreditBalanceAsync()
        {
            Response response = await _apiService.GetCreditBalanceByUserIdAsync<CreditBalance>(1, " / api", "/CreditBalance", "bearer", "", "");
            var creditBalanceList = (List<CreditBalance>)response.Result;
            var BalanceWithFilter = creditBalanceList.Where(c => c.NameStatus == "SIN COMPARTIR").ToList();
            CreditBalance = new ObservableCollection<CreditBalance>(BalanceWithFilter);
        }
        private async void ShareOptionsAsync()
        {
            await _navigationService.NavigateAsync($"NavigationPage/{nameof(ShareOptionsPage)}", useModalNavigation: true);
        }
    }
}
