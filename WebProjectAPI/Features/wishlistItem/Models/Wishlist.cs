using WebProjectAPI.Features.products.Models;

namespace WebProjectAPI.Features.wishlistItem.Models
{
    public class Wishlist
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; }
    }
}
