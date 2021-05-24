using System;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartDateLocal => StartDate.ToLocalTime();
        public DateTime? EndDate { get; set; }
        public DateTime? EndDateLocal => EndDate?.ToLocalTime();
        public string Source { get; set; }
        public string Target { get; set; }
        public float Qualification { get; set; }
        public double SourceLatitude { get; set; }
        public double SourceLongitude { get; set; }
        public double TargetLatitude { get; set; }
        public double TargetLongitude { get; set; }
        public string Remarks { get; set; }
        //public string Address { get; set; }
        public string Status { get; set; }
        public int OrderDeliveryNumber { get; set; }
        public string DeliveryDetails { get; set; }
        public Order Order { get; set; }
        public DeliveryMen DeliveryMen { get; set; }

    }
}
