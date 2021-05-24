namespace ClubersCustomerMobile.Prism.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string SecurityCode { get; set; }
        public string BillingPostalCode { get; set; }
        public string Expiration => $"{ExpirationMonth} / {ExpirationYear} ";

    }
}
