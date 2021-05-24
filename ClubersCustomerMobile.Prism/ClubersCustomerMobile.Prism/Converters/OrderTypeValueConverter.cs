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
    public class OrderTypeValueConverter : IValueConverter
    {
        public string Domicilio = "https://clubersblob.blob.core.windows.net/icons/llevar.png";
        public string EnSitio = "https://clubersblob.blob.core.windows.net/icons/ensitio.png";
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            if (value.ToString() == "Pedido a domicilio")
            {
                return Domicilio;
            }
            if (value.ToString() == "Consumo en sitio")
            {
                return EnSitio;
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
