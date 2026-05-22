namespace WebProjectAPI.Features.products.DTOs
{
    public class CategoryWithProductsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SubCategoryWithProductsDto>
   
    SubCategories
        { get; set; }
    }
}
