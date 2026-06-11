namespace WebProjectAPI.Data.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentTenantService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId =>
            int.TryParse(
                _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst("userId")?.Value,
                out var id)
                    ? id
                    : null;

        public int? TenantId =>
            int.TryParse(
                _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst("tenantId")?.Value,
                out var id)
                    ? id
                    : null;

        public string? Email =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirst("email")?.Value;
    }
}