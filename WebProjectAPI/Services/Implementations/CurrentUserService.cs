
using global::WebProjectAPI.Services.Interfaces;
using System.Security.Claims;
using WebProjectAPI.Helpers;


    namespace WebProjectAPI.Services.Implementations
    {
        public class CurrentUserService : ICurrentUserService
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CurrentUserService(
                IHttpContextAccessor httpContextAccessor)
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

            public int? TenantId =>
                int.TryParse(
                    User?.FindFirst(CustomClaims.TenantId)?.Value,
                    out var id)
                        ? id
                        : null;




    

        public bool IsPlatformUser =>
    bool.TryParse(
        _httpContextAccessor.HttpContext?
            .User?
            .FindFirst("IsPlatformUser")?.Value,
        out var value)
            ? value
            : false;




    }
    }

