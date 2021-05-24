using ClubersCustomerMobile.Prism.Models;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class ExplorePage : ContentPage
    {
        public ExplorePage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

        }
        private void OnClic(object sender, System.EventArgs e)
        {
            domicilio.BackgroundColor = Color.Black;
            domicilio.TextColor = Color.White;
            domicilio.ImageSource = "https://clubersblob.blob.core.windows.net/icons/DomicilioHomeNegativo.png";
            sitio.BackgroundColor = Color.White;
            sitio.TextColor = (Color)Application.Current.Resources["primary_text"];
            sitio.ImageSource = "https://clubersblob.blob.core.windows.net/icons/SitioHome.png";
        }

        private void OnClik(object sender, System.EventArgs e)
        {
            domicilio.BackgroundColor = Color.White;
            domicilio.TextColor = (Color)Application.Current.Resources["primary_text"];
            domicilio.ImageSource = "https://clubersblob.blob.core.windows.net/icons/DomicilioHome.png";
            sitio.BackgroundColor = Color.Black;
            sitio.TextColor = Color.White;
            sitio.ImageSource = "https://clubersblob.blob.core.windows.net/icons/SitioHomeNegativo.png";
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //domicilio.BackgroundColor = (Color)Application.Current.Resources["cluberspink"];
            /*domicilio.BackgroundColor = Color.White;
            domicilio.TextColor = Color.Black;
            domicilio.ImageSource = "https://clubersblob.blob.core.windows.net/icons/DomicilioHome.png";
            sitio.BackgroundColor = Color.Black;
            sitio.TextColor = Color.White;
            sitio.ImageSource = "https://clubersblob.blob.core.windows.net/icons/SitioHomeNegativo.png";*/
        }

    }
}
