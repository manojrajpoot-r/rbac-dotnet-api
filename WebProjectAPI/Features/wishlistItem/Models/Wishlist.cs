using WebProjectAPI.Features.products.Models;
using WebProjectAPI.Models;

namespace WebProjectAPI.Features.wishlistItem.Models
{
    public class Wishlist : TenantEntity
    {
        

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; }
    }
}
