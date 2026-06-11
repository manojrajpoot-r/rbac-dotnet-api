namespace WebProjectAPI.Features.Tenants.DTOs
{
    public class UpdateTenantDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string SubDomain { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }

}
