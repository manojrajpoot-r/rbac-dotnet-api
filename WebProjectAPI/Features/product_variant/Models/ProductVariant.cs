namespace WebProjectAPI.Features.product_variant.Models
{
    using WebProjectAPI.Features.colors.Models;
    using WebProjectAPI.Features.products.Models;
    using WebProjectAPI.Features.sizes.Models;
    using WebProjectAPI.Models;

    public class ProductVariant : TenantEntity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        public string SKU { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public Product Product { get; set; }

        public Color Color { get; set; }

        public Size Size { get; set; }
    }
}
