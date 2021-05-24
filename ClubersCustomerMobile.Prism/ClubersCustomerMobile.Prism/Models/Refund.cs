using System;
using System.Collections.Generic;
using System.Text;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Refund
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        //public double Price { get; set; }
        //public DateTime OrderDate { get; set; }
        public int OrderId { get; set; }
        public int RefundId { get; set; }
        public DateTime OrderDate { get; set; }
        public string RefundName { get; set; }
        public double Amount { get; set; }
    }
}
