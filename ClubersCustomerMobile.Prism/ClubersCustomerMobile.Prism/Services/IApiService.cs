using ClubersCustomerMobile.Prism.Models;
using System.Threading.Tasks;

namespace ClubersCustomerMobile.Prism.Services
{
    public interface IApiService
    {
        bool CheckConnection();
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
        //Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest request);
        Task<Response> GetUserByEmailAsync(string urlBase, string servicePrefix, string controller, string email, string tokenType, string accessToken);
        Task<RecoverPasswordResponse> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);
        Task<Response> GetUserById(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);
        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);
        Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model , string tokenType, string accessToken);
        Task<Response1<Membership>> GetMembershipAsync(string urlBase, string servicePrefix, string controller, string tokenType,string accessToken, int id);
        Task<Response1<DeliveryMen>> GetDeliveryManAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id);
        Task<Response1<Waiter>> GetEstablishmentStaffAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id);

        //Task<Response> GetAllAddressesAsync<T>(
        //    string urlBase,
        //    string servicePrefix,
        //    string controller,
        //    string tokenType,
        //    string accessToken);

        Task<Response> GetAddressTypes<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

        Task<Response> GetAllPaymentMethodsAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

        Task<ListResponse<T>> GetListAsync<T>(
                string urlBase,
                string servicePrefix,
                string controller,
                string tokenType,
                string accessToken,
                int id);

        Task<Response> GetCreditBalanceByUserIdAsync<T>(
            int userId,
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);
        Task<Response> GetAvailableBalanceByUserIdAsync<T>(
            int userId,
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

        Task<Response> GetAllSurveysAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);
    }
}


