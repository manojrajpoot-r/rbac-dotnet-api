using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebProjectAPI.Features.Categories.Models;

namespace WebProjectAPI.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasMany(x => x.SubCategories)
                   .WithOne(x => x.Category)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
