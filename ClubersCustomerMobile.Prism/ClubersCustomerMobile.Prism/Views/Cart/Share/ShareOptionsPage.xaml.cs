using ClubersCustomerMobile.Prism.Interfaces;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class ShareOptionsPage : ContentPage, IModalPage
    {
        public ShareOptionsPage()
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
