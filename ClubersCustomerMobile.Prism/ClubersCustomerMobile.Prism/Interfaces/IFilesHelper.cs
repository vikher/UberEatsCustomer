using System.IO;

namespace ClubersCustomerMobile.Prism.Interfaces
{
    public interface IFilesHelper
    {
        byte[] ReadFully(Stream input);
    }
}
