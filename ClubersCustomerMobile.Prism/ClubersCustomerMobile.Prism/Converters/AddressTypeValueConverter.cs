using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.ViewModels;
using System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClubersCustomerMobile.Prism.Converters
{
    public class AddressTypeValueConverter : IValueConverter
    {
        public string Briefcase = IconFont.Briefcase;
        public string Home = IconFont.Home;
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (value.ToString() == "Trabajo")
            {
                return Briefcase;
            }
            if (value.ToString() == "Casa")
            {
                return Home;
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
