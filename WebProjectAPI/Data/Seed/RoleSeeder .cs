using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Models;

namespace WebProjectAPI.Data.Seed
{
    public class RoleSeeder : IDataSeeder
    {
        public int Order => 2;

        public async Task SeedAsync(AppDbContext db, IServiceProvider sp)
        {
            if (await db.Roles.AnyAsync()) return;

            var tenant = await db.Tenants.FirstOrDefaultAsync();

            if (tenant == null)
                throw new Exception("Tenant not found for Role seeding");

            db.Roles.Add(new Role
            {
                Name = "SuperAdmin",
                Status = 1,
                TenantId = tenant.Id
            });

            await db.SaveChangesAsync();
        }
    }
}