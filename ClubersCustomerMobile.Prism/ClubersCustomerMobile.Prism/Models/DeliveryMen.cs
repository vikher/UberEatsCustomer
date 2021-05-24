using System;
using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class DeliveryMen
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Rfc { get; set; }
        public object FrontImageId { get; set; }
        public object BackImageId { get; set; }
        public object PerfilImageId { get; set; }
        public string FullAddress { get; set; }
        public string LatLong { get; set; }
        public int BackpackTypeId { get; set; }
        public int GenderId { get; set; }
        public int IdSquare { get; set; }
        public object BagId { get; set; }
        public int UserId { get; set; }
        public List<Vehicle> vehicles { get; set; }

    }
}
