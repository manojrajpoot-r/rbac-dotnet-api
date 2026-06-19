using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Data.Services;
using WebProjectAPI.Features.booking.Models;
using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.carts.Models;
using WebProjectAPI.Features.Categories.Models;
using WebProjectAPI.Features.colors.Models;
using WebProjectAPI.Features.orders.Models;
using WebProjectAPI.Features.payments.Models;
using WebProjectAPI.Features.plans.Models;
using WebProjectAPI.Features.product_images.Models;
using WebProjectAPI.Features.product_variant.Models;
using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Features.sizes.Models;
using WebProjectAPI.Features.sub_categories.Models;
using WebProjectAPI.Features.subscription.Models;
using WebProjectAPI.Features.wishlistItem.Models;
using WebProjectAPI.Models;

namespace WebProjectAPI.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ICurrentTenantService _tenantService;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ICurrentTenantService tenantService)
            : base(options)
        {
            _tenantService = tenantService;
        }

        // Shared
        public DbSet<PlatformUser> PlatformUsers => Set<PlatformUser>();
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        // Identity
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        // Ecommerce
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<SubCategory> SubCategories => Set<SubCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<Wishlist> Wishlists => Set<Wishlist>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<Color> Colors => Set<Color>();
        public DbSet<Size> Sizes => Set<Size>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

        // Beauty Parlour
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingServiceItem> BookingServiceItems => Set<BookingServiceItem>();

        public DbSet<Plan> Plans { get; set; }

        public DbSet<TenantSubscription> TenantSubscriptions { get; set; }

        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Automatically load all IEntityTypeConfiguration classes
            builder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            ApplyGlobalFilters(builder);
        }

        private void ApplyGlobalFilters(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasQueryFilter(x =>
                    !x.IsDeleted &&
                    (_tenantService.TenantId == null ||
                     x.TenantId == _tenantService.TenantId));

            builder.Entity<Role>()
                .HasQueryFilter(x =>
                    !x.IsDeleted &&
                    (_tenantService.TenantId == null ||
                     x.TenantId == _tenantService.TenantId));

            
        }
    }
}