using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class ShareWithPhotoPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ImageSource _imageSource;
        private MediaFile _file;
        private Order _shoppingCartOrder;
        private DelegateCommand _changeImageCommand;
        private DelegateCommand _finalizedPurchaseCommand;
        private DelegateCommand _shareCommand;


        public ShareWithPhotoPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Comparte tu experiencia";
        }
        public DelegateCommand FinalizedPurchaseCommand => _finalizedPurchaseCommand ?? (_finalizedPurchaseCommand = new DelegateCommand(FinalizedPurchaseAsync));
        public DelegateCommand ShareCommand => _shareCommand ?? (_shareCommand = new DelegateCommand(ShareAsync));
        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(ChangeImageAsync));
        public Order ShoppingCartOrder
        {
            get => _shoppingCartOrder;
            set => SetProperty(ref _shoppingCartOrder, value);
        }
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }
        private async void FinalizedPurchaseAsync()
        {
            if (ShoppingCartOrder == null)
                return;

            NavigationParameters parameters = new NavigationParameters
                    {
                        { "order", ShoppingCartOrder }
                    };

            await _navigationService.NavigateAsync(nameof(FinalizedPurchasePage), parameters);
        }
        private async void ChangeImageAsync()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Where do you want to get the picture from?",
                "Cancel",
                null,
                "From Gallery",
                "From Camera");

            if (source == "Cancel")
            {
                _file = null;
                return;
            }

            if (source == "From Camera")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("order"))
            {
                ShoppingCartOrder = parameters.GetValue<Order>("order");

            }

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
    }
}
