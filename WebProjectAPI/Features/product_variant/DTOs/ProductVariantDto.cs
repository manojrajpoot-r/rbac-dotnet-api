namespace WebProjectAPI.Features.product_variant.DTOs
{
    public class ProductVariantDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int ColorId { get; set; }

        public string ColorName { get; set; }

        public int SizeId { get; set; }

        public string SizeName { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string SKU { get; set; }
    }
}
