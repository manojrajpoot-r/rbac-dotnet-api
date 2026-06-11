
using WebProjectAPI.Data.Services;
namespace WebProjectAPI.Middleware
{


    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenantId = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();

            // Header hai to parse karo
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                if (!int.TryParse(tenantId, out var parsedTenantId))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid TenantId");
                    return;
                }

                context.Items["TenantId"] = parsedTenantId;
            }

            await _next(context);
        }
    }
}
