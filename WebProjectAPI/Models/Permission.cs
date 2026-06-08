using WebProjectAPI.Models;

namespace WebProjectAPI.Models
{
    public class Permission:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? GroupName { get; set; } = string.Empty;

        public ICollection<RolePermission> RolePermissions { get; set; }
       = new List<RolePermission>();
    }
}

  
   