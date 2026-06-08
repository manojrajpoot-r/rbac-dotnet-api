using WebProjectAPI.Models;

namespace WebProjectAPI.Models
{
    public class User : AuditLog
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public Tenant Tenant { get; set; } = null!;
        public int Status { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
       = new List<UserRole>();
    }
}

