using Microsoft.AspNetCore.Http;

namespace WebProjectAPI.Features.product_images.DTOs
{
    public class ProductImageCreateDto
    {
        public int ProductId { get; set; }

        public List<IFormFile> Images { get; set; } = new();
    }
}