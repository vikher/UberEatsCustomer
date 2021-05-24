using System.ComponentModel.DataAnnotations;

namespace ClubersCustomerMobile.Prism.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        //[Required]
        //public string CultureInfo { get; set; }
    }
}
