using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

        }

    }
}
