
using WebProjectAPI.Models;
namespace WebProjectAPI.Features.orders.Models
{
    public class OrderItem : TenantEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }= new Order();

        public int ProductId { get; set; }

        public string ProductName { get; set; }= string.Empty;
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
