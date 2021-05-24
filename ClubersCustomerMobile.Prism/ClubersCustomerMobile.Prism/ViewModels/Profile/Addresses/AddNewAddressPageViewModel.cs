using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class AddNewAddressPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;      
        private AddressType _addressType;
        private ObservableCollection<AddressType> _addressTypes;
        private DelegateCommand _saveNewAddressCommand;

        public AddNewAddressPageViewModel(INavigationService navigationService,
            IApiService apiService, 
            IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Agregar nueva dirección";
            LoadSavedAddressesAsync();
            LoadAddressTypesAsync();

        }
        public DelegateCommand SaveNewAddressCommand => _saveNewAddressCommand ?? (_saveNewAddressCommand = new DelegateCommand(SaveNewAddressAsync));
        public AddressType AddressType
        {
            get => _addressType;
            set => SetProperty(ref _addressType, value);
        }

        public ObservableCollection<AddressType> AddressTypes
        {
            get => _addressTypes;
            set => SetProperty(ref _addressTypes, value);
        }

        private async void LoadAddressTypesAsync()
        {

            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await _dialogService.DisplayAlertAsync("Error", "Compruebe la conexión a Internet", "Accept");
                return;
            }

            Response response = await _apiService.GetAddressTypes<AddressType>(Constants.urlBase, Constants.servicePrefix, Constants.controller, Constants.tokenType, Constants.accessToken);
            List<AddressType> list = (List<AddressType>)response.Result;
            AddressTypes = new ObservableCollection<AddressType>(list.OrderBy(t => t.Name));
        }



        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadSavedAddressesAsync();
        }

        private async void SaveNewAddressAsync()
        {
        }

        private async void LoadSavedAddressesAsync()
        {
        }
    }
}