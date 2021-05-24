using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class WithoutEvaluatingNorSharingPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        public ObservableCollection<CreditBalance> _creditBalances;
        public WithoutEvaluatingNorSharingPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Sin evaluar";
            LoadCreditBalanceAsync();
        }
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

            var BalanceWithFilter = creditBalanceList.Where(c => c.NameStatus == "SIN EVALUAR Y SIN COMPARTIR").ToList(); 
            CreditBalance = new ObservableCollection<CreditBalance>(BalanceWithFilter);
        }
    }
}
