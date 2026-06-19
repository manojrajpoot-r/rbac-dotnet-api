using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Models;

namespace WebProjectAPI.Data.Seed
{
    public class RolePermissionSeeder : IDataSeeder
    {
        public int Order => 4;

        public async Task SeedAsync(AppDbContext db, IServiceProvider sp)
        {
            if (await db.RolePermissions.AnyAsync()) return;

            var role = await db.Roles.FirstOrDefaultAsync();
            var permissions = await db.Permissions.ToListAsync();

            if (role == null || !permissions.Any())
                return;

            foreach (var perm in permissions)
            {
                db.RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = perm.Id
                });
            }

            await db.SaveChangesAsync();
        }
    }
}