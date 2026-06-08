using WebProjectAPI.Models;

namespace WebProjectAPI.Features.sizes.Models
{
    public class Size : TenantEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
