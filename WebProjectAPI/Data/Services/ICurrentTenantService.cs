namespace WebProjectAPI.Data.Services
{
    public interface ICurrentTenantService
    {
        Guid? TenantId { get; }
    }
}
