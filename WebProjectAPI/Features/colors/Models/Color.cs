using WebProjectAPI.Models;

namespace WebProjectAPI.Features.colors.Models
{
    public class Color : TenantEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
