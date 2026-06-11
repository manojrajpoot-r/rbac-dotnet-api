namespace WebProjectAPI.Features.Tenants.DTOs
{
    public class TenantDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Subdomain { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string? AdminName { get; set; }

        public string? AdminEmail { get; set; }
    }

}
