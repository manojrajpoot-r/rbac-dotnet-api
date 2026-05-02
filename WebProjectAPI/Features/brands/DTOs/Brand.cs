namespace WebProjectAPI.Features.brands.DTOs
{
    public class BrandDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool IsFeatured { get; set; }

        public bool Status { get; set; }
    }
}