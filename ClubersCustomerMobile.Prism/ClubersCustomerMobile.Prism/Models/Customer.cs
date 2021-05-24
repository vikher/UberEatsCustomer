namespace ClubersCustomerMobile.Prism.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int memberId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName} {SecondLastName}";
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string ImageUrl { get; set; }
    }
}
