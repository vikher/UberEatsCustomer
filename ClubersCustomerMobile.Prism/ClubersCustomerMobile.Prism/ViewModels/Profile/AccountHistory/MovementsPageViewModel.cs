using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class MovementsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private double _total;
        //private Order _selectedOrder;
        private List<Refund> _refunds;
        //private DelegateCommand<Order> _selectedOrderCommand;
        public MovementsPageViewModel(INavigationService navigationService, IApiService apiService, IPageDialogService dialogService) : base(navigationService)
        {
            Title = "Movimientos";
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;
            LoadRefundsAsync();
           
        }

        //public DelegateCommand<Order> OrderSelectedCommand => _selectedOrderCommand ?? (_selectedOrderCommand = new DelegateCommand<Order>(OrderDetails));

        public List<Refund> Refunds
        {
            get => _refunds;
            set => SetProperty(ref _refunds, value);
        }

        public double Total
        {
            get
            {
                if (Refunds == null)
                {
                    _total = 0;
                }
                else
                {
                    _total = (Refunds.Sum(x => x.Amount));
                }
                return _total;
            }

            set => SetProperty(ref _total, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadRefundsAsync();
        }

        private async void LoadRefundsAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            UserResponse User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            ListResponse<Refund> response = await _apiService.GetListAsync<Refund>(Constants.urlBase, Constants.servicePrefix, Constants.GetRefundsByCustomerIdAsyncController, Constants.tokenType, token.Result.token, User.Result.Id);
            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }

            Refunds = (List<Refund>)response.Result;

            IsRunning = false;
        }
    }
}