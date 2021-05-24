using ClubersCustomerMobile.Prism.Models;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class FreeTrialPage : ContentPage
    {
        public FreeTrialPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

        }
        private void Code_Focused(object sender, FocusEventArgs e)
        {
            boxCode.BorderColor = Color.DeepPink;
        }

        private void boxCode_Unfocused(object sender, FocusEventArgs e)
        {
            boxCode.BorderColor = Color.Gray;

        }
    }
}
