using ClubersCustomerMobile.Prism.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class HelpPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private List<Problem> _problems;
        private bool _isEnabled;
        private Problem _selectedProblem;
        private DelegateCommand _whatsappCommand;
        private DelegateCommand _selectedProblemCommand;

        public HelpPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Ayuda";
            Problems = new List<Problem>()
                {
                    new Problem(){Description = "Tengo un problema con un pedido"},
                    new Problem(){Description = "Tengo un problema con un pago"},
                    new Problem(){Description = "Tengo un problema con un Saldo"},
                };
        }
        public DelegateCommand WhatsappCommand => _whatsappCommand ?? (_whatsappCommand = new DelegateCommand(WhatsappAsync));
        public DelegateCommand SelectedProblemCommand => _selectedProblemCommand ?? (_selectedProblemCommand = new DelegateCommand(SelectedProblemAsync));

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        public Problem SelectedProblem
        {
            get => _selectedProblem;
            set => SetProperty(ref _selectedProblem, value);
        }
        public List<Problem> Problems
        {
            get => _problems;
            set => SetProperty(ref _problems, value);
        }

        private async void WhatsappAsync()
        {
            if (SelectedProblem.Description == "Tengo un problema con un pedido")
            {
                await Launcher.TryOpenAsync(Constants.wa1);
            }
            else if (SelectedProblem.Description == "Tengo un problema con un pago")
            {
                await Launcher.TryOpenAsync(Constants.wa2);
            }
            else if (SelectedProblem.Description == "Tengo un problema con un Saldo")
            {
                await Launcher.TryOpenAsync(Constants.wa3);
            }
            else
            {
                return;
            }
        }

        private void SelectedProblemAsync()
        {
            IsEnabled = true;
        }

    }
}
