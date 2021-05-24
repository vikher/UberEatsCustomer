using System;
using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Membership
    {
        public int MembershipId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<string> Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ChargeTypeId { get; set; }
        public string ChargeType { get; set; }
    }
}
