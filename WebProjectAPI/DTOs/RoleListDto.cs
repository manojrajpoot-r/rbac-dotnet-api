namespace WebProjectAPI.DTOs
{
    public class RoleListDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Status { get; set; }

        public string TenantName { get; set; } = string.Empty;
    }
}
