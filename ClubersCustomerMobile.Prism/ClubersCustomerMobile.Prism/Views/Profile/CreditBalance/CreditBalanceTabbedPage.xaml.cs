using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class CreditBalanceTabbedPage : TabbedPage
    {
        public CreditBalanceTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            UnselectedTabColor = Color.Gray;
            SelectedTabColor = Color.Black;
            BarBackgroundColor = Color.White;
        }
    }
}
