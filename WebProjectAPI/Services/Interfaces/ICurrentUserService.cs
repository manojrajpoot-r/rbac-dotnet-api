namespace WebProjectAPI.Services.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        int? TenantId { get; }
        bool IsPlatformUser { get; }
    }
}
