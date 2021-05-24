using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Order : BindableBase
    {


        public int Quantity { get; set; }
        public int EstablishmentId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int ConsumptionTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public int? DeliveryFee { get; set; }
        public int? TipId { get; set; }
        public int TrackingStatusId { get; set; }
        public int? EstablishmentStaffId { get; set; }
        public string? TableNumber { get; set; }
        public int trackingStatusId { get; set; }
        //public List<OrderProduct> orderProducts { get; set; }
        public int Id { get; set; }
        public string TypeOfOrder { get; set; }
        public string OrderStatus { get; set; }
        public Tip? Tip { get; set; }
        public Refund? Refund { get; set; }
        public Establishment? Establishment { get; set; }
        public Delivery? Delivery { get; set; }
        public Evaluation? Evaluation { get; set; }
        public decimal PaidTotal { get; set; }
        public double ProductsCount => Products.Sum(x => x.Quantity);
        public double TotalPrice => (Products.Sum(x => x.Total) + Tip.Amount);
        public double OnSiteTotalPrice => (Products.Sum(x => x.OnSiteTotal) + Tip.Amount);
        public double Total { get; set; }

        private List<Product> _products;
        public List<Product> Products
        {
            get => _products; 
            set => SetProperty(ref _products, value);
        }
    }
}
