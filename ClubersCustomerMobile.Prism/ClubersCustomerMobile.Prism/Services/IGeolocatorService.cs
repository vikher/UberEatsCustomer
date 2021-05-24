using System.Threading.Tasks;

namespace ClubersCustomerMobile.Prism.Services
{
    public interface IGeolocatorService
    {
        double Latitude { get; set; }

        double Longitude { get; set; }

        Task GetLocationAsync();
    }
}