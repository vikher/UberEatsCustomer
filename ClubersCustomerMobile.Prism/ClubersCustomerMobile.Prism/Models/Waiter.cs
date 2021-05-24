using System;

namespace ClubersCustomerMobile.Prism.Models
{
    public class Waiter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string EstablishmentName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string IneFrontUrl { get; set; }
        public string IneBackUrl { get; set; }
    }
}
