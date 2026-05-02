using WebProjectAPI.Features.products.Models;

namespace WebProjectAPI.Features.brands.Models
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Image { get; set; }

        public bool IsFeatured { get; set; } = false;

        public bool Status { get; set; } = true;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}