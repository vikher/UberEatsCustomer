using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Services;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class AmenitiesPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        private readonly IApiService _apiService;
        private Establishment _establishment;
        private List<Amenity> _amenities;
        private ObservableCollection<Grouping<string, Amenity>> _amenitiesGrouped;


        public AmenitiesPageViewModel(INavigationService navigationService,
                                         IPageDialogService dialogService,
                                         IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _apiService = apiService;
            Title = "Amenidades";
        }
        public ObservableCollection<Grouping<string, Amenity>> AmenitiesGrouped
        {
            get => _amenitiesGrouped;
            set => SetProperty(ref _amenitiesGrouped, value);
        }
        public Establishment Establishment
        {
            get => _establishment;
            set => SetProperty(ref _establishment, value);
        }

        public List<Amenity> Amenities
        {
            get => _amenities;
            set => SetProperty(ref _amenities, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("Establishment"))
            {
                Establishment = parameters.GetValue<Establishment>("Establishment");
                Amenities = Establishment.Amenities;
            }
            var sorted = from Ameni in Amenities
                         orderby Ameni.Name
                         group Ameni by Ameni.AmenityType into AmeniGroup
                         select new Grouping<string, Amenity>(AmeniGroup.Key, AmeniGroup);

            //create a new collection of groups
            AmenitiesGrouped = new ObservableCollection<Grouping<string, Amenity>>(sorted);
        }

        public  override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add( "Establishment", Establishment );
        }
    }
}

