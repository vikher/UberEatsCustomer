using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OrdersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private DateTime _startDate;
        private DateTime _endDate;
        private Order _selectedOrder;
        private List<Order> _orders;
        private DelegateCommand<Order> _selectedOrderCommand;
        private DelegateCommand _filterCommand;
        public OrdersPageViewModel(INavigationService navigationService, IApiService apiService, IPageDialogService dialogService) : base(navigationService)
        {
            Title = "Ordenes";
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;
            LoadOrdersAsync();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
        //public DelegateCommand FilterCommand => _filterCommand ?? (_filterCommand = new DelegateCommand(FiltersAsync));

        public DelegateCommand<Order> OrderSelectedCommand => _selectedOrderCommand ?? (_selectedOrderCommand = new DelegateCommand<Order>(OrderDetails));

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }
        public List<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadOrdersAsync();
        }

        private async void LoadOrdersAsync()
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

            ListResponse<Order> response = await _apiService.GetListAsync<Order>(Constants.urlBase, Constants.servicePrefix, Constants.GetAllOrdersByCustomerIdAsyncController, Constants.tokenType, token.Result.token, User.Result.Id);

            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }
            Orders = (List<Order>)response.Result;

            IsRunning = false;
        }

        private async void OrderDetails(Order obj)
        {
            if (SelectedOrder == null)
                return;
            var parameters = new NavigationParameters
            {
                { "orderdetail", SelectedOrder }
            };
            await _navigationService.NavigateAsync(nameof(OrderDetailPage), parameters);
            SelectedOrder = null;
        }
        //private async void FiltersAsync()
        //{
        //    Response response = await _apiService.GetAllOrdersAsync<Order>(Constants.urlBase, Constants.servicePrefix, Constants.controller, Constants.tokenType, Constants.accessToken);
        //    Orders = (List<Order>)response.Result;
        //    Orders = new List<Order>(_orders.Where(t => (t.StartDate.Date >= StartDate.Date && t.StartDate.Date <= EndDate.Date)));
        //}
    }
}
