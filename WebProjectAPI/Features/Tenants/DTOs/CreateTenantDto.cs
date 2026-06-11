namespace WebProjectAPI.Features.Tenants.DTOs
{
    public class CreateTenantDto
    {
        public string Name { get; set; } = string.Empty;

        public string SubDomain { get; set; } = string.Empty;

        public string AdminName { get; set; } = string.Empty;

        public string AdminEmail { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }

}
