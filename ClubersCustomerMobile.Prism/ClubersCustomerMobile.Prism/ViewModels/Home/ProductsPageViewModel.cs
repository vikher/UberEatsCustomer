using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Newtonsoft.Json;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{

    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private Establishment _establishment;
        private Order _shoppingCartOrder;
        private Product? _selectedProduct;
        private ObservableCollection<Product> _Products;
        private ObservableCollection<Grouping<string, Product>> _ProductsGrouped;
        private DelegateCommand<Product> _selectProductCommand;
        private DelegateCommand _opinionsCommand;
        private DelegateCommand _informationCommand;
        private DelegateCommand _photosCommand;
        private DelegateCommand _amenitiesCommand;
        private DelegateCommand _viewCartCommand;

        public ProductsPageViewModel(INavigationService navigationService,
            IApiService apiService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;

            Title = "Menú del restaurante";
            var EmptyTip = new Tip() { Amount = 0 };
            ShoppingCartOrder = new Order() { Tip = EmptyTip };
        }
        public DelegateCommand ViewCartCommand => _viewCartCommand ?? (_viewCartCommand = new DelegateCommand(ViewCartAsync));
        public DelegateCommand<Product> SelectedProductCommand => _selectProductCommand ?? (_selectProductCommand = new DelegateCommand<Product>(ProductDetails));
        public DelegateCommand OpinionsCommand => _opinionsCommand ?? (_opinionsCommand = new DelegateCommand(OpinionsAsync));
        public DelegateCommand InformationCommand => _informationCommand ?? (_informationCommand = new DelegateCommand(NavigateToInformationAsync));
        public DelegateCommand PhotosCommand => _photosCommand ?? (_photosCommand = new DelegateCommand(NavigateToPhotosAsync));
        public DelegateCommand AmenitiesCommand => _amenitiesCommand ?? (_amenitiesCommand = new DelegateCommand(NavigateToAmenitiesAsync));
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }
        public ObservableCollection<Grouping<string, Product>> ProductsGrouped
        {
            get => _ProductsGrouped;
            set => SetProperty(ref _ProductsGrouped, value);
        }

        //[AutoInitialize("Establishment", true)]
        public Establishment Establishment
        {
            get => _establishment;
            set => SetProperty(ref _establishment, value);
        }

        public ObservableCollection<Product> Products
        {
            get => _Products;
            set => SetProperty(ref _Products, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("Establishment"))
            {
                Establishment = parameters.GetValue<Establishment>("Establishment");
                LoadProductsAsync();
            }
            if (parameters.ContainsKey("order"))
            {
                SelectedProduct = null;
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                LoadProductsAsync();
            }
        }
        private async void ViewCartAsync()
        {
            if (ShoppingCartOrder.Products == null)
            {
                await _dialogService.DisplayAlertAsync("Carrito de Compras", "Aún no tienes productos en tu cesta", "Ok");
                return;
            }

            ShoppingCartOrder.Establishment = Establishment;
            Settings.Products = JsonConvert.SerializeObject(ShoppingCartOrder.Products);

            NavigationParameters parameters = new NavigationParameters
            {
                { "order", ShoppingCartOrder }
            };

            await _navigationService.NavigateAsync(nameof(PurchaseDetailPage), parameters);
        }
        private async void OpinionsAsync()
        {
            await _navigationService.NavigateAsync(nameof(OpinionsPage));
        }
        private async void NavigateToAmenitiesAsync()
        {
            if (Establishment == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "Establishment", Establishment }
            };

            await _navigationService.NavigateAsync(nameof(AmenitiesPage), parameters);
        }

        private async void NavigateToPhotosAsync()
        {
            if (Establishment == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "Establishment", Establishment }
            };

            await _navigationService.NavigateAsync(nameof(PhotosPage), parameters);
        }

        private async void NavigateToInformationAsync()
        {
            if (Establishment == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "Info", Establishment }
            };

            await _navigationService.NavigateAsync(nameof(EstablishmentInfoPage), parameters);
        }

        private async void LoadProductsAsync()
        {
            IsRunning = true;
            if (!_apiService.CheckConnection())
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, Constants.ConnectionError, Constants.AcceptMessage);
                return;
            }

            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            ListResponse<Product> response = await _apiService.GetListAsync<Product>(Constants.urlBase, Constants.servicePrefix, Constants.GetAllProductsByEstablishmentAsyncController, Constants.tokenType, token.Result.token, Establishment.Id);
            if (response.ResultCode != ResultCode.Success)
            {
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(Constants.ErrorMessage, response.ResultMessages.FirstOrDefault(), Constants.AcceptMessage);
                return;
            }

            var products = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(products);

            var sorted = from Product in products
                         orderby Product.Name
                         group Product by Product.Sections[0].Name into ProductGroup
                         select new Grouping<string, Product>(ProductGroup.Key, ProductGroup);

            //create a new collection of groups
            ProductsGrouped = new ObservableCollection<Grouping<string, Product>>(sorted);
            IsRunning = false;

        }
        private async void ProductDetails(Product obj)
        {
            if (SelectedProduct == null)
                return;

            if (ShoppingCartOrder == null)
            {
                NavigationParameters parameters = new NavigationParameters
            {
                { "ProductItemViewModelId", SelectedProduct }
            };
                await _navigationService.NavigateAsync(nameof(ProductDetailSectionPage), parameters);
                SelectedProduct = null;
            }
            else
            {

                NavigationParameters parameters = new NavigationParameters
            {
                { "ProductItemViewModelId", SelectedProduct },
                { "order", ShoppingCartOrder }
            };
                await _navigationService.NavigateAsync(nameof(ProductDetailSectionPage), parameters);
                SelectedProduct = null;
            }
            SelectedProduct = null;

        }
    }
}
