using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Establishment
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int AvgPrice { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Condition { get; set; }
        public bool OnSite { get; set; }
        public string LogoUrl { get; set; }
        public string BannerUrl { get; set; }
        public PriceRange PriceRange { get; set; }
        public string LatLng { get; set; }
        public List<Refund> Refunds { get; set; }
        public double DeliveryEstimatedTime { get; set; }
        public double HomeCost { get; set; }
        public double Rating { get; set; }
        public double Distance { get; set; }
        public Subcategories Subcategory { get; set; }
        public List<Product> Products { get; set; }
        public List<Amenity> Amenities { get; set; }
        public List<Photo> Photos { get; set; }

    }
}
