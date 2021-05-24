using Naxam.Controls.Forms;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class AccountHistoryTabbedPage : TopTabbedPage
    {
        public AccountHistoryTabbedPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

        }
    }
}
