namespace ClubersCustomerMobile.Prism.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Home { get; set; }
        public bool Onsite { get; set; }
        public string StartTimeOnSite { get; set; }
        public string EndTimeOnSite { get; set; }
        public string StartTimeHome { get; set; }
        public string EndTimeHome { get; set; }
    }
}
