using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class OpinionsPage : ContentPage
    {
        public OpinionsPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            webView.Source = "https://www.google.com/search?hl=en-AU&gl=au&q=Pizza+Hut+Eastwood,+79+Balaclava+Road,+Eastwood&ludocid=11716357863625053629&#lrd=0x6b12a5d34eb22687:0xa298dd66aab9d5bd,1,,,";

        }
    }
}
