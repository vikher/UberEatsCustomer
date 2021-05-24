using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Evaluation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Order Order { get; set; }
        public List<Survey> Surveys { get; set; }

    }
}
