

using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Models;

namespace WebProjectAPI.Data.Seed
{
    public class PermissionSeeder : IDataSeeder
    {
        public int Order => 3;

        public async Task SeedAsync(AppDbContext db, IServiceProvider sp)
        {
            if (await db.Permissions.AnyAsync()) return;

            var permissions = new List<Permission>
            {
               new Permission { Name = "USER_CREATE", GroupName = "USER" },
            new Permission { Name = "USER_VIEW", GroupName = "USER" },
            new Permission { Name = "USER_EDIT", GroupName = "USER" },
            new Permission { Name = "USER_DELETE", GroupName = "USER" },

            new Permission { Name = "ROLE_CREATE", GroupName = "ROLE" },
            new Permission { Name = "ROLE_VIEW", GroupName = "ROLE" },
            new Permission { Name = "ROLE_EDIT", GroupName = "ROLE" },
            new Permission { Name = "ROLE_DELETE", GroupName = "ROLE" }
            };

            db.Permissions.AddRange(permissions);
            await db.SaveChangesAsync();
        }
    }
}