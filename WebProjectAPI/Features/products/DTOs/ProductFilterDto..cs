namespace WebProjectAPI.Features.products.DTOs
{
    public class ProductFilterDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 8;

        public string? Search { get; set; }

        public List<int> CategoryIds { get; set; } = new();

        public List<int> BrandIds { get; set; } = new();

        public decimal MinPrice { get; set; } = 0;

        public decimal MaxPrice { get; set; } = 100000;

        public string? SortBy { get; set; }
    }
}
