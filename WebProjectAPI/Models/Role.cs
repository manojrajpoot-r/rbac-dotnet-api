using WebProjectAPI.Models;

namespace WebProjectAPI.Models
{

    public class Role:AuditLog
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Tenant Tenant { get; set; } = null!;
        public int Status { get; set; }


        public ICollection<UserRole> UserRoles { get; set; }
      = new List<UserRole>();

        public ICollection<RolePermission> RolePermissions { get; set; }
            = new List<RolePermission>();


    }
}

