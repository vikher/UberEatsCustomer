using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imageUrl { get; set; }
        public List<Subcategory> subcategories { get; set; }
    }
}
