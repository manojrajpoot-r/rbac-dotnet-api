namespace WebProjectAPI.Features.sub_categories.DTOs
{
    public class SubCategoryDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool Status { get; set; } = true;

        public string? CategoryName { get; set; }
    }
}
