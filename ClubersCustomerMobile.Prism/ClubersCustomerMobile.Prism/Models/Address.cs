namespace ClubersCustomerMobile.Prism.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string FullAddress { get; set; }
        public string LatLng { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressType { get; set; }
        public string CustomType { get; set; }
        public string Comments { get; set; }
        public string? GeoAddress { get; set; }
    }
}
