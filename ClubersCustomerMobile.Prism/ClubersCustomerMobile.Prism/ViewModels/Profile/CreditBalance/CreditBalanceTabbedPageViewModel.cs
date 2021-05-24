using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.ViewModels
{
    public class CreditBalanceTabbedPageViewModel : ViewModelBase
    {
        public CreditBalanceTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Acreditar Saldo";

        }
    }
}
