using WebProjectAPI.Features.sub_categories.Models;
using WebProjectAPI.Models;

namespace WebProjectAPI.Features.Categories.Models
{
    public class Category : TenantEntity
    {
        

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;


        public string? Description { get; set; }

        public bool Status { get; set; } = true;

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}