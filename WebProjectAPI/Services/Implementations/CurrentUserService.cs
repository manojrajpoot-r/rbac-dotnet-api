using System.Security.Claims;
using WebProjectAPI.Helpers;
using WebProjectAPI.Services.Interfaces;

namespace WebProjectAPI.Services.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public int UserId =>
            int.TryParse(
                User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                out var id)
                    ? id
                    : 0;

        //  Tenant Isolation (VERY IMPORTANT)
        public int? TenantId =>
            int.TryParse(
                User?.FindFirst(CustomClaims.TenantId)?.Value,
                out var id)
                    ? id
                    : null;

        // RoleId (ADD THIS)
        public int? RoleId =>
            int.TryParse(
                User?.FindFirst(ClaimTypes.Role)?.Value,
                out var roleId)
                    ? roleId
                    : null;

        //  Platform User Flag
        public bool IsPlatformUser =>
            bool.TryParse(
                User?.FindFirst("IsPlatformUser")?.Value,
                out var value)
                    ? value
                    : false;

        //  Optional: Permissions (BEST PRACTICE ADDITION)
        public List<string> Permissions =>
            User?.FindAll("permission")
                .Select(x => x.Value)
                .ToList()
            ?? new List<string>();
    }
}