using System.ComponentModel.DataAnnotations;

namespace WebProjectAPI.Features.Categories.DTOs
{
 
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
