using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CustomerAddressPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IGeolocatorService _geolocatorService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private double Latitude;
        private double Logitude;
        private bool _isEnabled;
        private Position _position;
        private Address _myAddress;
        private string _source;
        private AddressType _addressType;
        private ObservableCollection<AddressType> _addressTypes;
        private DelegateCommand _addAddressCommand;
        private DelegateCommand _getAddressCommand;
        public CustomerAddressPageViewModel(INavigationService navigationService, 
            IGeolocatorService geolocatorService, 
            IApiService apiService,
            IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _geolocatorService = geolocatorService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Dirección de entrega";
        }
        public DelegateCommand AddAddressCommand => _addAddressCommand ?? (_addAddressCommand = new DelegateCommand(AddAddressAsync));
        public DelegateCommand GetAddressCommand => _getAddressCommand ?? (_getAddressCommand = new DelegateCommand(LoadSourceAsync));
        public AddressType AddressType
        {
            get => _addressType;
            set => SetProperty(ref _addressType, value);
        }
        public Address MyAddress
        {
            get => _myAddress;
            set => SetProperty(ref _myAddress, value);
        }
        public ObservableCollection<AddressType> AddressTypes
        {
            get => _addressTypes;
            set => SetProperty(ref _addressTypes, value);
        }
        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            LoadSourceAsync();
            LoadAddressTypesAsync();
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
        private async void LoadSourceAsync()
        {
            IsRunning = true;
            IsEnabled = false;
            await _geolocatorService.GetLocationAsync();

            if (_geolocatorService.Latitude == 0 && _geolocatorService.Longitude == 0)
            {
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error" , "GeolocationError", "Accept");
                await _navigationService.GoBackAsync();
                return;
            }

            _position = new Position(_geolocatorService.Latitude, _geolocatorService.Longitude);
            Geocoder geoCoder = new Geocoder();
            IEnumerable<string> sources = await geoCoder.GetAddressesForPositionAsync(_position);
            List<string> addresses = new List<string>(sources);

            if (addresses.Count > 1)
            {
                Source = addresses[0];
            }

            CustomerAddressPage.GetInstance().AddPin(_position, Source, "StartTrip", PinType.Place);

            IsRunning = false;
            IsEnabled = true;
        }

        private async void AddAddressAsync()
        {

            IsRunning = true;
            IsEnabled = false;
            await _geolocatorService.GetLocationAsync();

            try
            {
                if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
                {
                    Latitude = _geolocatorService.Latitude;
                    Logitude = _geolocatorService.Longitude;
                }

                var placemarks = await Geocoding.GetPlacemarksAsync(Latitude, Logitude);

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var geocodeAddress =
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";

                    string fullAddress = $"{placemark.SubThoroughfare} {placemark.Thoroughfare} {placemark.SubAdminArea} {placemark.Locality} {placemark.CountryCode} {placemark.PostalCode}";

                    MyAddress = new Address() { AddressId = 3, GeoAddress = fullAddress, FullAddress = fullAddress, AddressType = "Trabajo", Comments = "Trabajo azul" };
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                await App.Current.MainPage.DisplayAlert("Error", fnsEx.ToString(), "Aceptar");
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
                await App.Current.MainPage.DisplayAlert("Error", ex.ToString(), "Aceptar");
            }

            IsRunning = false;
            IsEnabled = true;

            if (MyAddress == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "address", MyAddress }
            };

            await _navigationService.GoBackAsync(parameters);
        }

    }
}
