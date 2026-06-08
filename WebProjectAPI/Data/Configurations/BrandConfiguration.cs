using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebProjectAPI.Features.brands.Models;

namespace WebProjectAPI.Data.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasMany(x => x.Products)
                   .WithOne(x => x.Brand)
                   .HasForeignKey(x => x.BrandId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
