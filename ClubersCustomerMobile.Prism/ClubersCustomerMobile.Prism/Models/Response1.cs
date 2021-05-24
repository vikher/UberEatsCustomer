namespace ClubersCustomerMobile.Prism.Models
{
    public class Response1<T> : TransactionResult
    {
        public T Result { get; set; }
    }
}
