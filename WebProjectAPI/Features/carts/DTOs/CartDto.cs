namespace WebProjectAPI.Features.carts.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }
    }
}
