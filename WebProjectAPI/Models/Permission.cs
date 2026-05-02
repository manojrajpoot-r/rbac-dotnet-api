namespace WebProjectAPI.Models
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? GroupName { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
