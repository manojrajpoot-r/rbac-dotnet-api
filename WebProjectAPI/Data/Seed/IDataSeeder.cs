namespace WebProjectAPI.Data.Seed
{
    public interface IDataSeeder
    {
     
            int Order { get; }
            Task SeedAsync(AppDbContext db, IServiceProvider sp);
        
    }
}
