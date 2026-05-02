using WebProjectAPI.Features.brands.Models;
using WebProjectAPI.Features.product_images.Models;

namespace WebProjectAPI.Features.products.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }

        public bool IsFeatured { get; set; } = false;

        public bool Status { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }
            = new List<ProductImage>();
    }
}