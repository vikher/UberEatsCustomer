using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class FinalizedPurchasePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _imageUrl;
        private Order _shoppingCartOrder;
        private Refund _refund;
        private DelegateCommand _homeCommand;
        public FinalizedPurchasePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Compra finalizada";
            ImageUrl = "https://firebasestorage.googleapis.com/v0/b/clubers-278716.appspot.com/o/checked.png?alt=media&token=4cfbd219-287e-4ba4-b1fd-bd7c45ffc948";
        }
        public DelegateCommand HomeCommand => _homeCommand ?? (_homeCommand = new DelegateCommand(HomeAsync));

        public Refund Refund
        {
            get => _refund;
            set => SetProperty(ref _refund, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        //public static INavigation Navigation { get; set; }
        private async void HomeAsync()
        {
            await NavigationService.NavigateAsync(nameof(CustomerTabbedPage));
            //Navigation.NavigationStack.ToList().Clear();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");
                Refund = ShoppingCartOrder.Refund;
            }
        }
    }
}
