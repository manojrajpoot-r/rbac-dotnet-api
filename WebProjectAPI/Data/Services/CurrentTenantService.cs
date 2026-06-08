
namespace WebProjectAPI.Data.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? TenantId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .Request.Headers["X-Tenant-Id"]
                    .FirstOrDefault();

                if (Guid.TryParse(value, out var tenantId))
                {
                    return tenantId;
                }

                return null;
            }
        }


    }

}
