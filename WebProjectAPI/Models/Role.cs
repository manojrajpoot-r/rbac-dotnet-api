using WebProjectAPI.Models;

namespace WebProjectAPI.Models
{
  
        public class Role : AuditLog
        {
            public string Name { get; set; } = string.Empty;

            public int Status { get; set; }

           
        public Tenant? Tenant { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
                = new List<UserRole>();

            public ICollection<RolePermission> RolePermissions { get; set; }
                = new List<RolePermission>();
        }
    
}

