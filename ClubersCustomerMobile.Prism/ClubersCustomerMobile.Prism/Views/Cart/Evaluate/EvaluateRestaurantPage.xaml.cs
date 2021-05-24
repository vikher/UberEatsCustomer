using ClubersCustomerMobile.Prism.Interfaces;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class EvaluateRestaurantPage : ContentPage, IModalPage
    {
        public EvaluateRestaurantPage()
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
