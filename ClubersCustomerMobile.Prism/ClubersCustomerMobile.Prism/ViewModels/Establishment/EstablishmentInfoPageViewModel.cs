using ClubersCustomerMobile.Prism.Models;
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
    public class EstablishmentInfoPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private Establishment _establishment;

        private string _addressIcon = IconFont.MapMarker;
        private string _businessHoursIcon = IconFont.Hours24;
        private string _phoneIcon = IconFont.Cellphone;
        private string _websiteIcon = IconFont.Web;
        private string _emailIcon = IconFont.EmailOutline;
        public EstablishmentInfoPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Informacion";
        }

        public string AddressIcon
        {
            get => _addressIcon;
            set => SetProperty(ref _addressIcon, value);
        }
        public string BusinessHoursIcon
        {
            get => _businessHoursIcon;
            set => SetProperty(ref _businessHoursIcon, value);
        }
        public string PhoneIcon
        {
            get => _phoneIcon;
            set => SetProperty(ref _phoneIcon, value);
        }
        public string WebsiteIcon
        {
            get => _websiteIcon;
            set => SetProperty(ref _websiteIcon, value);
        }
        public string EmailIcon
        {
            get => _emailIcon;
            set => SetProperty(ref _emailIcon, value);
        }

        public Establishment Establishment
        {
            get => _establishment;
            set => SetProperty(ref _establishment, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("Info"))
            {
                Establishment = parameters.GetValue<Establishment>("Info");
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add("Establishment", Establishment);
        }
    }
}