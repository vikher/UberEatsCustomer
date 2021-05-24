using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Submenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public int ComponentsMax { get; set; }
        public bool Required { get; set; }
        public int TotalComponents { get; set; }
        public List<Component> components { get; set; }
    }
}
