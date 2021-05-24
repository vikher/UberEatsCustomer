using Xamarin.Forms;


namespace ClubersCustomerMobile.Prism.Views
{
    public partial class CustomerTabbedPage : TabbedPage
    {
        public CustomerTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

        }
    }
}
