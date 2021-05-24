using ClubersCustomerMobile.Prism.Services;
using Prism.Commands;
using Prism.Navigation;
using System;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class FilterPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private int _condition;
        private DelegateCommand<string> _conditionCommand;
        private DelegateCommand _applyFilterCommand;

        public FilterPageViewModel(INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Filtros";
        }

        public DelegateCommand<string> ConditionCommand => _conditionCommand ?? (_conditionCommand = new DelegateCommand<string>(PassFilterParamAsync));
        public DelegateCommand ApplyFilterCommand => _applyFilterCommand ?? (_applyFilterCommand = new DelegateCommand(NavigateToExplorePageAsync));
        public int Condition
        {
            get => _condition;
            set => SetProperty(ref _condition, value);
        }
        private void PassFilterParamAsync(string obj)
        {
            string caseSwitch = obj;

            switch (caseSwitch)
            {
                case "1":
                    //id 1,Reembolso de bienvenida
                    Condition = 1;
                    break;
                case "2":
                    //id 2 Reembolso recurrente en sitio
                    Condition = 2;
                    break;
                case "3":
                    //id 3 Menor tiempo de entrega
                    Condition = 3;
                    break;
                case "4":
                    //id 4 Menor costo a domicilio
                    Condition = 4;
                    break;
                case "5":
                    //id 5 Mejor calificacion
                    Condition = 5;
                    break;
                case "6":
                    //id 6 Restaurantes cercanos
                    Condition = 6;
                    break;
                case "7":
                    //id 7 Restaurantes con servicio en sitio
                    Condition = 7;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        private async void NavigateToExplorePageAsync()
        {
            if (Condition == null)
                return;

            NavigationParameters parameters = new NavigationParameters
            {
                { "condition", Condition }
            };
            await NavigationService.NavigateAsync("/NavigationPage/CustomerTabbedPage?selectedTab=HomePage", parameters);
            //await _navigationService.GoBackAsync(parameters);
        }

    }
}
