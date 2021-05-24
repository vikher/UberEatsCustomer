namespace ClubersCustomerMobile.Prism.Interfaces
{
    public interface IRegexHelper
    {
        bool IsValidEmail(string emailaddress);
        bool IsValidPIN(string pinnumber);

    }
}
