namespace WebProjectAPI.Models
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string SubDomain { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; }
            = new List<User>();

        public ICollection<Role> Roles { get; set; }
            = new List<Role>();
    }
}
