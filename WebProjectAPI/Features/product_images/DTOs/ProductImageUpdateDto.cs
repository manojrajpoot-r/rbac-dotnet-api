using Microsoft.AspNetCore.Http;

namespace WebProjectAPI.Features.product_images.DTOs
{
    public class ProductImageUpdateDto
    {
        public int ProductId { get; set; }

        public bool IsMain { get; set; }

        public IFormFile? Image { get; set; }
    }
}