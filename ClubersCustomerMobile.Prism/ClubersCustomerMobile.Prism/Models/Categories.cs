using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<Subcategories> Subcategories { get; set; }

    }
}
