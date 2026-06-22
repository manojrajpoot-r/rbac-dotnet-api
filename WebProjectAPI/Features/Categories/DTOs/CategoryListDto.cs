using WebProjectAPI.Features.products.DTOs;

namespace WebProjectAPI.Features.Categories.DTOs
{

    public class CategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool Status {get; set;}


    }
}