namespace WebProjectAPI.Data.Services
{
    public interface ICurrentTenantService
    {
        
        int? UserId { get; }
        int? TenantId { get; }
        string? Email { get; }
    }
}
