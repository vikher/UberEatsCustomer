using System;

namespace ClubersCustomerMobile.Prism.Models
{
    public class AvailableBalance
    {
        public DateTime OrderDate { get; set; }
        public string RefundName { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Total { get; set; }
    }
}
