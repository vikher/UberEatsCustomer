using System.Collections.Generic;

namespace ClubersCustomerMobile.Prism.Models
{
    public class RoleMenu : BaseDto<int>
    {
        public int RoleId { get; set; }
        public int? UserId { get; set; }
        public int MenuId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public string Name { get; set; }
        public List<RoleMenu> Childs { get; set; } = new List<RoleMenu>();
    }
}
