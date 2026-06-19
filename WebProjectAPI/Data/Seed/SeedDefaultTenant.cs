using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Models;

namespace WebProjectAPI.Data.Seed
{
    public class SeedDefaultTenant : IDataSeeder
    {
        public int Order => 1;

        public async Task SeedAsync(AppDbContext db, IServiceProvider sp)
        {
            if (await db.Tenants.AnyAsync()) return;

            var tenant = new Tenant
            {
                Name = "SuperAdminTenant",
                SubDomain ="techsaga.co.in",
                CreatedAt = DateTime.UtcNow
            };

            db.Tenants.Add(tenant);
            await db.SaveChangesAsync();
        }
    }
}