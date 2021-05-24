using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Subcategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImageUrl { get; set; }

		[JsonIgnore]
		public List<Categories> Category { get; set; }
		public List<Establishment> Establishments { get; set; }

	}
}
