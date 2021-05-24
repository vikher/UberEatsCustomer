using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClubersCustomerMobile.Prism.Interfaces;
using ClubersCustomerMobile.Prism.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;


[assembly: ExportRenderer(typeof(Page), typeof(CustomPageRenderer))]

namespace ClubersCustomerMobile.Prism.iOS
{
    public class CustomPageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (Element is IModalPage modalPage)
            {
                NavigationController.TopViewController.NavigationItem.LeftBarButtonItem =
                    new UIBarButtonItem(title: "Cancel",
                        style: UIBarButtonItemStyle.Plain,
                        handler: (sender, args) => { modalPage.Dismiss(); });
            }
        }
    }
}