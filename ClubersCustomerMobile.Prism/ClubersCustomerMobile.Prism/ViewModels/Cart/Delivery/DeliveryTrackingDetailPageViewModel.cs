using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class DeliveryTrackingDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private Order _shoppingCartOrder;
        private Delivery _delivery;
        private DeliveryMen _deliveryMen;

        public DeliveryTrackingDetailPageViewModel(INavigationService navigationService,
                                  IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Detalle del progreso de compra";
        }
        public DeliveryMen DeliveryMen
        {
            get => _deliveryMen;
            set => SetProperty(ref _deliveryMen, value);
        }
        public Delivery Delivery
        {
            get => _delivery;
            set => SetProperty(ref _delivery, value);
        }
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");

            }
        }

    }
}
