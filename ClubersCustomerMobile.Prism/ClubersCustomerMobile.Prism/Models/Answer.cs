namespace ClubersCustomerMobile.Prism.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Response { get; set; }
        public Question Question { get; set; }
    }
}
