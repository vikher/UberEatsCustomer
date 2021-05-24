using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CustomTipPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private string _customTip;
        private DelegateCommand<string> _numberCommand;
        private DelegateCommand _eraseNumberCommand;
        private DelegateCommand _navigateToPurchaseDetailPageCommand;
        private DelegateCommand _navigateToWaiterTipPageCommand;

        public CustomTipPageViewModel(INavigationService navigationService,
                                  IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Title = "CustomTipPage";
        }

        public DelegateCommand<string> NumberCommand => _numberCommand ?? (_numberCommand = new DelegateCommand<string>(EnterNumber));
        public DelegateCommand EraseNumberCommand => _eraseNumberCommand ?? (_eraseNumberCommand = new DelegateCommand(EraseNumber));
        public DelegateCommand NavigateToPurchaseDetailPageCommand => _navigateToPurchaseDetailPageCommand ?? (_navigateToPurchaseDetailPageCommand = new DelegateCommand(NavigateToPurchaseDetailPageAsync));
        public DelegateCommand NavigateToWaiterTipPageCommand => _navigateToWaiterTipPageCommand ?? (_navigateToWaiterTipPageCommand = new DelegateCommand(NavigateToWaiterTipPageAsync));
        public string CustomTip
        {
            get => _customTip;
            set => SetProperty(ref _customTip, value);
        }
        private void EraseNumber()
        {
            CustomTip = CustomTip.Remove(CustomTip.Length - 1, 1);
        }

        private async void NavigateToPurchaseDetailPageAsync()
        {
            if (CustomTip == null)
                return;
            
            NavigationParameters parameters = new NavigationParameters
            {
                { "customTip", CustomTip }
            };

            await _navigationService.GoBackAsync(parameters);
        }
       
        private async void NavigateToWaiterTipPageAsync()
        {
            if (CustomTip == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "customTip", CustomTip }
            };

            await _navigationService.GoBackAsync(parameters);
        }

        private async void EnterNumber(string key)
        {
            // Add the key to the input string until we have the max length of 6.
            if (CustomTip == null || CustomTip.Length < 6)
                CustomTip += key;

            // If there's a pin and it's 6 in length we try a login.
            if (CustomTip != null && CustomTip.Length == 6)
            {
                CustomTip = string.Empty;
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}
