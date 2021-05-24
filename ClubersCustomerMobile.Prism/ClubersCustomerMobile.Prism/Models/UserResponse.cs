using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class UserResponse : TransactionResult
    {
        public UserResult Result { get; set; }
    }

    public class UserResult
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int? SquareId { get; set; }
        //public Square Square { get; set; }
        public int? CellId { get; set; }
        //public Cell Cell { get; set; }
        public int ZoneId { get; set; }
        public int? DepartmentId { get; set; }
        //public Department Department { get; set; }
        public int StatusId { get; set; }
        //public Status Status { get; set; }
        //public List<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
        public Department Department { get; set; }
        public Status Status { get; set; }
        public List<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
        //public Establishments Establishment { get; set; }
        //public string PhoneNumber { get; set; }
        //public string Address { get; set; }
        //public string PicturePath { get; set; }
        public string FullName => $"{firstName} {lastName}";

        //public LoginType LoginType { get; set; }

        /*public string PictureFullPath => string.IsNullOrEmpty(PicturePath)
            ? "https://clubersqa.azurewebsites.net//images/noimage.png"
            : $"https://clubersblob.blob.core.windows.net/users/{PicturePath}";*/
    }
}
