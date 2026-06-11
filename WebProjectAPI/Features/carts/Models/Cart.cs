using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Models;

namespace WebProjectAPI.Features.carts.Models
{
    public class Cart : TenantEntity
    {
      

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; }
    }
}
