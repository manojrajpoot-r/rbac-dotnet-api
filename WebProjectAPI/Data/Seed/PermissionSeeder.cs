using WebProjectAPI.Models;

namespace WebProjectAPI.Data.Seed
{
    public static class PermissionSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<AppDbContext>();

            if (!db.Permissions.Any())
            {
                db.Permissions.AddRange(
                    new Permission { Name = "USER_CREATE", GroupName = "Create User" },
                    new Permission { Name = "USER_VIEW", GroupName = "View User" },
                    new Permission { Name = "USER_DELETE", GroupName = "Delete User" }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}   
