using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ClubersCustomerMobile.Prism
{
    public static class Constants
    {
        public static string HostName { get; set; } = "https://chatserverclubers.azurewebsites.net";
        public static string MessageName { get; set; } = "newMessage";
        public static string Username
        {
            get
            {
                return $"{Device.RuntimePlatform} User";
            }
        }

        public const string ErrorMessage = "Error en el app";
        public const string AcceptMessage = "Aceptar";
        public const string ConnectionError = "Error de Conexion a Internet";
        public const string RegisterMessge = "Usuario registrado";
        public const string DocumentError = "";
        public const string FirstNameError = "";
        public const string LastNameError = "";
        public const string AddressError = "";
        public const string EmailError = "";
        public const string PhoneError = "";
        public const string NIPError = "";
        public const string SecretAnswerError = "";
        public const string PasswordError = "";
        public const string PasswordConfirmError1 = "";
        public const string PasswordConfirmError2 = "";
        public const string FinishOrderMessage = "Orden Enviada";

        

        public const string GoogleMapsApiKey = "";
        public const string urlBase = "http://clubers.qagperti.tk/";
        public const string servicePrefix = "api";

        //Controllers
        public const string GetUserByEmailAsyncController = "/Account/GetUserByEmailAsync";
        public const string GetAllCategoriesAsyncController = "/Categories/GetAllCategoriesAppAsync/{\"lat\":19.433205712481872,\"lng\":-99.12158984351414}";
        public const string GetAllEstablishmentsAsyncController = "/Establishment/GetAllEstablishmentsAppAsync/{\"lat\":19.433205712481872,\"lng\":-99.12158984351414}";
        public const string GetAllProductsByEstablishmentAsyncController = "/Product/GetAllProductsByEstablishmentIdAppAsync";
        public const string GetTipsByEstablishmentIdAsyncController = "/Establishment/GetTipsByEstablishmentIdAppAsync";
        public const string GetSecretQuestionsAsyncController = "/Customer/GetSecretQuestions";
        public const string GetAllOrdersByCustomerIdAsyncController = "/Order/GetAllOrdersByCustomerIdAppAsync";
        public const string GetRefundsByCustomerIdAsyncController = "/Customer/GetRefundsByCustomerIdAppAsync";
        public const string GetMembershipByCustomerIdController = "/Membership/GetMembershipByCustomerIdAppAsync";
        public const string GetEstablishmentStaffByOrderIdController = "/EstablishmentStaff/GetEstablishmentStaffByOrderIdAppAsync";
        public const string GetDeliveryMenByOrderIdController = "/HumanResources/GetDeliveryMenByOrderIdAppAsync";
        public const string PostOrderController = "/Order/NewCommandAsync";
        public const string GetAllAddressesByUserIdController = "/User/GetAllAddressesByUserIdAppAsync";

        

    public const string controller = "";
        public const string tokenType = "bearer";
        public const string accessToken = "";
        //Whatsapp
        public const string wa1 = "https://wa.me/+5215586765674?text=Tengo%20un%20problema%20con%20un%20pedido";
        public const string wa2 = "https://wa.me/+5215586765674?text=Tengo%20un%20problema%20con%20un%20pago";
        public const string wa3 = "https://wa.me/+5215586765674?text=Tengo%20un%20problema%20con%20un%20Saldo";

    }
}
