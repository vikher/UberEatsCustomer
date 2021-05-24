using System;

namespace ClubersCustomerMobile.Prism.Models
{
    public class BaseDto<T>
    {
        public T Id { get; set; }
        public DateTime Created { get; set; }
        public int CreatedById { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedById { get; set; }
        public int StatusId { get; set; }
    }
}
