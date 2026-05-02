using Microsoft.AspNetCore.Http;

namespace WebProjectAPI.Features.brands.DTOs
{
    public class UpdateBrandDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}