using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class ListResponse<T> : TransactionResult
    {
        public List<T> Result { get; set; }
    }
}
