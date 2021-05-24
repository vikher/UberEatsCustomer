using ClubersCustomerMobile.Prism.Models;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class OnSiteProductsPage : ContentPage
    {
        public OnSiteProductsPage()
        {

            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, ""); //Empty string as title

        }
    }
}
