using ClubersCustomerMobile.Prism.Interfaces;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class CommandWithWaiterPage : ContentPage, IModalPage
    {
        public CommandWithWaiterPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
        }

        public void Dismiss()
        {
            Navigation.PopModalAsync();
        }
    }
}
