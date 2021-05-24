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
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class OnSiteProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private Establishment _establishment;
        private Order _shoppingCartOrder;
        private Product? _selectedProduct;
        private string _directionsIcon = IconFont.Directions;
        private string _qrIcon = IconFont.Qrcode;
        private string _shareIcon = IconFont.ShareVariant;
        private string _giftIcon = IconFont.Gift;
        private List<Product> _Products;
        private ObservableCollection<Grouping<string, Product>> _ProductsGrouped;
        private DelegateCommand<Product> _selectProductCommand;
        private DelegateCommand _opinionsCommand;
        private DelegateCommand _informationCommand;
        private DelegateCommand _photosCommand;
        private DelegateCommand _amenitiesCommand;
        private DelegateCommand _viewCartCommand;
        private DelegateCommand _shareCommand;
        private DelegateCommand _qrCodeCommand;
        public OnSiteProductsPageViewModel(INavigationService navigationService,
            IApiService apiService, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _dialogService = dialogService;

            Title = "Menú del restaurante en sitio";
            var EmptyTip = new Tip() { Amount = 0 };
            ShoppingCartOrder = new Order() { Tip = EmptyTip };

        }
        public DelegateCommand ViewCartCommand => _viewCartCommand ?? (_viewCartCommand = new DelegateCommand(ViewCartAsync));
        public DelegateCommand<Product> SelectedProductCommand => _selectProductCommand ?? (_selectProductCommand = new DelegateCommand<Product>(ProductDetails));
        public DelegateCommand OpinionsCommand => _opinionsCommand ?? (_opinionsCommand = new DelegateCommand(OpinionsAsync));
        public DelegateCommand InformationCommand => _informationCommand ?? (_informationCommand = new DelegateCommand(NavigateToInformationAsync));
        public DelegateCommand PhotosCommand => _photosCommand ?? (_photosCommand = new DelegateCommand(NavigateToPhotosAsync));
        public DelegateCommand AmenitiesCommand => _amenitiesCommand ?? (_amenitiesCommand = new DelegateCommand(NavigateToAmenitiesAsync));
        public DelegateCommand ShareCommand => _shareCommand ?? (_shareCommand = new DelegateCommand(ShareAsync));

        public DelegateCommand QrCodeCommand => _qrCodeCommand ?? (_qrCodeCommand = new DelegateCommand(QrCodeAsync));

        public string DirectionsIcon
        {
            get => _directionsIcon;
            set => SetProperty(ref _directionsIcon, value);
        }
        public string QrIcon
        {
            get => _qrIcon;
            set => SetProperty(ref _qrIcon, value);
        }
        public string ShareIcon
        {
            get => _shareIcon;
            set => SetProperty(ref _shareIcon, value);
        }
        public string GiftIcon
        {
            get => _giftIcon;
            set => SetProperty(ref _giftIcon, value);
        }
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

        public List<Product> Products
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
                await _dialogService.DisplayAlertAsync("Comanda", "Aún no tienes productos en tu orden", "Ok");
                return;
            }

            ShoppingCartOrder.Establishment = Establishment;
            NavigationParameters parameters = new NavigationParameters
            {
                { "order", ShoppingCartOrder }
            };
            await _navigationService.NavigateAsync(nameof(QrCodePage), parameters);

            //await _navigationService.NavigateAsync(nameof(PurchaseDetailPage), parameters);
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
                { "ProductItemViewModelId", SelectedProduct },
                { "establishment", Establishment }
            };
                await _navigationService.NavigateAsync(nameof(OnSiteProductDetailPage), parameters);
                SelectedProduct = null;
            }
            else
            {

                NavigationParameters parameters = new NavigationParameters
            {
                { "ProductItemViewModelId", SelectedProduct },
                { "order", ShoppingCartOrder }
            };
                await _navigationService.NavigateAsync(nameof(OnSiteProductDetailPage), parameters);
                SelectedProduct = null;
            }
            SelectedProduct = null;

        }

        private async void ShareAsync()
        {
            if (ShoppingCartOrder == null)
                return;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };


            await Share.RequestAsync(new ShareTextRequest
            {
                //Text = ReferralCode,
                Title = "Share Text"
            });



            await _navigationService.NavigateAsync(nameof(FinalizedPurchasePage), parameters);
        }
        private async void QrCodeAsync()
        {
            await _navigationService.NavigateAsync(nameof(QrCodePage));
        }
    }
}
