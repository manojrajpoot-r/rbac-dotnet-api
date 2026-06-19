using WebProjectAPI.Models;

namespace WebProjectAPI.Models
{
    public class RolePermission : TenantEntity
    {
       
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }= null!;

    }
}
