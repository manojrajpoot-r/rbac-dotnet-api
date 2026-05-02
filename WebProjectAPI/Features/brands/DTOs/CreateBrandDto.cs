using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebProjectAPI.Features.brands.DTOs
{
    public class CreateBrandDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }
    }
}