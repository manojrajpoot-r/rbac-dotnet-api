
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Models;

namespace WebProjectAPI.Data.Seed
{
    public class SuperAdminSeeder : IDataSeeder
    {
        public int Order => 5;

        public async Task SeedAsync(AppDbContext db, IServiceProvider sp)
        {
            if (await db.PlatformUsers.AnyAsync(x => x.Email == "super_admin@saas.com"))
                return;
            var hasher = sp.GetRequiredService<IPasswordHasher<PlatformUser>>();
            var role = await db.Roles.FirstOrDefaultAsync(x => x.Name == "SuperAdmin");

            if (role == null)
                throw new Exception("SuperAdmin role not found");

           
             var user = new PlatformUser
            {
                FullName = "Super Admin",
                Email = "super_admin@saas.com",
                 Role = PlatformRole.SuperAdmin,
                 IsActive = true
            };
            user.PasswordHash = hasher.HashPassword(user, "Admin@123");
            db.PlatformUsers.Add(user);
            await db.SaveChangesAsync();
        }
    }
}




















