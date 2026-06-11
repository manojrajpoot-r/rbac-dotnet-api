using Microsoft.AspNetCore.Identity;
using WebProjectAPI.Data;
using WebProjectAPI.Models;

public static class SuperAdminSeeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        try
        {
            Console.WriteLine("Seeder Started");

            var db = serviceProvider.GetRequiredService<AppDbContext>();

            var hasher = serviceProvider
                .GetRequiredService<IPasswordHasher<PlatformUser>>();

            if (!db.PlatformUsers.Any())
            {
                var admin = new PlatformUser
                {
                    FullName = "Super Admin",
                    Email = "admin@saas.com",
                    Role = PlatformRole.SuperAdmin,
                    IsActive = true
                };

                admin.PasswordHash =
                    hasher.HashPassword(admin, "Admin@123");

                db.PlatformUsers.Add(admin);

                await db.SaveChangesAsync();

                Console.WriteLine("Super Admin Created");
            }
            else
            {
                Console.WriteLine("Super Admin Already Exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Seeder Error: " + ex);
        }
    }
}