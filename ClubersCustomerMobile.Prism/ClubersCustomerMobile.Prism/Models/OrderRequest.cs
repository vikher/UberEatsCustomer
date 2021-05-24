using System;
using System.Collections.Generic;
using System.Text;

namespace ClubersCustomerMobile.Prism.Models
{
    public class OrderRequest
    {
        public int EstablishmentId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int ConsumptionTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public int DeliveryFee { get; set; }
        public double TipAmount { get; set; }
        public int TrackingStatusId { get; set; }
        public int EstablishmentStaffId { get; set; }
        public double Total { get; set; }
        public string TableNumber { get; set; }
        public List<Product> OrderProducts { get; set; }
    }
}
