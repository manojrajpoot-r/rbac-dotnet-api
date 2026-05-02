using Microsoft.AspNetCore.Http;

namespace WebProjectAPI.Features.products.DTOs
{
    public class UpdateProductDto
    {
        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int BrandId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }

        public IFormFile? Image { get; set; }
    }
}