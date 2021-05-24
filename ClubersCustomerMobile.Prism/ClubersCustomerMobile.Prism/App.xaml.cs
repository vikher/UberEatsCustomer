using Prism;
using Prism.Ioc;
using ClubersCustomerMobile.Prism.ViewModels;
using ClubersCustomerMobile.Prism.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClubersCustomerMobile.Prism.Services;
using ClubersCustomerMobile.Prism.Models;
using ClubersCustomerMobile.Prism.Views.CreditBalance;
using ClubersCustomerMobile.Prism.Views.Profile.AvailableBalance;
using ClubersCustomerMobile.Prism.Interfaces;
using ClubersCustomerMobile.Prism.Helpers;
using ClubersCustomerMobile.Prism.Views.Profile;
using Prism.Plugin.Popups;
using ClubersCustomerMobile.Prism.Views.Cart.Purchase;

namespace ClubersCustomerMobile.Prism
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Device.SetFlags(new[] {
                "Shapes_Experimental",
                "SwipeView_Experimental",
                "CarouselView_Experimental",
                "RadioButton_Experimental",
                "IndicatorView_Experimental",
                "AppTheme_Experimental",
                "Brush_Experimental",
                
            });

            if (Settings.IsRemembered && Settings.IsLogin)
            {
                await NavigationService.NavigateAsync("/NavigationPage/CustomerTabbedPage?selectedTab=HomePage");
            }
            else
            {
                await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IGeolocatorService, GeolocatorService>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ExplorePage, ExplorePageViewModel>();
            containerRegistry.RegisterForNavigation<CustomerTabbedPage, CustomerTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<OrdersPage, OrdersPageViewModel>();
            containerRegistry.RegisterForNavigation<CartPage, CartPageViewModel>();
            containerRegistry.RegisterForNavigation<CustomerAddressPage, CustomerAddressPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductsPage, ProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductDetailSectionPage, ProductDetailSectionPageViewModel>();
            containerRegistry.RegisterForNavigation<CreditBalancePage, CreditBalancePageViewModel>();
            containerRegistry.RegisterForNavigation<AvailableBalancePage, AvailableBalancePageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<TermsAndConditionsPage, TermsAndConditionsPageViewModel>();
            containerRegistry.RegisterForNavigation<SavedAddressesPage, SavedAddressesPageViewModel>();
            containerRegistry.RegisterForNavigation<AddNewAddressPage, AddNewAddressPageViewModel>();
            containerRegistry.RegisterForNavigation<AddNewPaymentPage, AddNewPaymentPageViewModel>();
            containerRegistry.RegisterForNavigation<SavedPaymentMethodsPage, SavedPaymentMethodsPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationsPage, NotificationsPageViewModel>();
            containerRegistry.RegisterForNavigation<AccountHistoryTabbedPage, AccountHistoryTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<OrdersPage, OrdersPageViewModel>();
            containerRegistry.RegisterForNavigation<MovementsPage, MovementsPageViewModel>();
            containerRegistry.RegisterForNavigation<RefundsPage, RefundsPageViewModel>();
            containerRegistry.RegisterForNavigation<OrderDetailPage, OrderDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ReferPage, ReferPageViewModel>();
            containerRegistry.RegisterForNavigation<HelpPage, HelpPageViewModel>();
            containerRegistry.RegisterForNavigation<VerifyPhonePage, VerifyPhonePageViewModel>();
            containerRegistry.RegisterForNavigation<MembershipPage, MembershipPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<UpdatePasswordPage, UpdatePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<OpinionsPage, OpinionsPageViewModel>();
            containerRegistry.RegisterForNavigation<CartPage, CartPageViewModel>();
            containerRegistry.RegisterForNavigation<AmenitiesPage, AmenitiesPageViewModel>();
            containerRegistry.RegisterForNavigation<EstablishmentInfoPage, EstablishmentInfoPageViewModel>();
            containerRegistry.RegisterForNavigation<PhotosPage, PhotosPageViewModel>();
            containerRegistry.RegisterForNavigation<FreeTrialPage, FreeTrialPageViewModel>();
            containerRegistry.RegisterForNavigation<SharePage, SharePageViewModel>();
            containerRegistry.RegisterForNavigation<ShoppingCartPage, ShoppingCartPageViewModel>();
            containerRegistry.RegisterForNavigation<PinPage, PinPageViewModel>();
            containerRegistry.RegisterForNavigation<PurchaseDetailPage, PurchaseDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<FilterPage, FilterPageViewModel>();
            containerRegistry.RegisterForNavigation<UseBalancePage, UseBalancePageViewModel>();
            containerRegistry.RegisterForNavigation<UseBalancePage, UseBalancePageViewModel>();
            containerRegistry.RegisterForNavigation<SuccessOrderPage, SuccessOrderPageViewModel>();
            containerRegistry.RegisterForNavigation<CustomTipPage, CustomTipPageViewModel>();
            containerRegistry.RegisterForNavigation<DeliveryTrackingPage, DeliveryTrackingPageViewModel>();
            containerRegistry.RegisterForNavigation<DeliveryTrackingDetailPage, DeliveryTrackingDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<OrderAtHomePage, OrderAtHomePageViewModel>();
            containerRegistry.RegisterForNavigation<PurchaseThankingMessagePage, PurchaseThankingMessagePageViewModel>();
            containerRegistry.RegisterForNavigation<EvaluateDeliveryPage, EvaluateDeliveryPageViewModel>();
            containerRegistry.RegisterForNavigation<EvaluateRestaurantPage, EvaluateRestaurantPageViewModel>();
            containerRegistry.RegisterForNavigation<ShareOptionsPage, ShareOptionsPageViewModel>();
            containerRegistry.RegisterForNavigation<ShareWithPhotoPage, ShareWithPhotoPageViewModel>();
            containerRegistry.RegisterForNavigation<FinalizedPurchasePage, FinalizedPurchasePageViewModel>();
            containerRegistry.RegisterForNavigation<SurveyPage, SurveyPageViewModel>();
            containerRegistry.RegisterForNavigation<AnswersPage, AnswersPageViewModel>();
            containerRegistry.RegisterForNavigation<CreditBalanceTabbedPage, CreditBalanceTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<WithoutEvaluatingNorSharingPage, WithoutEvaluatingNorSharingPageViewModel>();
            containerRegistry.RegisterForNavigation<WithoutSharingPage, WithoutSharingPageViewModel>();
            containerRegistry.RegisterForNavigation<PinRecoveryPage, PinRecoveryPageViewModel>();
            containerRegistry.RegisterForNavigation<PinConfigurationPage, PinConfigurationPageViewModel>();
            containerRegistry.RegisterForNavigation<CompleteDataPage, CompleteDataPageViewModel>();
            containerRegistry.RegisterForNavigation<CommandPage, CommandPageViewModel>();
            containerRegistry.RegisterForNavigation<CommandWithWaiterPage, CommandWithWaiterPageViewModel>();
            containerRegistry.RegisterForNavigation<WaiterTipPage, WaiterTipPageViewModel>();
            containerRegistry.RegisterForNavigation<OnSitePaymentConfirmationPage, OnSitePaymentConfirmationPageViewModel>();
            containerRegistry.RegisterForNavigation<OnSitePinPage, OnSitePinPageViewModel>();
            containerRegistry.RegisterForNavigation<OnSiteProductDetailPage, OnSiteProductDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<OnSiteProductsPage, OnSiteProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<PaymentMethodsPage, PaymentMethodsPageViewModel>();
            containerRegistry.RegisterForNavigation<QrCodePage, QrCodePageViewModel>();
            containerRegistry.RegisterForNavigation<PaymentPage, PaymentPageViewModel>();
            containerRegistry.RegisterForNavigation<MessagesPage, MessagesPageViewModel>();
        }
    }
}
