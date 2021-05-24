using ClubersCustomerMobile.Prism.Interfaces;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class FilterPage : ContentPage, IModalPage
    {
        public FilterPage()
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
