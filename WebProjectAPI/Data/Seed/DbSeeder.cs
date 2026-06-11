namespace WebProjectAPI.Data.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            await SuperAdminSeeder.Seed(serviceProvider);
            await PermissionSeeder.Seed(serviceProvider);
        }
    }
}
