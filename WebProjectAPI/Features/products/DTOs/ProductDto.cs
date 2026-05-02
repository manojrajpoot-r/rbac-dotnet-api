using WebProjectAPI.Features.product_images.DTOs;

namespace WebProjectAPI.Features.products.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }


        public int BrandId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string SKU { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }

        public string? Image { get; set; }

        public bool IsFeatured { get; set; }

        public bool Status { get; set; }
        public List<ProductImageDto> Images { get; set; }
     = new();
    }
}