using ClubersCustomerMobile.Prism.Interfaces;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class EvaluateDeliveryPage : ContentPage, IModalPage
    {
        public EvaluateDeliveryPage()
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
