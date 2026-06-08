using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebProjectAPI.Features.orders.Models;
namespace WebProjectAPI.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
