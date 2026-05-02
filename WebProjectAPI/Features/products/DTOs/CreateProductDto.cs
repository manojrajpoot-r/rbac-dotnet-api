using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebProjectAPI.Features.products.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(1, 999999)]
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        [Required]
        [Range(0, 100000)]
        public int Quantity { get; set; }

        public IFormFile? Image { get; set; }


    }
}