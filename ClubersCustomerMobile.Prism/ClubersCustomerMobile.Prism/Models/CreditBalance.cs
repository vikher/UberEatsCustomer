using System;
namespace ClubersCustomerMobile.Prism.Models
{
    public class CreditBalance
    {
        public int Id { get; set; }
        public string NameStatus { get; set; }
        public string OrderId { get; set; }
        public string OrderType { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
    }
}