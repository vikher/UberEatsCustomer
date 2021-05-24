﻿using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism.Views
{
    public partial class AddNewPaymentPage : ContentPage
    {
        public Color PlaceholderValueColor { get { return (Color)App.Current.Resources.FirstOrDefault(x => x.Key.Equals("placeholder_value")).Value; } }

        public Color OnAccentColor { get { return (Color)App.Current.Resources.FirstOrDefault(x => x.Key.Equals("icons")).Value; } }

        public uint FadeCreditCardLogo { get { return 1000; } }

        public AddNewPaymentPageViewModel Vm { get { return (AddNewPaymentPageViewModel)BindingContext; } }

        public AddNewPaymentPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                await Task.Delay(200);
                entryNumber.Focus();
            });
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e?.NewTextValue))
            {
                lbNumberValue.Text = "XXXX XXXX XXXX XXXX";
                lbNumberValue.TextColor = PlaceholderValueColor;

                //imgLogoCard.Opacity = 1;
                imgLogoCard.FadeTo(0, FadeCreditCardLogo);
            }

            else
            {
                lbNumberValue.Text = e.NewTextValue;
                lbNumberValue.TextColor = OnAccentColor;

                if (lbNumberValue.Text.Length.Equals(1) && string.IsNullOrEmpty(e?.OldTextValue))
                {
                    var cardType = e.NewTextValue.GetCardType();

                    if (cardType != CreditCardHelper.CardType.Undefined)
                    {
                        Vm.LogoCreditCard.Source = cardType.GetLogoUrl();
                    }
                }
            }
        }

        private void Entry_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e?.NewTextValue))
            {
                lbNameValue.Text = "NOMBRE APELLIDOS";
                lbNameValue.TextColor = PlaceholderValueColor;
            }

            else
            {
                lbNameValue.Text = e.NewTextValue.ToUpper();
                lbNameValue.TextColor = OnAccentColor;
            }
        }

        private void Entry_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e?.NewTextValue))
            {
                lbValidValue.Text = "MM/YY";
                lbValidValue.TextColor = PlaceholderValueColor;
            }

            else
            {
                lbValidValue.Text = e.NewTextValue;
                lbValidValue.TextColor = OnAccentColor;
            }
        }

        private void Entry_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e?.NewTextValue))
                lbCvvValue.Text = "CVV";

            else
                lbCvvValue.Text = e.NewTextValue;
        }

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Image.Source)))
            {
                var img = (Image)sender;
                img.Opacity = 0;
                img.FadeTo(1, FadeCreditCardLogo);
            }
        }

        private void StackLayout_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(StackLayout.IsVisible)) && ((StackLayout)sender).IsVisible)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(200);
                    entryNumber.Focus();
                });

            }
        }

        private void StackLayout_PropertyChanged_1(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(StackLayout.IsVisible)) && ((StackLayout)sender).IsVisible)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(200);
                    entryName.Focus();
                });

            }
        }

        private void StackLayout_PropertyChanged_2(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(StackLayout.IsVisible)) && ((StackLayout)sender).IsVisible)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(200);
                    entryValid.Focus();
                });

            }
        }

        private void StackLayout_PropertyChanged_3(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(StackLayout.IsVisible)) && ((StackLayout)sender).IsVisible)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(200);
                    entryCvv.Focus();
                });
            }
        }
    }
}
