using WebProjectAPI.Features.products.Models;

namespace WebProjectAPI.Features.carts.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; }
    }
}
