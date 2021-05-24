using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class ExplorePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Establishment _selectedEstablishment;
        private bool _isRefreshing;
        private string _search;
        private int _condition;
        private bool _onSite;
        private bool _homeDelivery;
        private string _locationIcon1 = IconFont.MapMarker;
        private string _shoppingCartIcon = IconFont.Cart;
        private Address _myAddress;
        private List<Establishment> _myEstablishments;
        private ObservableCollection<Establishment> _establishments;
        private DelegateCommand<Establishment> _selectEstablishmentCommand;
        private DelegateCommand<Subcategories> _selectedSubcategoryCommand;
        private DelegateCommand<Categories> _selectedCategoryCommand;
        private DelegateCommand _refreshViewCommand;
        private DelegateCommand _searchCommand;
        private DelegateCommand _filterCommand;
        private DelegateCommand _filterOnSiteCommand;
        private DelegateCommand _filterHomeDeliveryCommand;
        private List<Categories> _categories;
        private List<Subcategories> _subcategories;
        private Categories _selectedCategory;
        private Subcategories _selectedSubcategory;
        private UserResponse _user;
        private string _filterIcon = IconFont.Filter;
        private readonly IGeolocatorService _geolocatorService;
        private double Latitude;
        private double Logitude;
         
        public ExplorePageViewModel(INavigationService navigationService,
        IGeolocatorService geolocatorService,
        IApiService apiService) : base(navigationService)
        {
            _myEstablishments = new List<Establishment>();
            _navigationService = navigationService;
            _geolocatorService = geolocatorService;

            _apiService = apiService;
            Title = "Explorar";
            OnSite = true;
            HomeDelivery = false;
            LoadUser();
            LoadLocation();
            //LoadCategoriesAsync();
            //LoadEstablishmentsAsync();
            //LoadAddressAsync();
        }

        public DelegateCommand<Subcategories> SubcategorySelectedCommand => _selectedSubcategoryCommand ?? (_selectedSubcategoryCommand = new DelegateCommand<Subcategories>(LoadEstablishmentsBySubcategories));
        public DelegateCommand<Categories> CategorySelectedCommand => _selectedCategoryCommand ?? (_selectedCategoryCommand = new DelegateCommand<Categories>(LoadSubCategoryByCategory));
        public DelegateCommand<Establishment> EstablishmentSelectedCommand => _selectEstablishmentCommand ?? (_selectEstablishmentCommand = new DelegateCommand<Establishment>(NavigateToProductsAsync));
        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowEstablishments));
        public DelegateCommand FilterCommand => _filterCommand ?? (_filterCommand = new DelegateCommand(ShowFiltersAsync));
        public DelegateCommand FilterOnSiteCommand => _filterOnSiteCommand ?? (_filterOnSiteCommand = new DelegateCommand(FilterEstablishmentsOnSiteAsync));
        public DelegateCommand FilterHomeDeliveryCommand => _filterHomeDeliveryCommand ?? (_filterHomeDeliveryCommand = new DelegateCommand(FilterEstablishmentsHomeDeliveryAsync));
        public DelegateCommand RefreshViewCommand => _refreshViewCommand ?? (_refreshViewCommand = new DelegateCommand(RefreshData));
        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string FilterIcon
        {
            get => _filterIcon;
            set => SetProperty(ref _filterIcon, value);
        }
        public string LocationIcon1
        {
            get => _locationIcon1;
            set => SetProperty(ref _locationIcon1, value);
        }
        public string ShoppingCartIcon
        {
            get => _shoppingCartIcon;
            set => SetProperty(ref _shoppingCartIcon, value);
        }
        public Establishment SelectedEstablishment
        {
            get => _selectedEstablishment;
            set => SetProperty(ref _selectedEstablishment, value);
        }
        public Address MyAddress
        {
            get => _myAddress;
            set => SetProperty(ref _myAddress, value);
        }
        public ObservableCollection<Establishment> Establishments
        {
            get => _establishments;
            set => SetProperty(ref _establishments, value);
        }
        public Categories SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        public bool OnSite
        {
            get => _onSite;
            set => SetProperty(ref _onSite, value);
        }
        public bool HomeDelivery
        {
            get => _homeDelivery;
            set => SetProperty(ref _homeDelivery, value);
        }
        public Subcategories SelectedSubcategory
        {
            get => _selectedSubcategory;
            set => SetProperty(ref _selectedSubcategory, value);
        }
        public List<Categories> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        public List<Subcategories> SubCategories
        {
            get => _subcategories;
            set => SetProperty(ref _subcategories, value);
        }
        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowEstablishments();
            }
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        public int Condition
        {
            get => _condition;
            set => SetProperty(ref _condition, value);
        }
        private void RefreshData()
        {
            LoadEstablishmentsAsync();
            IsRefreshing = false;
        }

        private async void LoadLocation()
        {
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

                    string FullAddress = $"{placemark.SubThoroughfare} {placemark.Thoroughfare} {placemark.SubAdminArea} {placemark.Locality} {placemark.CountryCode} {placemark.PostalCode}";

                    MyAddress = new Address() { GeoAddress = FullAddress };
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, fnsEx.ToString(), Constants.AcceptMessage);

            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, ex.ToString(), Constants.AcceptMessage);

            }
        }

        private void LoadEstablishmentsBySubcategories(Subcategories obj)
        {
            if (SelectedSubcategory == null)
                return;
            Establishments = new ObservableCollection<Establishment>(SelectedSubcategory.Establishments);  

        }
        private void LoadSubCategoryByCategory(Categories obj)
        {
            if (SelectedCategory == null)
                return;
            SubCategories = SelectedCategory.Subcategories;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            
            if (parameters.ContainsKey("condition"))
            {
                Condition = parameters.GetValue<int>("condition");

                FilterEstablishmentsAsync(Condition);
            }
            else
            {
                LoadCategoriesAsync();
                LoadEstablishmentsAsync();
            }

        }
        private async void LoadCategoriesAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            ListResponse<Categories> response = await _apiService.GetListAsync<Categories>(Constants.urlBase, Constants.servicePrefix, Constants.GetAllCategoriesAsyncController, Constants.tokenType, token.Result.token, 0);

            if (response.ResultCode != ResultCode.Success)
            {
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }

            Categories = (List<Categories>)response.Result;

            IsRunning = false;
        }
        private async void FilterEstablishmentsAsync(int condition)
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }
            LoadCategoriesAsync();

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            ListResponse<Establishment> response = await _apiService.GetListAsync<Establishment>(Constants.urlBase, Constants.servicePrefix, Constants.GetAllEstablishmentsAsyncController, Constants.tokenType, token.Result.token, 0);
            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }

            _myEstablishments = (List<Establishment>)response.Result;

            int caseSwitch = condition;

            switch (caseSwitch)
            {
                case 1:
                    //id 1,Reembolso de bienvenida
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.Refunds.All(c => c.RefundId == 1)));
                    break;
                case 2:
                    //id 2 Reembolso recurrente en sitio
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.Refunds.All(c => c.RefundId == 2)));
                    break;
                case 3:
                    //id 3 Menor tiempo de entrega
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.OrderBy(r => r.DeliveryEstimatedTime));
                    break;
                case 4:
                    //id 4 Menor costo a domicilio
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.OrderBy(r => r.HomeCost));
                    break;
                case 5:
                    //Mejor calificacion
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.OrderByDescending(r => r.Rating));
                    break;
                case 6:
                    //id 6 Restaurantes cercanos
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.OrderBy(r => r.Distance));
                    break;
                case 7:
                    //id 7 Restaurantes con servicio en sitio
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.OnSite == true));
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            IsRunning = false;

        }
        private void FilterEstablishmentsOnSiteAsync()
        {
            OnSite = true;
            HomeDelivery = false;
            Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.OnSite == true));
        }
        private void FilterEstablishmentsHomeDeliveryAsync()
        {
            OnSite = false;
            HomeDelivery = true;
            Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.OnSite == false));
        }
        private async void LoadEstablishmentsAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            ListResponse<Establishment> response = await _apiService.GetListAsync<Establishment>(Constants.urlBase, Constants.servicePrefix, Constants.GetAllEstablishmentsAsyncController, Constants.tokenType, token.Result.token, 0);
            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }
            _myEstablishments = (List<Establishment>)response.Result;
            Establishments = new ObservableCollection<Establishment>(_myEstablishments);

            IsRunning = false;

            //ShowEstablishments();
        }
        private void ShowEstablishments()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Establishments = new ObservableCollection<Establishment>(_myEstablishments);
                if (OnSite)
                {
                    OnSite = true;
                    HomeDelivery = false;
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.OnSite == true));
                }
                else  if (OnSite ==false)
                {
                    OnSite = false;
                    HomeDelivery = true;
                    Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.OnSite == false));
                }
            }
            else
            {
                Establishments = new ObservableCollection<Establishment>(_myEstablishments.Where(r => r.Name.ToUpper().Contains(Search.ToUpper())));
            }
        }
        private async void NavigateToProductsAsync(Establishment obj)
        {
            if (SelectedEstablishment == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "Establishment", SelectedEstablishment }
            };

            if (HomeDelivery)
            {
                var errors = await _navigationService.NavigateAsync(nameof(ProductsPage), parameters);
            } else if (OnSite)
            {
                var errors = await _navigationService.NavigateAsync(nameof(OnSiteProductsPage), parameters);
            }
                
            SelectedEstablishment = null;
        }
        private async void ShowFiltersAsync()
        {
            await _navigationService.NavigateAsync($"NavigationPage/{nameof(FilterPage)}");
        }
        //private async void LoadAddressAsync()
        //{
        //    Response response = await _apiService.GetAllAddressesAsync<Address>("", " / api", "/Establishment", "bearer", "");
        //    var addresses = (List<Address>)response.Result;
        //    MyAddress = addresses[0];

        //}
        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            }
        }
    }
}
