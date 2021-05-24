using ClubersCustomerMobile.Prism.Models;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
        }
    }
}
