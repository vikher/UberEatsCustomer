using System.Linq;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Renders
{
    public class CustomCard : Frame
    {
        public CustomCard()
        {
            HasShadow = !(Device.RuntimePlatform == Device.iOS);
            CornerRadius = 20;
            BackgroundColor = (Color)App.Current.Resources.FirstOrDefault(x => x.Key.Equals("accent")).Value;
        }
    }
}
