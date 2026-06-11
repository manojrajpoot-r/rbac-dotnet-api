using System.Security.Claims;
using WebProjectAPI.Attributes;
using WebProjectAPI.Data;

namespace WebProjectAPI.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            var endpoint = context.GetEndpoint();
            var permissionAttr = endpoint?.Metadata.GetMetadata<PermissionAttribute>();

            if (permissionAttr == null)
            {
                await _next(context);
                return;
            }

            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var userIdInt = int.Parse(userId);

            var roles = db.UserRoles
                .Where(ur => ur.UserId == userIdInt)
                .Select(ur => ur.Role.Name)
                .ToList();

            // Admin bypass
            //if (roles.Any(r => r.Equals("Admin", StringComparison.OrdinalIgnoreCase)))
            //{
            //    await _next(context);
            //    return;
            //}



       

            // SuperAdmin bypass
            if (context.User.IsInRole("SuperAdmin"))
            {
                await _next(context);
                return;
            }


            var hasPermission = db.UserRoles
                .Where(ur => ur.UserId == userIdInt)
                .Join(db.RolePermissions,
                    ur => ur.RoleId,
                    rp => rp.RoleId,
                    (ur, rp) => rp.PermissionId)
                .Join(db.Permissions,
                    rp => rp,
                    p => p.Id,
                    (rp, p) => p.Name)
                .Any(p => p == permissionAttr.Name);

            if (!hasPermission)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            await _next(context);
        }
    }
}