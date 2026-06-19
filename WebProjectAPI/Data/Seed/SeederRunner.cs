using WebProjectAPI.Data;
using WebProjectAPI.Data.Seed;

public static class SeederRunner
{
    public static async Task RunAsync(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var seeders = scope.ServiceProvider
            .GetServices<IDataSeeder>()
            .OrderBy(x => x.Order)
            .ToList();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(db, scope.ServiceProvider);
        }
    }
}