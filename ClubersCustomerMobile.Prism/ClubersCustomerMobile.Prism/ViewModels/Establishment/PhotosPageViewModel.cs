using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Views;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class PhotosPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private Establishment _establishment;
        private List<Photo> _photoss;

        public PhotosPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Fotos";
        }

        public Establishment Establishment
        {
            get => _establishment;
            set => SetProperty(ref _establishment, value);
        }

        public List<Photo> Photos
        {
            get => _photoss;
            set => SetProperty(ref _photoss, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("Establishment"))
            {
                Establishment = parameters.GetValue<Establishment>("Establishment");
                Photos = Establishment.Photos;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add("Establishment", Establishment);
        }
    }
}

