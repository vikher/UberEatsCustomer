using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class SavedAddressesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private bool _isRefreshing;
        private ObservableCollection<Address> _addresses;
        private Address _selectedAddress;
        private Address _parameterAddress;

        private DelegateCommand _refreshViewCommand;
        private DelegateCommand _addNewAddressCommand;
        private DelegateCommand _goBackCommand;
        private DelegateCommand<Address> _deleteAddressCommand;
        private DelegateCommand<Address> _selectedAddressCommand;

        public SavedAddressesPageViewModel(INavigationService navigationService,
            IPageDialogService dialogService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Agregar dirección";
            //LoadSavedAddressesAsync();
        }
        public DelegateCommand<Address> DeleteAddressCommand => _deleteAddressCommand ?? (_deleteAddressCommand = new DelegateCommand<Address>(DeleteAddressAsync));

        public DelegateCommand AddNewAddressCommand => _addNewAddressCommand ?? (_addNewAddressCommand = new DelegateCommand(AddNewAddressAsync));

        public DelegateCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new DelegateCommand(GoBackAsync));

        public DelegateCommand RefreshViewCommand => _refreshViewCommand ?? (_refreshViewCommand = new DelegateCommand(RefreshData));
        public DelegateCommand<Address> SelectedAddressCommand => _selectedAddressCommand ?? (_selectedAddressCommand = new DelegateCommand<Address>(NavigateToPurchaseDetail));

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public Address SelectedAddress
        {
            get => _selectedAddress;
            set => SetProperty(ref _selectedAddress, value);
        }

        public Address ParameterAddress
        {
            get => _parameterAddress;
            set => SetProperty(ref _parameterAddress, value);
        }
        public ObservableCollection<Address> Addresses
        {
            get => _addresses;
            set => SetProperty(ref _addresses, value);
        }

        private async void AddNewAddressAsync()
        {
            await _navigationService.NavigateAsync(nameof(CustomerAddressPage));
        }

        private async void NavigateToPurchaseDetail(Address obj)
        {
            if (SelectedAddress == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "address", SelectedAddress }
            };

            await _navigationService.GoBackAsync(parameters);
        }

        private async void GoBackAsync()
        {
            if (SelectedAddress == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "address", SelectedAddress }
            };

            await _navigationService.GoBackAsync(parameters);
        }

        private async void LoadSavedAddressesAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            UserResponse User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            ListResponse<Address> response = await _apiService.GetListAsync<Address>(Constants.urlBase, Constants.servicePrefix, Constants.GetAllAddressesByUserIdController, Constants.tokenType, token.Result.token, User.Result.Id);

            if (response.ResultCode != ResultCode.Success)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }
            Addresses = new ObservableCollection<Address>((List<Address>)response.Result);

            IsRunning = false;
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadSavedAddressesAsync();

            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("address"))
            {
                ParameterAddress = parameters.GetValue<Address>("address");

                Addresses.Add(ParameterAddress);
            }
        }

        private void RefreshData()
        {
            LoadSavedAddressesAsync();
            IsRefreshing = false;
        }

        private async void DeleteAddressAsync(Address address)
        {
            Addresses.Remove(address);
        }
    }
}
