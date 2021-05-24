using ClubersCustomerMobile.Prism.Enums;
using ClubersCustomerMobile.Prism.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ClubersCustomerMobile.Prism.Services
{
    public class ApiService : IApiService
    {

        List<AddressType> AddressTypes = new List<AddressType>()
                {
                    new AddressType(){Id = 1, Name = "Trabajo" },
                    new AddressType(){Id = 2, Name = "Casa"},
                };


        List<CreditBalance> CreditBalances = new List<CreditBalance>()
        { 
            new CreditBalance(){Id = 1, OrderId = "123456", OrderType = "A domicilio", NameStatus = "SIN EVALUAR Y SIN COMPARTIR", StartDate = DateTime.Now, Total = 175, Quantity = 3 },
            new CreditBalance(){Id = 2, OrderId = "123456", OrderType = "A domicilio", NameStatus = "SIN COMPARTIR", StartDate = DateTime.Now, Total = 175, Quantity = 3 },
            new CreditBalance(){Id = 3, OrderId = "RT5456", OrderType = "En sitio", NameStatus = "SIN EVALUAR Y SIN COMPARTIR", StartDate = DateTime.Now, Total = 225, Quantity = 3 }
        };

        //List<Address> Addresses = new List<Address>()
        //{
        //    new Address(){Id = 1, Street = "Valle de mitla" , Number = "101", Suburb = "Fuentes", City = "Ecatepec", PostalCode = 55210 , State = " Estado de Mexico" , AddressType = "Trabajo", References = "Trabajo azul" },
        //    new Address(){Id = 2, Street = "Calle Nuevo Leon" , Number = "45", Suburb = "Roma", City = "Benito Juarez", PostalCode = 55410 , State = "CDMX" , AddressType = "Casa", References = "casa azul" }
        // };

        List<PaymentMethod> PaymentMethods = new List<PaymentMethod>()
        {
            new PaymentMethod(){Id = 1, CardNumber = "1231456578984582" , PaymentType = "visa" , ExpirationMonth = "03" ,ExpirationYear = "20", BillingPostalCode = "55210", SecurityCode = "455" },
            new PaymentMethod(){Id = 2, CardNumber = "1231456578984582" , PaymentType = "mastercard" , ExpirationMonth = "03" ,ExpirationYear = "20", BillingPostalCode = "55210", SecurityCode = "455" }

         };
        List<AvailableBalance> AvailableBalance = new List<AvailableBalance>()
        {
            new AvailableBalance(){ OrderDate = DateTime.Now, RefundName = "Reembolso recurrente segunda visita", EffectiveDate = DateTime.Now, ExpirationDate = DateTime.Now, Total = 800 },
            new AvailableBalance(){ OrderDate = DateTime.Now, RefundName = "Reembolso recurrente", EffectiveDate = DateTime.Now, ExpirationDate = DateTime.Now, Total = 500 },
            new AvailableBalance(){ OrderDate = DateTime.Now, RefundName = "Reembolso de bienvenido", EffectiveDate = DateTime.Now, ExpirationDate = DateTime.Now, Total = 600 },
        };

            List<Survey> Surveys = new List<Survey>()
                    {
                        new Survey(){Id = 1, Name = "ENCUESTA DE RECURRENTE A DOMICILIO", Questions = new ObservableCollection<Question>()}
                    };

            ObservableCollection<Question> Questions = new ObservableCollection<Question>()
                    {
                         new Question(){Id = 1, QuestionType = "Establishment", Interrogation = "¿Se tomaron en cuenta las notas de tu pedido?",  Survey = new Survey(), Answers = new List<Answer>()},
                         new Question(){Id = 2, QuestionType = "Establishment", Interrogation = "Tomando como base tu pedido anterior, consideras en general que el producto:",  Survey = new Survey(), Answers = new List<Answer>()},
                         new Question(){Id = 3, QuestionType = "Delivery", Interrogation = "¿Qué te gustó más?",  Survey = new Survey(), Answers = new List<Answer>()},
                         new Question(){Id = 4, QuestionType = "Delivery", Interrogation = "¿Qué puede mejorar?",  Survey = new Survey(), Answers = new List<Answer>()}
                    };

            List<Answer> Answers = new List<Answer>()
                    {
                        new Answer(){Id = 1, Response = "A. Si", Question = new Question()},
                        new Answer(){Id = 2, Response = "B. No", Question = new Question()},
                        new Answer(){Id = 3, Response = "A.	Mejoró ", Question = new Question()},
                        new Answer(){Id = 4, Response = "B.	Esta igual ", Question = new Question()},
                        new Answer(){Id = 5, Response = "C.	Empeoró ", Question = new Question()},

                         new Answer(){Id = 6, Response = "A. Llegó a tiempo", Question = new Question()},
                        new Answer(){Id = 7, Response = "B.	Buena presentación y limpieza personal", Question = new Question()},
                        new Answer(){Id = 8, Response = "C.	Trato amable y profesional", Question = new Question()},
                         new Answer(){Id = 9, Response = "A. Tiempo de entrega", Question = new Question()},
                        new Answer(){Id = 10, Response = "B. Presentación y limpieza personal", Question = new Question()},
                        new Answer(){Id = 11, Response = "C. Trato amable y profesional ", Question = new Question()}
                    };
        public void Seeder()
        {
            Answers[0].Question = Questions[0];
            Answers[1].Question = Questions[0];
            Answers[2].Question = Questions[1];
            Answers[3].Question = Questions[1];
            Answers[4].Question = Questions[1];

            Answers[5].Question = Questions[2];
            Answers[6].Question = Questions[2];
            Answers[7].Question = Questions[2];
            Answers[8].Question = Questions[3];
            Answers[9].Question = Questions[3];
            Answers[10].Question = Questions[3];

            Questions[0].Answers.Add(Answers[0]);
            Questions[0].Answers.Add(Answers[1]);
            Questions[1].Answers.Add(Answers[2]);
            Questions[1].Answers.Add(Answers[3]);
            Questions[1].Answers.Add(Answers[4]);
            Questions[2].Answers.Add(Answers[5]);
            Questions[2].Answers.Add(Answers[6]);
            Questions[2].Answers.Add(Answers[7]);
            Questions[3].Answers.Add(Answers[8]);
            Questions[3].Answers.Add(Answers[9]);
            Questions[3].Answers.Add(Answers[10]);

            Surveys[0].Questions = Questions;

        }
        public bool CheckConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
        public async Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest)
        {
            try
            {
                string request = JsonConvert.SerializeObject(userRequest);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();
                Response obj = JsonConvert.DeserializeObject<Response>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        //public async Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken)
        //{
        //    try
        //    {
        //        string request = JsonConvert.SerializeObject(model);
        //        StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
        //        HttpClient client = new HttpClient
        //        {
        //            BaseAddress = new Uri(urlBase)
        //        };

        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
        //        string url = $"{servicePrefix}{controller}";
        //        HttpResponseMessage response = await client.PostAsync(url, content);
        //        string answer = await response.Content.ReadAsStringAsync();

        //        Response ResponseResult = JsonConvert.DeserializeObject<Response>(answer);
        //        if (ResponseResult.ResultCode != ResultCode.Success)
        //        {
        //            return new Response
        //            {
        //                ResultCode = ResultCode.Warning,
        //                ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
        //            };
        //        }

        //        //T obj = JsonConvert.DeserializeObject<T>(answer);
        //        return new Response
        //        {
        //            ResultCode = ResultCode.Success,
        //            Result = true
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            ResultCode = ResultCode.Warning,
        //            ResultMessages = new List<string> { ex.Message }
        //        };
        //    }
        //}

        public async Task<Response> PostAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken )
        {
            try
            {
                string requestString = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(requestString, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                Response ResponseResult = JsonConvert.DeserializeObject<Response>(result);
                if (ResponseResult.ResultCode != ResultCode.Success)
                {
                    return new Response
                    {
                        ResultCode = ResultCode.Warning,
                        ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
                    };
                }

                //EstablishmentStaffDetailResponse establishmentStaff = JsonConvert.DeserializeObject<EstablishmentStaffDetailResponse>(result);
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request)
        {
            try
            {
                string requestString = JsonConvert.SerializeObject(request);
                StringContent content = new StringContent(requestString, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                Response ResponseResult = JsonConvert.DeserializeObject<Response>(result);
                if (ResponseResult.ResultCode != ResultCode.Success)
                {
                    return new Response
                    {
                        ResultCode = ResultCode.Warning,
                        ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
                    };
                }

                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    //IsSuccess = true,
                    Result = token
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetUserById(string urlBase,string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}/{1}";
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                Response ResponseResult = JsonConvert.DeserializeObject<Response>(result);
                if (ResponseResult.ResultCode != ResultCode.Success)
                {
                    return new Response
                    {
                        ResultCode = ResultCode.Warning,
                        ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
                    };
                }

                UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(result);


                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = userResponse
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetUserByEmailAsync(string urlBase, string servicePrefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}/{email}";
                HttpResponseMessage response = await client.GetAsync(url);
                string answer = await response.Content.ReadAsStringAsync();

                Response ResponseResult = JsonConvert.DeserializeObject<Response>(answer);
                if (ResponseResult.ResultCode != ResultCode.Success)
                {
                    return new Response
                    {
                        ResultCode = ResultCode.Warning,
                        ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
                    };
                }

                UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(answer);
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = userResponse
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<RecoverPasswordResponse> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest)
        {
            try
            {
                string request = JsonConvert.SerializeObject(emailRequest);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                string url = $"{servicePrefix}{controller}";
                HttpResponseMessage response = await client.PostAsync(url, content);
                string answer = await response.Content.ReadAsStringAsync();
                RecoverPasswordResponse obj = JsonConvert.DeserializeObject<RecoverPasswordResponse>(answer);
                return obj;
            }
            catch (Exception ex)
            {
                return new RecoverPasswordResponse
                {
                    result = false,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response1<DeliveryMen>> GetDeliveryManAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}/{id}";
                HttpResponseMessage response = await client.GetAsync(url);
                string answer = await response.Content.ReadAsStringAsync();

                Response1<DeliveryMen> ResponseResult = JsonConvert.DeserializeObject<Response1<DeliveryMen>>(answer);
                if (ResponseResult.ResultCode != ResultCode.Success)
                {
                    return new Response1<DeliveryMen>
                    {
                        ResultCode = ResultCode.Warning,
                        ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
                    };
                }

                return new Response1<DeliveryMen>
                {
                    ResultCode = ResultCode.Success,
                    Result = ResponseResult.Result
                };
            }
            catch (Exception ex)
            {
                return new Response1<DeliveryMen>
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response1<Membership>> GetMembershipAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}/{id}";
                HttpResponseMessage response = await client.GetAsync(url);
                string answer = await response.Content.ReadAsStringAsync();

                Response1<Membership> ResponseResult = JsonConvert.DeserializeObject<Response1<Membership>>(answer);
                if (ResponseResult.ResultCode != ResultCode.Success)
                {
                    return new Response1<Membership>
                    {
                        ResultCode = ResultCode.Warning,
                        ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
                    };
                }

                return new Response1<Membership>
                {
                    ResultCode = ResultCode.Success,
                    Result = ResponseResult.Result
                };
            }
            catch (Exception ex)
            {
                return new Response1<Membership>
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response1<Waiter>> GetEstablishmentStaffAsync(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                string url = $"{servicePrefix}{controller}/{id}";
                HttpResponseMessage response = await client.GetAsync(url);
                string answer = await response.Content.ReadAsStringAsync();

                Response1<Waiter> ResponseResult = JsonConvert.DeserializeObject<Response1<Waiter>>(answer);
                if (ResponseResult.ResultCode != ResultCode.Success)
                {
                    return new Response1<Waiter>
                    {
                        ResultCode = ResultCode.Warning,
                        ResultMessages = new List<string> { ResponseResult.ResultMessages[0] }
                    };
                }

                return new Response1<Waiter>
                {
                    ResultCode = ResultCode.Success,
                    Result = ResponseResult.Result
                };
            }
            catch (Exception ex)
            {
                return new Response1<Waiter>
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetAddressTypes<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                //Seeder();
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = AddressTypes
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetAllPaymentMethodsAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                //Seeder();
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = PaymentMethods
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        //public async Task<Response> GetAllAddressesAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        //{
        //    try
        //    {
        //        //Seeder();
        //        return new Response
        //        {
        //            ResultCode = ResultCode.Success,
        //            Result = Addresses
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            ResultCode = ResultCode.Warning,
        //            ResultMessages = new List<string> { ex.Message }
        //        };
        //    }
        //}
        public async Task<ListResponse<T>> GetListAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, int id = 0)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    tokenType,
                    accessToken);

                var url = (id != 0) ? $"{servicePrefix}{controller}/{id}" : $"{servicePrefix}{controller}";

                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<ListResponse<T>>(result);

                if (!response.IsSuccessStatusCode)
                {
                    return new ListResponse<T>
                    {
                        ResultCode = ResultCode.Success,
                        ResultMessages = new List<string> { list.ResultMessages[0] },
                    };
                }

                return new ListResponse<T>
                {
                    ResultCode = ResultCode.Success,
                    Result = list.Result
                };
            }
            catch (Exception ex)
            {
                return new ListResponse<T>
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetCreditBalanceByUserIdAsync<T>(int userId,  string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = CreditBalances

                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetAvailableBalanceByUserIdAsync<T>(int userId, string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = AvailableBalance
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public async Task<Response> GetAllSurveysAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                return new Response
                {
                    ResultCode = ResultCode.Success,
                    Result = Surveys
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    ResultCode = ResultCode.Warning,
                    ResultMessages = new List<string> { ex.Message }
                };
            }
        }
        public ApiService()
        {
            Seeder();
        }
    }
}
