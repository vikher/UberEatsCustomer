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
    public class PinRecoveryPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IPageDialogService _dialogService;
        private string _lockIcon = IconFont.Lock;
        private DelegateCommand _pinConfigurationCommand;
        
        public PinRecoveryPageViewModel(INavigationService navigationService,
                                  IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            Title = "Recuperar PIN";
        }
        public DelegateCommand PinConfigurationCommand => _pinConfigurationCommand ?? (_pinConfigurationCommand = new DelegateCommand(PinConfigurationAsync));

        public string LockIcon
        {
            get => _lockIcon;
            set => SetProperty(ref _lockIcon, value);
        }
        private async void PinConfigurationAsync()
        {
            await _navigationService.NavigateAsync(nameof(PinConfigurationPage));
        }
    }
}
