using WebProjectAPI.Data;
using Microsoft.EntityFrameworkCore;
namespace WebProjectAPI.Helpers.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantMiddleware> _logger;
        public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            // Sirf API requests ke liye tenant resolve karo
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            var host = context.Request.Headers["X-Tenant-Domain"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(host))
            {
                host = context.Request.Host.Host;
            }

            _logger.LogInformation("Method : {Method}", context.Request.Method);
            _logger.LogInformation("Path : {Path}", context.Request.Path);
            _logger.LogInformation("Host : {Host}", host);

            var tenant = await db.Tenants.FirstOrDefaultAsync(x => x.SubDomain == host);

            if (tenant == null)
            {
                _logger.LogInformation("Tenant NOT Found");

                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Tenant not found");
                return;
            }

            _logger.LogInformation("Tenant Id : {TenantId}", tenant.Id);

            context.Items["TenantId"] = tenant.Id;

            await _next(context);
        }
    }
}
