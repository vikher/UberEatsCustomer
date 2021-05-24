using System;

namespace ClubersCustomerMobile.Prism.Models
{
    public class TokenResponse : TransactionResult
    {
        public TokenResult Result { get; set; }
        //public string Token { get; set; }

        //public DateTime Expiration { get; set; }

        //public DateTime ExpirationLocal => Expiration.ToLocalTime();
        public class TokenResult
        {
            public string token { get; set; }
            public DateTime expirationDate { get; set; }
            public DateTime ExpirationLocal => expirationDate.ToLocalTime();
            public string fullName { get; set; }
            public string email { get; set; }
            public object roles { get; set; }
        }
    }
}
