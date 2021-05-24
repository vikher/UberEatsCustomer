using ClubersCustomerMobile.Prism.Models;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class ShoppingCartPage : ContentPage
    {
        public ShoppingCartPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

        }
    }
}
